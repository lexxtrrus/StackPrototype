using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckReset : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnGameResetFromBegining += CheckResetFinal;
    }

    private void CheckResetFinal()
    {
        Destroy(this.gameObject);
    }
}
