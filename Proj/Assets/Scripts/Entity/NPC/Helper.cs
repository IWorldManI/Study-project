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
        }

        protected override void Start()
        {
            inventoryManager = GetComponentInChildren<InventoryManager>();
            
            //Replace to zenject
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            _stateMachine = new StateMachine();
            _stateMachine.Initialize(new CustomerIdle(this));
            
            standObjects = FindObjectsOfType<Stand>();
            InitStands(standObjects);
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

        public override void TryNextTarget(NPC npc)
        {
            Debug.Log("Looking for next target " + name);
            var standList = standObjects;
            
            if (inventoryManager._ingredientList.Count <= 0)
            {
                ReturnToStartPosition(this);
            }
            else
            {
                FindCurrentStand(this, standList);
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
    }
}

