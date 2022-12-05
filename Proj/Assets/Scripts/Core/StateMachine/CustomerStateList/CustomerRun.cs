using Entity.NPC;

namespace Core.StateMachine.StateList
{
    public class CustomerRun : State
    {
        private NPC _customer;

        public CustomerRun(NPC customer)
        {
            _customer = customer;
        }
        public override void Enter()
        {
            base.Enter();
            //Debug.Log("customer enter to run");
            _customer.animator.SetBool("isRun",true);
        }

        public override void Exit()
        {
            base.Exit();
            //Debug.Log("customer exit run");
        }

        public override void Update()
        {
            base.Update();
            //Debug.Log("customer update state run");
        }
    }
}