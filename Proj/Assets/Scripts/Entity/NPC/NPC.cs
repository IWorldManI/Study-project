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
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] public Animator animator;

        public int ordersCount;
        public float customerMultiplier;

        protected StateMachine _stateMachine;

        private ItemCollection _itemCollection;
        
        protected ItemGiver _startPosition;

        protected InventoryManager inventoryManager; //need reference
        protected CustomerOrdersManager _customerOrdersManager;
        
        [SerializeField] protected Vector3 target;
        
        internal Action<NPC> OnCollect;

        protected virtual void Start()
        {
            inventoryManager = GetComponentInChildren<InventoryManager>();
            _customerOrdersManager = GetComponentInChildren<CustomerOrdersManager>();
            
            //testing values field
            ordersCount = Random.Range(10, 200);
            customerMultiplier = Random.Range(1f, 2f);

            //Replace to zenject
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            _stateMachine = new StateMachine();
            _stateMachine.Initialize(new CustomerIdle(this));
            
        }

        protected void InitStands(ItemDistributor[] standObjects)
        {
            //iterator test
            _itemCollection = new ItemCollection();
            
            //iterator add items // for debug
            foreach (var stand in standObjects)
            {
                var component = stand.GetComponent<ItemDistributor>();
                _itemCollection.AddItem(component);
            }
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
        internal void TryOrderNext(NPC npc)
        {
            if (ordersCount > 0)
            {
                ordersCount -= 1;
                Debug.Log("Next order finding ");
                var item = _itemCollection.GetItems();
                var itemId = Random.Range(0, item.Count);
            
                npc.target = item[itemId].transform.position;
                Debug.Log("Looking for " + item[itemId].name + " " + name);
            
                npc.inventoryManager.LookingItem = _customerOrdersManager.GetOrder(item[itemId]);
                Debug.Log(_customerOrdersManager.GetOrder(item[itemId]));
                npc.StartCoroutine(NextState(this));
            }
            else
            {
                Debug.Log("orderCount = 0" + name);
            }
        }
        internal void TryHelperNext(NPC npc)
        {
            if (inventoryManager._ingredientList.Count <= 0)
            {
                npc.target = _startPosition.transform.position;
                //Debug.Log("Looking for " + _startPosition.name + name);
                
                npc.StartCoroutine(NextState(this));
            }
            else
            {
                var item = _itemCollection.GetItems();
                var itemType = inventoryManager._ingredientList[0].GetType();
                foreach (var find in item)
                {
                    if (find.GetType() == itemType)
                    {
                        npc.target = find.transform.position;
                    }
                    else
                    {
                        Debug.Log("Cant find");
                    }
                }
                npc.StartCoroutine(NextState(this));
                //TryOrderNext(npc);
                Debug.Log("Helper have items" + name);
            }
        }
        
        private IEnumerator NextState(NPC npc)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            
            npc._stateMachine.ChangeState(new CustomerRun(this));
            navMeshAgent.SetDestination(target);
        }
    }
}
