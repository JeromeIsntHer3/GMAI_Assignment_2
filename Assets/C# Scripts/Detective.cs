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
    public TextMeshProUGUI text;

    public GameObject player;

    public Transform[] clues;
    int _clueIndex = 0;
    Transform _currTarget;
    public string clueTag;

    public NavMeshAgent agent;


    void Start()
    {
        clues = GameObject.FindGameObjectsWithTag(clueTag)
            .Select(go => go.transform)
            .OrderBy(go => go.name)
            .ToArray();
    }

    void Update()
    {

    }

    #region "Tasks"

    [Task]
    void MoveTo(string tag)
    {
        if(_currTarget == null)
        {
            _currTarget = GameObject.FindGameObjectWithTag(tag).transform;
        }
        agent.stoppingDistance = 0.5f;
        if(tag == "clue")
        {
            _currTarget = clues[_clueIndex];
        }
        if(agent.destination != _currTarget.transform.position)
        {
            agent.SetDestination(_currTarget.position);
        }
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Task.current.Succeed();
            _currTarget = null;
        }
    }

    [Task]
    bool Idle()
    {
        text.text = "Current State: " + "Waiting for player...";
        return true;
    }

    [Task]
    bool isClose(string tag)
    {
        player = GameObject.FindGameObjectWithTag(tag);
        float distFromPlayer = (agent.transform.position - player.transform.position).x;
        return Vector3.Distance(player.transform.position, gameObject.transform.position) < distFromPlayer;
    }

    [Task]
    void DisplayText(string dispText)
    {
        text.text = dispText;
    }

    #endregion
}