using Entity.NPC;

namespace Core.StateMachine.StateList
{
    public class CustomerIdle : State
    {
        private NPC _customer;

        public CustomerIdle(NPC customer)
        {
            _customer = customer;
        }
        public override void Enter()
        {
            base.Enter();
            //Debug.Log("customer enter to idle");
            _customer.animator.SetBool("isRun",false);
        }

        public override void Exit()
        {
            base.Exit();
            //Debug.Log("customer exit idle");
        }

        public override void Update()
        {
            base.Update();
            //Debug.Log("customer update state idle");
        }
    }
}
