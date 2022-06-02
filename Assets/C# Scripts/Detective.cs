using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Detective : MonoBehaviour
{
    //Display the state/statements of the agent
    public TextMeshProUGUI detectiveSpeech;
    //Reference the player gameobject
    public GameObject player;
    //store the clues' transforms into an array
    public Transform[] clues;
    //the index of the clue
    int currClue = 0;
    //reference and store the current target's transform
    Transform _currTarget;
    //find the clue tag to know which objects need to be stored
    public string clueTag;
    //Reference the nav mesh agent to track
    public NavMeshAgent agent;
    //the dist where the player is close enough to the player
    public float closeDist;
    //check if the investigation has started
    bool igIsStarted;
    //check if the player is ready
    bool ready = false;
    //find the idle pos
    public Vector3 idlePos;
    //check if the player has already gotten close to the detective once
    bool metUp = false;


    void Start()
    {
        //find the gameobjects that contain the clue tag and add them to the tranmsform array
        clues = GameObject.FindGameObjectsWithTag(clueTag)
            .Select(go => go.transform)
            .OrderBy(go => go.name)
            .ToArray();
    }

    #region "Tasks"

    //Task to move the agent to the current target
    [Task]
    void GoTo(string tag)
    {
        if (_currTarget == null)
        {
            _currTarget = GameObject.FindGameObjectWithTag(tag).transform;
        }
        agent.stoppingDistance = 0.5f;
        if (tag == "clue")
        {
            _currTarget = clues[currClue];
        }
        if (agent.destination != _currTarget.transform.position)
        {
            agent.SetDestination(_currTarget.position);
        }
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Task.current.Succeed();
            _currTarget = null;
        }
    }

    //Show that the current state of the agent is idle
    [Task]
    bool Idle()
    {
        return true;
    }

    //check if the player is close enough to the agent
    [Task]
    bool isClose(string tag)
    {
        player = GameObject.FindGameObjectWithTag(tag);
        float distBetween = Vector3.Distance(player.transform.position, agent.destination);
        return distBetween < closeDist;
    }

    //set to display the dialogue of the agent 
    [Task]
    void DetectiveLines(string lines)
    {
        this.detectiveSpeech.text = lines;
        Task.current.Succeed();
    }

    //check if the playuer is ready
    [Task]
    bool PlayerReady(bool n)
    {
        if(n == true)
        {
            ready = true;
        }
        else
        {
            ready = false;
        }
        return true;
    }

    //set the player to be ready
    [Task]
    bool PlayerIsReady()
    {
        return ready;
    }

    //start the investigation 
    [Task]
    bool StartInvestigation()
    {
        currClue = 0;
        igIsStarted = true;
        return true;
    }

    //if the investigation has been started
    [Task]
    bool isInvestigating()
    {
        return igIsStarted;
    }

    //set to end the invesigation 
    [Task]
    bool EndInvestigation()
    {
        currClue = 0;
        igIsStarted = false;
        return true;
    }

    //check if the player and agent have met once
    [Task]
    bool Met()
    {
        return metUp;
    }

    //set if the player has met the agnent
    [Task]
    bool haveMet()
    {
        metUp = true;
        return true;
    }

    //increase the index of the clue and show which clue number it is
    [Task]
    bool NoteDown()
    {
        detectiveSpeech.text = "Searching Clue: " + (currClue + 1);
        currClue++;
        return true;
    }

    //check which clues are valid to go to
    [Task]
    bool ValidClue()
    {
        if (currClue < clues.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //check to see if all the clues have been found
    [Task]
    bool AllCluesFound()
    {
        return currClue >= 5;
    }
    #endregion

    void Update()
    {
        Debug.Log(AllCluesFound());
    }
}