using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatState : State
{
    float timer;

    public DefeatState(GameManager gameManager, StateMachine stateMachine) : base(gameManager, stateMachine)
    {

    }

    public override void Enter()
    {
        timer = Time.time + 1f;
    }

    public override void Exit()
    {

    }

    public override void InputUpdate()
    {
        if (Time.time < timer) return;
        if(Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.OnGameResetFromBegining?.Invoke();
        }
    }
}
