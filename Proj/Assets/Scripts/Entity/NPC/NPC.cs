using System.Collections;
using Core.Iterator;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Core.StateMachine.StateList;
using StateMachine = Core.StateMachine.StateMachine;

namespace Entity.NPC
{
    public class NPC : MonoBehaviour
    {
        [SerializeField] public Animator animator;

        public int orders;
        public float customerMultiplier;

        [SerializeField] private NavMeshAgent _navMeshAgent;

        private StateMachine _stateMachine;

        private ItemCollection _itemCollection;

        private InventoryManager _inventoryManager;
        
        [SerializeField] private Vector3 target;
        
        private void Start()
        {
            //testing values field
            orders = Random.Range(0, 2);
            customerMultiplier = Random.Range(1f, 2f);

            //Replace to zenject
            _navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            //_navMeshAgent.stoppingDistance = Random.Range(1f, 2f); //for test

            _stateMachine = new StateMachine();
            _stateMachine.Initialize(new CustomerIdle(this));
            
            //test moving
            StartCoroutine(NextState());
            
            //test list moving
            ItemDistributor[] standObjects = FindObjectsOfType<ItemDistributor>();
            
            //iterator test
            _itemCollection = new ItemCollection();
            
            //iterator additems
            foreach (var stand in standObjects)
            {
                _itemCollection.AddItem(stand);
            }
            _inventoryManager = GetComponentInChildren<InventoryManager>();
        }
           
        //debug
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
               
            }
            //mb need fix 
            if (_navMeshAgent.remainingDistance<=_navMeshAgent.stoppingDistance)
            {
                _stateMachine.ChangeState(new CustomerIdle(this));
            }
        }
        //test moving
        private IEnumerator NextState()
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            
            var item = _itemCollection.GetItems();
            
            target = item[Random.Range(0, item.Count)].transform.position;

            _stateMachine.ChangeState(new CustomerRun(this));
            _navMeshAgent.SetDestination(target); //here start moving?
            
            StartCoroutine(NextState());
            
        }
    }
}
