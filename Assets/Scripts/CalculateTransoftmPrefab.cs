using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CalculateTransoftmPrefab : MonoBehaviour
{
    private Transform bottomFigure;
    private Transform upFigure;

    private Transform fallingFigure;

    //placement figure
    private float scaleZ = 0f;
    private float scaleX = 0f;
    private float newZ = 0f;
    private float newX = 0f;

    //falling figure
    private float fallScaleZ = 0f;
    private float fallScaleX = 0f;
    private float newFallZ = 0f;
    private float newFallX = 0f;

    private float zCovered = 0;
    private float xCovered = 0;

    private void Awake()
    {
        GameObject temp = new GameObject();
        fallingFigure = temp.GetComponent<Transform>();
        Debug.Log(fallingFigure.gameObject.name);
    }

    public void Init(Transform bottom, Transform up)
    {
        bottomFigure = bottom;
        upFigure = up;
    }

    public bool IsPlayerMissed()
    {
        var zScaleBottom = bottomFigure.localScale.z * 0.5f;
        var xScaleBottom = bottomFigure.localScale.x * 0.5f;

        var zScaleUp = upFigure.localScale.z * 0.5f;
        var xScaleUp = upFigure.localScale.x * 0.5f;

        float possibleZCover = zScaleBottom + zScaleUp - 0.1f;
        float possibleXCover = xScaleBottom + xScaleUp - 0.1f;

        zCovered = Mathf.Abs(bottomFigure.position.z - upFigure.position.z);
        xCovered = Mathf.Abs(bottomFigure.position.x - upFigure.position.x);

        bool isZAxisCover = zCovered < possibleZCover;
        bool isXAxisCover = xCovered < possibleXCover;

        if(!isZAxisCover || !isXAxisCover)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsPerfectPlacement()
    {
        if(Mathf.Abs(zCovered) < 0.1f && Mathf.Abs(xCovered) < 0.1f)
        {
            return true;
        }
        return false;
    }

    public Transform CalculateZAxisPlacementPosition()
    {
        float positiveFactor = 0f;

        if (bottomFigure.position.z > upFigure.position.z)
        {
            positiveFactor = 1f;
        }
        else
        {
            positiveFactor = - 1f;
        }
            
        float northC = upFigure.position.z + upFigure.localScale.z;            
        float southL = bottomFigure.position.z - bottomFigure.localScale.z;

            
        scaleZ = northC - southL;            
        newZ = southL + scaleZ * 0.5f;            
        scaleX = upFigure.localScale.x;            
        newX = upFigure.position.x;

        fallScaleZ = upFigure.localScale.z - scaleZ;
        newFallZ = newZ - scaleZ * 0.5f - fallScaleZ * positiveFactor * 0.5f;
        fallScaleX = upFigure.localScale.x;
        newFallX = upFigure.position.x;

        upFigure.transform.position = new Vector3(newX, upFigure.position.y, newZ);
        upFigure.transform.localScale = new Vector3(scaleX, 1f, scaleZ);

        fallingFigure.position = new Vector3(newFallX, upFigure.position.y, newFallZ);
        fallingFigure.localScale = new Vector3(fallScaleX, 1f, fallScaleZ);

        return upFigure;
    }

    public Transform CalculateZAxisFallingPosition()
    {
        return fallingFigure;
    }

    public Transform CalculateXAxisPlacementPositions()
    {
        float positiveFactor = 0f;

        if (bottomFigure.position.x > upFigure.position.x)
        {
            positiveFactor = 1f;
        }
        else
        {
            positiveFactor = -1f;
        }

            
        float eastC = upFigure.position.x + upFigure.localScale.x;            
        float westL = bottomFigure.position.x - bottomFigure.localScale.x;
        
        scaleX = eastC - westL;            
        newX = westL + scaleX * 0.5f;
        scaleZ = upFigure.localScale.z;            
        newZ = upFigure.position.z;
        
        fallScaleZ = upFigure.localScale.z;            
        newFallZ = upFigure.position.z;            
        fallScaleX = upFigure.localScale.x - scaleX;            
        newFallX = newX - scaleX * 0.5f - fallScaleX * positiveFactor * 0.5f;

        upFigure.transform.position = new Vector3(newX, upFigure.position.y, newZ);
        upFigure.transform.localScale = new Vector3(scaleX, 1f, scaleZ);

        fallingFigure.position = new Vector3(newFallX, upFigure.position.y, newFallZ);
        fallingFigure.localScale = new Vector3(fallScaleX, 1f, fallScaleZ);

        return upFigure;
    }

    public Transform CalculateXAxisFallingPosition()
    {
        return fallingFigure;
    }
}

