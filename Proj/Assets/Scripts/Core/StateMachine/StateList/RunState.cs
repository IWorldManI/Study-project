using UnityEngine;

namespace Core.StateMachine.StateList
{
    public class RunState : State
    { 
        private CharacterMoveAndRotate player;

        public RunState(CharacterMoveAndRotate _player)
        {
            player = _player;
        }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("enter to  Run");
            player.animator.SetBool("isRun",true);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("exit Run");
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("update state Run");
        }
    }
}
