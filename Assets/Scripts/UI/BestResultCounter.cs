using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestResultCounter : MonoBehaviour
{
    [SerializeField] private Text bestResult;
    private int currentResultNumber = 0;
    private bool IsFirstFigure = true;

    private void Start()
    {
        Profile.BestResult = currentResultNumber;
        GameManager.Instance.OnFigurePlaced += LevelUp;
        GameManager.Instance.OnGameResetFromBegining += ResetCounter;
    }

    private void ResetCounter()
    {
        currentResultNumber = 0;
        IsFirstFigure = true;
    }

    

    private void LevelUp()
    {
        Profile.BestResult = currentResultNumber + 1;
        bestResult.text = $"BEST: {Profile.BestResult.ToString()}";
        currentResultNumber++;

        if (IsFirstFigure)
        {
            bestResult.GetComponent<Text>().enabled = true;
            IsFirstFigure = false;
        }
    }
}
