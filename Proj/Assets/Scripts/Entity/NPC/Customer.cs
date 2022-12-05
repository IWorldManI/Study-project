using System;
using System.Collections;
using System.Collections.Generic;
using Core.StateMachine.StateList;
using UnityEngine;
using UnityEngine.AI;
using Zenject.Internal;
using Random = UnityEngine.Random;
using StateMachine = Core.StateMachine.StateMachine;

namespace Entity.NPC
{
    public class Customer : NPC, IObserver
    {
        private int ordersCount;
        private float customerMultiplier;
        
        private List<ItemDistributor> _stands = new List<ItemDistributor>();

        protected override void Awake()
        {
            _eventBus = FindObjectOfType<EventBus>();
            _customerOrdersManager = new CustomerOrdersManager();
            
            standObjects = FindObjectsOfType<Stand>();
            InitStands(standObjects);
        }

        protected override void Start()
        {
            //_stands = new List<ItemDistributor>();
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
                yield return new WaitForSeconds(.5f);
            
                navMeshAgent.SetDestination(target.transform.position);
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
            Debug.Log(npc.inventoryManager.LookingItem + " + " + name);
            //Debug.Log(_customerOrdersManager.GetOrder(_stands[itemId]));
            npc.StartCoroutine(NextState(this));
        }
        
        private void MoveToCashier(NPC npc)
        {
            npc.target = _cashierStand.transform;
            npc.StartCoroutine(NextState(this));
        }

        public void ReturnToStartPosition()
        {
            target = _startPosition.transform;
            //Debug.Log("Looking for " + _startPosition.name + name);
                
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
            Debug.Log("Customer was reacted to the event" + name);
        }
    }
}

