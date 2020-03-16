using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentResultCounter : MonoBehaviour
{
    [SerializeField] private Text currentResult;
    private int currentResultNumber = 0;
    private bool IsFirstFigure = true;

    private void Start()
    {
        GameManager.Instance.OnFigurePlaced += LevelUp;
        GameManager.Instance.OnGameResetFromBegining += OnResetGameFromBegining;
    }

    private void LevelUp()
    {
        currentResultNumber++;
        currentResult.text = $"{currentResultNumber.ToString()}";

        if(IsFirstFigure)
        {
            currentResult.GetComponent<Text>().enabled = true;
            IsFirstFigure = false;
        }
    }

    private void OnResetGameFromBegining()
    {
        IsFirstFigure = true;
        currentResultNumber = 0;
        currentResult.text = $"{currentResultNumber.ToString()}";
        currentResult.GetComponent<Text>().enabled = false;
    }
}
