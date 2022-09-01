using Entity.Customer;
using UnityEngine;

namespace Core.StateMachine.StateList
{
    public class CustomerRun : State
    {
        private Customer customer;

        public CustomerRun(Customer _customer)
        {
            customer = _customer;
        }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("customer enter to run");
            customer.animator.SetBool("isRun",true);
            //customer._navMeshAgent.SetDestination(customer.target);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("customer exit run");
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("customer update state run");
        }
    }
}