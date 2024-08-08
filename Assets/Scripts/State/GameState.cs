using UnityEngine;

public class GameState : State
{
    public GameState(GameManager gameManager, StateMachine stateMachine) : base(gameManager, stateMachine)
    {

    }

    public override void Enter()
    {
        gameManager.OnGameStart?.Invoke();
        gameManager.InstantiateFigure();
    }

    public override void Exit()
    {

    }

    public override void InputUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            gameManager.CheckMatches();
        }
    }
}
