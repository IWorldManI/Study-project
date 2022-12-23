using System;
using System.Collections;
using Core.StateMachine;
using Core.StateMachine.StateList;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

namespace Entity.NPC
{
    public class Helper : NPC
    {
        protected override void Awake()
        {
            _eventBus = FindObjectOfType<EventBus>();
            
            standObjects = FindObjectsOfType<Stand>();
            InitStands(standObjects);
        }

        protected override void Start()
        {
            inventoryManager = GetComponentInChildren<InventoryManager>();
            
            //Replace to zenject
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            _stateMachine = new StateMachine();
            _stateMachine.Initialize(new CustomerIdle(this));

            trashCan = FindObjectOfType<TrashCan>();

            StartCoroutine(InitDelay());
        }
        
        private IEnumerator InitDelay()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
            
                TryNextTarget(this);
            }
        }
        
        public override void TryNextTarget(NPC npc)
        {
            Debug.Log("Looking for next target " + name);

            if (inventoryManager._ingredientList.Count <= 0)
            {
                ReturnToStartPosition(this);
            }
            else
            {
                FindCurrentStand(this, standObjects);
            }
        }

        private void ReturnToStartPosition(NPC npc)
        {
            npc.target = _startPosition.transform;

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
                    npc.target = find.transform;
                }
                else
                {
                    if (itemType == stand.StandType && stand.isFull)
                    {
                        npc.target = trashCan.transform;
                    }
                }
            }
            npc.StartCoroutine(NextState(this));
        }
    }
}

