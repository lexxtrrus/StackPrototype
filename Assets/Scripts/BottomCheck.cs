using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.RemoveThisFallingFigure(gameObject);
        Destroy(this.gameObject);
    }
}
