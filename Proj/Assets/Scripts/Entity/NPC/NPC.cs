using System;
using System.Collections;
using Core.Iterator;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Core.StateMachine.StateList;
using Debug = UnityEngine.Debug;
using StateMachine = Core.StateMachine.StateMachine;

namespace Entity.NPC
{
    public class NPC : MonoBehaviour
    {
        [SerializeField] public Animator animator;

        public int ordersCount;
        public float customerMultiplier;

        [SerializeField] private NavMeshAgent navMeshAgent;

        private StateMachine _stateMachine;

        private ItemCollection _itemCollection;

        private InventoryManager inventoryManager; //need reference
        private CustomerOrdersManager _customerOrdersManager;
        
        [SerializeField] private Vector3 target;
        
        public Action OnCollect;
        
        private void Start()
        {
            //testing values field
            ordersCount = Random.Range(10, 200);
            customerMultiplier = Random.Range(1f, 2f);

            //Replace to zenject
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            _stateMachine = new StateMachine();
            _stateMachine.Initialize(new CustomerIdle(this));
            
            //test moving
            OrderNext();
            
            //test list moving
            ItemDistributor[] standObjects = FindObjectsOfType<StandPointForCustomers>(); // for debug
            
            //iterator test
            _itemCollection = new ItemCollection();
            
            //iterator additems // for debug
            foreach (var stand in standObjects)
            {
                var component = stand.GetComponent<ItemDistributor>();
                _itemCollection.AddItem(component);
            }
            inventoryManager = GetComponentInChildren<InventoryManager>();
            _customerOrdersManager = GetComponentInChildren<CustomerOrdersManager>();
        }
        
        //debug
        private void Update()
        {
            if (navMeshAgent.remainingDistance<=navMeshAgent.stoppingDistance)
            {
                _stateMachine.ChangeState(new CustomerIdle(this));
            }
        }
        
        //test moving
        public void OrderNext()
        {
            if (ordersCount > 0)
            {
                ordersCount -= 1;
                Debug.Log("Next order find");
                StartCoroutine(NextState());
            }
        }
        private IEnumerator NextState()
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            
            var item = _itemCollection.GetItems();
            var itemId = Random.Range(0, item.Count);
            
            target = item[itemId].transform.position;
            Debug.Log("Looking for " + item[itemId].name + name);
            
            inventoryManager.LookingItem = _customerOrdersManager.GetOrder(item[itemId]);

            _stateMachine.ChangeState(new CustomerRun(this));
            navMeshAgent.SetDestination(target);
        }
    }
}
