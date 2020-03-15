using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalResult : MonoBehaviour
{
    [SerializeField] private Text tapToPlay;

    private void Start()
    {
        GameManager.Instance.OnGameFailed += ShowTapToPlay;
        GameManager.Instance.OnGameResetFromBegining += OnResetGame;
        
    }

    private void ShowTapToPlay()
    {
        var Col = tapToPlay.GetComponent<Text>().color;
        Col.a = 1f;
        tapToPlay.GetComponent<Text>().color = Col ;
    }

    private void OnResetGame()
    {
        var Col = tapToPlay.GetComponent<Text>().color;
        Col.a = 0f;
        tapToPlay.GetComponent<Text>().color = Col;
    }
}
