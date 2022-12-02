using System.Collections.Generic;
using Core.StateMachine.StateList;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using StateMachine = Core.StateMachine.StateMachine;

namespace Entity.NPC
{
    public class Customer : NPC, IObserver
    {
        private int ordersCount;
        private float customerMultiplier;
        public Transform _transform { get; private set; }
        private List<ItemDistributor> _stands = new List<ItemDistributor>();

        private void Awake()
        {
            _eventBus = FindObjectOfType<EventBus>();
        }
        
        private void Start()
        {
            _customerOrdersManager = new CustomerOrdersManager();
            _stands = new List<ItemDistributor>();
            inventoryManager = GetComponentInChildren<InventoryManager>();

            //testing values field
            ordersCount = inventoryManager.MaxCapacity;
            customerMultiplier = Random.Range(1f, 2f);
            
            _cashierStand = FindObjectOfType<CashierStand>();

            //Replace to zenject
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            _stateMachine = new StateMachine();
            _stateMachine.Initialize(new CustomerIdle(this));
            
            ItemDistributor[] standObjects = FindObjectsOfType<Stand>(); 
            InitStands(standObjects);
            
            TryNextTarget(this);
            _transform = transform;
        }

        private void TryNextTarget(NPC npc)
        {
            if (inventoryManager._ingredientList.Count < inventoryManager.MaxCapacity)
            {
                FindStand(npc);
            }
            else
            {
                Debug.Log("orderCount = 0" + name);
                MoveToCashier(npc);
            }
        }

        private void FindStand(NPC npc)
        {
            ordersCount -= 1;
            //Debug.Log("Next order finding ");
            _stands = _itemCollection.GetItems();
            var itemId = Random.Range(0, _stands.Count);
            
            npc.target = _stands[itemId].transform.position;
            //Debug.Log("Looking for " + _stands[itemId].name + " " + name);
            
            npc.inventoryManager.LookingItem = _customerOrdersManager.GetOrder(_stands[itemId]);
            //Debug.Log(_customerOrdersManager.GetOrder(_stands[itemId]));
            npc.StartCoroutine(NextState(this));
        }
        
        private void MoveToCashier(NPC npc)
        {
            npc.target = _cashierStand.transform.position;
            npc.StartCoroutine(NextState(this));
        }

        private void FindSlotInQueue(NPC npc)
        {
            npc.target = _cashierStand.GetLastCustomerPosition();
            _cashierStand.Attach(this);
            //if customers in queue need notify all if slot any is empty
            
            //need position in queue and set it here 
            npc.StartCoroutine(NextState(this));
        }
        private void OnEnable()
        {  
            _eventBus.OnCollect += TryNextTarget;
        }

        private void OnDisable()
        {
            _eventBus.OnCollect -= TryNextTarget;
        }

        public void UpdateObserver(ISubject subject)
        {
            //Debug.Log("Customer was reacted to the event");
        }
    }
  
}

