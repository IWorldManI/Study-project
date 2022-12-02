using System;
using System.Collections;
using Core.StateMachine;
using Core.StateMachine.StateList;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Entity.NPC
{
    public class Helper : NPC
    {
        private void Awake()
        {
            _eventBus = FindObjectOfType<EventBus>();
        }

        private void Start()
        {
            _customerOrdersManager = new CustomerOrdersManager();
            inventoryManager = GetComponentInChildren<InventoryManager>();
            
            //Replace to zenject
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            _stateMachine = new StateMachine();
            _stateMachine.Initialize(new CustomerIdle(this));
            
            standObjects = FindObjectsOfType<Stand>();
            InitStands(standObjects);
            //_startPosition = FindObjectOfType<ItemGiver>();
            trashCan = FindObjectOfType<TrashCan>();

            StartCoroutine("Delay", 2f);
        }
        
        private IEnumerator Delay()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
            
                TryNextTarget(this);
            }
        }

        private void TryNextTarget(NPC npc)
        {
            var standList = standObjects;
            
            if (inventoryManager._ingredientList.Count <= 0)
            {
                ReturnToStartPosition(npc);
            }
            else
            {
                FindCurrentStand(npc, standList);
            }
        }

        private void ReturnToStartPosition(NPC npc)
        {
            npc.target = _startPosition.transform.position;
            //Debug.Log("Looking for " + _startPosition.name + name);
                
            npc.StartCoroutine(NextState(this));
        }

        private void FindCurrentStand(NPC npc, ItemDistributor[] standArray)
        {
            var itemType = inventoryManager._ingredientList[0].GetType();
            foreach (var find in standArray)
            {
                var stand = find.GetComponent<Stand>();
                if (itemType == stand.StandType && !stand.isFull)
                {
                    npc.target = find.transform.position;
                    //Debug.Log("Moving to " + find.name);
                }
                else
                {
                    if (itemType == stand.StandType && stand.isFull)
                    {
                        npc.target = trashCan.transform.position;
                    }
                    ///Debug.Log("Cant find");
                }
            }
            npc.StartCoroutine(NextState(this));
                
            //Debug.Log("Helper have items" + name);
        }
        private void OnEnable()
        {
            //_eventBus.OnCollect += TryNextTarget;
        }

        private void OnDisable()
        {
            //_eventBus.OnCollect -= TryNextTarget;
        }
    }
}

