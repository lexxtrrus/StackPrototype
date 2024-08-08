public abstract class State
{
    public State(GameManager gameManager, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameManager = gameManager;
    }

    protected GameManager gameManager;
    protected StateMachine stateMachine;

    public abstract void Enter();
    public abstract void Exit();
    public abstract void InputUpdate();
}
