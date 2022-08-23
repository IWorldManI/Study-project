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
            Debug.Log("вошел в  Run");
            player.animator.SetBool("isRun",true);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("вышел из Run");
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("обновляю Run");
        }
    }
}
