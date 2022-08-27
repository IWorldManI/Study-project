using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Entity.Customer
{
    public class Customer : MonoBehaviour
    {
        public int orders;
        public float customerMultiplier;
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private void Start()
        {
            //testing values field
            orders = Random.Range(1, 3);
            customerMultiplier = Random.Range(1f, 2f);

            //Replace to zenject
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            //test moving
            Vector3 newPos = RandomNavSphere(transform.position, 5f, -1);
            _navMeshAgent.SetDestination(newPos);
        }
        //random pos inside sphere
        public static Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) 
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance;
           
            randomDirection += origin;
           
            NavMeshHit navHit;
           
            NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
           
            return navHit.position;
        }
    }
}
