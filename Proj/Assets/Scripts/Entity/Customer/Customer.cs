using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Iterator;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Core.StateMachine.StateList;
using Unity.VisualScripting;
using StateMachine = Core.StateMachine.StateMachine;

namespace Entity.Customer
{
    public class Customer : MonoBehaviour
    {
        [SerializeField] public Animator animator;
        private static readonly int IsRun = Animator.StringToHash("isRun");
        
        public int orders;
        public float customerMultiplier;

        [SerializeField] public NavMeshAgent _navMeshAgent;

        private StateMachine stateMachine;

        private ItemCollection _itemCollection;
        
        [SerializeField] private Vector3 target;
        
        
        
        //debug
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
               
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
            orders = Random.Range(0, 2);
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
            
            //iterator test
            _itemCollection = new ItemCollection();
            
            //iterator additems
            _itemCollection.AddItem(tomatoes);
            _itemCollection.AddItem(milk);
            _itemCollection.AddItem(cashBox);
        }
        
        //test moving
        private IEnumerator NextState()
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            
            var item = _itemCollection.GetItems();
            
            target = item[Random.Range(0, item.Count)].transform.position;

            stateMachine.ChangeState(new CustomerRun(this));
            _navMeshAgent.SetDestination(target); //here start moving?
            
            StartCoroutine(NextState());
            
        }
    }
}
