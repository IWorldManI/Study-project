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
            Debug.Log("вошел в  idle");
            player.animator.SetBool("isRun",false);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("вышел из idle");
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("обновляю idle");
        }
    }
}
