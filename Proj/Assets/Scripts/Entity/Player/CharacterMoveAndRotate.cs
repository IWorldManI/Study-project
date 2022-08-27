using System;
using System.Collections;
using System.Collections.Generic;
using Core.Factory;
using Core.StateMachine;
using Core.StateMachine.StateList;
using UnityEngine;

public class CharacterMoveAndRotate : MonoBehaviour
{
    [SerializeField] public Animator animator;
    private static readonly int IsRun = Animator.StringToHash("isRun");
    
    [SerializeField] private Joystick joystick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed = 360;

    private StateMachine stateMachine;
    private RunState runState;
    
    private void Start()
    {
        //Replace to zenject
        animator = GetComponent<Animator>();
        joystick = FindObjectOfType<FloatingJoystick>();
        rb = GetComponent<Rigidbody>();

        stateMachine = new StateMachine();
        stateMachine.Initialize(new IdleState(this));
        
    }

    private void Update()
    {
        stateMachine.CurrentState.Update();
        if (Input.GetMouseButtonDown(0))
        {
            stateMachine.ChangeState(new RunState(this));
        }
        else if(Input.GetMouseButtonUp(0))
        {
            stateMachine.ChangeState(new IdleState(this));
        }
    }

    private void FixedUpdate()
    {
        float xMovement = joystick.Horizontal;
        float zMovement = joystick.Vertical;
        
        Vector3 movementDirection = new Vector3(xMovement, 0, zMovement);
        movementDirection.Normalize();
        
        rb.MovePosition(transform.position + movementDirection * (speed * Time.deltaTime));

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);          
        }
    }
}
