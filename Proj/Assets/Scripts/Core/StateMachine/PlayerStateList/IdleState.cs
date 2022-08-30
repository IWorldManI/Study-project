using UnityEngine;

namespace Core.StateMachine.StateList
{
    public class IdleState : State
    {
        private CharacterMoveAndRotate player;

        public IdleState(CharacterMoveAndRotate _player)
        {
            player = _player;
        }
        public override void Enter()
        {
            base.Enter();
            //Debug.Log("enter to idle");
            player.animator.SetBool("isRun",false);
        }

        public override void Exit()
        {
            base.Exit();
            //Debug.Log("exit idle");
        }

        public override void Update()
        {
            base.Update();
            //Debug.Log("update state idle");
        }
    }
}
