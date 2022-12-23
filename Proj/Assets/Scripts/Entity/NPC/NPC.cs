using System.Collections;
using Core.Iterator;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Core.StateMachine.StateList;
using StateMachine = Core.StateMachine.StateMachine;

namespace Entity.NPC
{
    public class NPC : EntityBase
    {
        [SerializeField] internal NavMeshAgent navMeshAgent;
        [SerializeField] public Animator animator;
        
        protected StateMachine _stateMachine;

        protected ItemCollection _itemCollection;

        protected ItemDistributor[] standObjects;
        
        [SerializeField] protected Transform _startPosition;
        protected TrashCan trashCan;
        protected CashierStand _cashierStand;

        internal InventoryManager inventoryManager; //need reference
        protected CustomerOrdersManager _customerOrdersManager;
        
        [SerializeField] internal Transform target;
        
        protected EventBus _eventBus;

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            
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
            else
            {
                _stateMachine.ChangeState(new CustomerRun(this));
            }
        }
        //test moving

       
        protected IEnumerator NextState(NPC npc)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            
            npc._stateMachine.ChangeState(new CustomerRun(this));
            navMeshAgent.SetDestination(target.transform.position);
        }

        public virtual void TryNextTarget(NPC npc)
        {
            
        }
    }
}
