using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    Vector3 _moveDir = Vector3.zero;
    CharacterController CC;

    void Start()
    {
        CC = GetComponent<CharacterController>();
    }

    void Update()
    {
        _moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        _moveDir *= speed;
        CC.Move(_moveDir * Time.deltaTime);
    }
}