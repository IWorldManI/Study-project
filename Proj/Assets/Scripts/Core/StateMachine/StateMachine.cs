namespace Core.StateMachine
{
    public class StateMachine
    {
        public State CurrentState { get; set; }

        public void Initialize(State initState)
        {
            CurrentState = initState;
            CurrentState.Enter();
        }

        public void ChangeState(State newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
