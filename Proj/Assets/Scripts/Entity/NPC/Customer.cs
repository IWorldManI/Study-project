using System;
using System.Collections;
using System.Collections.Generic;
using Core.StateMachine.StateList;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using StateMachine = Core.StateMachine.StateMachine;

namespace Entity.NPC
{
    public class Customer : NPC, IObserver, IEnumTypeToVisual
    {
        private int ordersCount;
        private float customerMultiplier;
        private Vector3 oldTarget;
        private float _delayForCheckTargetPosition = 1f;
        public bool WaitForPay { get; set; }
        public UI2World UI2World;

        private List<ItemDistributor> _stands = new List<ItemDistributor>();

        protected override void Awake()
        {
            _eventBus = FindObjectOfType<EventBus>();
            _customerOrdersManager = new CustomerOrdersManager();
            
            standObjects = FindObjectsOfType<Stand>();
            inventoryManager = GetComponentInChildren<InventoryManager>();
            InitStands(standObjects);
        }

        protected override void Start()
        {
            //_stands = new List<ItemDistributor>();

            //testing values field
            ordersCount = inventoryManager.MaxCapacity;
            customerMultiplier = Random.Range(1f, 2f);
            
            _cashierStand = FindObjectOfType<CashierStand>();

            //Replace to zenject
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            _stateMachine = new StateMachine();
            _stateMachine.Initialize(new CustomerIdle(this));

            _startPosition = FindObjectOfType<ExitPoint>().transform;
            
            StartCoroutine(InitDelay());
        }

        private IEnumerator InitDelay()
        {
            yield return new WaitForSeconds(2f);
            
            TryNextTarget(this);
            StartCoroutine(Alive());
        }
        private IEnumerator Alive()
        {
            while (true)
            {
                yield return new WaitForSeconds(_delayForCheckTargetPosition);

                if (oldTarget == target.transform.position)
                {
                    Debug.Log("Target position not changed");
                }
                else
                {
                    Debug.Log("Target position is changed, new way created"); 
                    
                    var position = target.transform.position;
                    navMeshAgent.SetDestination(position);
                    oldTarget = position;
                }
            }
        }
        
        public override void TryNextTarget(NPC npc)
        {
            Debug.Log("Looking for next order " + name);
            
            if (inventoryManager._ingredientList.Count < inventoryManager.MaxCapacity)
            {
                FindStand(this);
            }
            else
            {
                Debug.Log("orderCount = 0" + name);
                FindSlotInQueue(this);
            }
        }
        
        private void FindStand(NPC npc)
        {
            ordersCount -= 1;
            //Debug.Log("Next order finding ");
            _stands = _itemCollection.GetItems();
            var itemId = Random.Range(0, _stands.Count);
            
            npc.target = _stands[itemId].transform;
            Debug.Log("Looking for " + _stands[itemId].name + " " + name);
            
            npc.inventoryManager.LookingItem = _customerOrdersManager.GetOrder(_stands[itemId]);
            var image = _customerOrdersManager.GetImage(_stands[itemId]);
            UI2World.UpdateOrderImage(image);
          
            Debug.Log(npc.inventoryManager.LookingItem + " + " + name);
            
            npc.StartCoroutine(NextState(this));
        }
        
        public void MovingToExit()
        {
            target = _startPosition.transform;

            StartCoroutine(NextState(this));
        }

        private void FindSlotInQueue(NPC npc)
        {
            npc.target = _cashierStand.GetLastCustomer() == null ? _cashierStand.transform : _cashierStand.GetLastCustomer().transform;
            _cashierStand.Attach(this);
            //if customers in queue need notify all if slot any is empty
            
            //need position in queue and set it here 
            npc.StartCoroutine(NextState(this));
        }

        public void UpdateObserver(ISubject subject)
        {
            this.target = _cashierStand.transform;
            //Debug.Log("Customer was reacted to the event" + name);
        }

        private void OnEnable()
        {
            ordersCount = inventoryManager.MaxCapacity;
            WaitForPay = false;
            StartCoroutine(InitDelay());
        }
    }
}

