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

    private StateMachine _stateMachine;
    private RunState _runState;
    
    private InventoryManager _inventoryManager;
    
    private void Start()
    {
        //Replace to zenject
        animator = GetComponent<Animator>();
        joystick = FindObjectOfType<FloatingJoystick>();
        rb = GetComponent<Rigidbody>();

        _stateMachine = new StateMachine();
        _stateMachine.Initialize(new IdleState(this));

        _inventoryManager = GetComponent<InventoryManager>();
    }

    private void Update()
    {
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
