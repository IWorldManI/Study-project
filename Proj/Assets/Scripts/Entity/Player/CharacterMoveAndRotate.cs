using System;
using Core.StateMachine;
using Core.StateMachine.StateList;
using UnityEngine;

public class CharacterMoveAndRotate : EntityBase
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 10.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    
    [SerializeField] private Joystick joystick;
    
    private StateMachine _stateMachine;
    private RunState _runState;
    
    [SerializeField] public Animator animator;
    private static readonly int IsRun = Animator.StringToHash("isRun");

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if(controller==null)
            controller = gameObject.AddComponent<CharacterController>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        joystick = FindObjectOfType<FloatingJoystick>();
        _stateMachine = new StateMachine();
        _stateMachine.Initialize(new IdleState(this));
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        controller.Move(move * (Time.deltaTime * playerSpeed));

        if (move != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,10f);
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
        //State machine
        _stateMachine.CurrentState.Update();
        if (Input.GetMouseButtonDown(0))
        {
            _stateMachine.ChangeState(new RunState(this));
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _stateMachine.ChangeState(new IdleState(this));
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * 10f;
    }
}
