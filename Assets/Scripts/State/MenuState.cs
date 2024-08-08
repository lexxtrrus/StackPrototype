using UnityEngine;

public class MenuState : State
{
    public MenuState(GameManager gameManager, StateMachine stateMachine) : base(gameManager, stateMachine)
    {

    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void InputUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            stateMachine.ChangeState(gameManager.GameState);
        }
    }
}
