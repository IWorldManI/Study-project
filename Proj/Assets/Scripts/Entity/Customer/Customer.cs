using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Core.StateMachine;
using Core.StateMachine.StateList;

namespace Entity.Customer
{
    public class Customer : MonoBehaviour
    {
        [SerializeField] public Animator animator;
        private static readonly int IsRun = Animator.StringToHash("isRun");
        
        public int orders;
        public float customerMultiplier;

        public  List<GameObject> orderList = new List<GameObject>();
        private int n;

        [SerializeField] public NavMeshAgent _navMeshAgent;

        private StateMachine stateMachine;

        public Vector3 target;
        
        //debug
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                n++;
            }
            //mb need fix 
            if (_navMeshAgent.remainingDistance==0)
            {
                stateMachine.ChangeState(new CustomerIdle(this));
            }
        }
        
        private void Start()
        {
            //testing values field
            orders = Random.Range(1, 3);
            customerMultiplier = Random.Range(1f, 2f);

            //Replace to zenject
            _navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            stateMachine = new StateMachine();
            stateMachine.Initialize(new CustomerIdle(this));
            
            //test moving
            StartCoroutine(NextState());

            //test list moving
            var tomatoes = FindObjectOfType<Tomatoes>().gameObject;
            var milk = FindObjectOfType<Milk>().gameObject;
            var cashBox = FindObjectOfType<CashBox>().gameObject;
            orderList.Add(tomatoes);
            orderList.Add(milk);
            orderList.Add(cashBox);

            n = Random.Range(0, 3);
        }
        
        //test moving
        IEnumerator NextState()
        {
            yield return new WaitForSeconds(Random.Range(1f,5f));
            
            switch (n)
            {
                case 0:
                {
                    target = orderList[0].transform.position;
                    break;
                }
                case 1:
                {
                    target = orderList[1].transform.position;
                    break;
                }
                case 2:
                {
                    target = orderList[2].transform.position;
                    break;
                }
                default:
                {
                    target = orderList[2].transform.position;
                    break;
                }
            }
            stateMachine.ChangeState(new CustomerRun(this));
            
            StartCoroutine(NextState());
            
            StartCoroutine(Nplusplus());
            StartCoroutine(Nminusminus());
        }

        IEnumerator Nplusplus()
        {
            yield return new WaitForSeconds(Random.Range(10f, 15f));
            n++;
        }IEnumerator Nminusminus()
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));
            n = 0;
        }
        
    }
}
