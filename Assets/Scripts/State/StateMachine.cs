public class StateMachine
{
    private State currentState;
    public State CurrnetState
    {
        get
        {
            return currentState;
        }
        private set
        {
            currentState = value;
        }
    }
    public void Initialize(State startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(State newState)
    {
        currentState.Exit();
        CurrnetState = newState;
        currentState.Enter();
    }
}
