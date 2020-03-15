using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    private Vector3 nextPosition;

    private void Reset()
    {
        startPosition = transform.position;
    }

    private void Start()
    {
        nextPosition = startPosition;
        GameManager.Instance.OnLevelUp += OnCameraStartMovement;        
        GameManager.Instance.OnGameFailed += CalcutateResultPositionCamera;
        GameManager.Instance.OnGameResetFromBegining += ResetCameraToStartPosition;
    }

    private void OnCameraStartMovement()
    {
        nextPosition += Vector3.up;
        var temp = GameManager.Instance.targerPos;
        nextPosition = new Vector3(temp.x + 10f, nextPosition.y, temp.z - 10f);
    }

    private void ResetCameraToStartPosition()
    {
        transform.position = startPosition;
        nextPosition = startPosition;
    }

    private void CalcutateResultPositionCamera()
    {
        nextPosition = new Vector3(30f, nextPosition.y + 15f , -30f);
    }



    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime);
    }
}
