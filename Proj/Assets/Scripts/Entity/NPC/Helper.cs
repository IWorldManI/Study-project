using System.Collections;
using Core.StateMachine;
using Core.StateMachine.StateList;
using UnityEngine;
using UnityEngine.AI;

namespace Entity.NPC
{
    public class Helper : NPC
    {
        private void Start()
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
            
            ItemDistributor[] standObjects = FindObjectsOfType<Stand>();
            InitStands(standObjects);
            _startPosition = FindObjectOfType<ItemGiver>();

            StartCoroutine("Delay", 2f);
        }
        
        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            
            TryHelperNext(this);
        }
    }
}

