using Entity.Customer;
using UnityEngine;

namespace Core.StateMachine.StateList
{
    public class CustomerIdle : State
    {
        private Customer customer;

        public CustomerIdle(Customer _customer)
        {
            customer = _customer;
        }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("customer enter to idle");
            customer.animator.SetBool("isRun",false);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("customer exit idle");
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("customer update state idle");
        }
    }
}
