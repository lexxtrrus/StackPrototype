using UnityEngine;

public class CalculateTransoftms
{
    private Transform bottomFigure;
    private Transform upFigure;

    private Transform fallingFigure;

    //placement figure
    private float bottomEdgePosition = 0f;
    private float upEdgePosition = 0f;

    private float zCovered = 0;
    private float xCovered = 0;

    public Transform CreateTempTransform()
    {
        GameObject temp = new GameObject("temp");
        fallingFigure = temp.GetComponent<Transform>();
        return fallingFigure;
    }

    public void Init(Transform bottom, Transform placedFigure)
    {        
        bottomFigure = bottom;
        upFigure = placedFigure;
    }

    public bool IsPlayerMissed()
    {
        var zScaleBottom = bottomFigure.localScale.z * 0.5f;
        var xScaleBottom = bottomFigure.localScale.x * 0.5f;

        var zScaleUp = upFigure.localScale.z * 0.5f;
        var xScaleUp = upFigure.localScale.x * 0.5f;

        var possibleZCover = zScaleBottom + zScaleUp - 0.1f;
        var possibleXCover = xScaleBottom + xScaleUp - 0.1f;

        zCovered = Mathf.Abs(bottomFigure.position.z - upFigure.position.z);
        xCovered = Mathf.Abs(bottomFigure.position.x - upFigure.position.x);

        var isZAxisCover = zCovered < possibleZCover;
        var isXAxisCover = xCovered < possibleXCover;

        return !isZAxisCover || !isXAxisCover;
    }

    public bool IsPerfectPlacement()
    {
        return Mathf.Abs(zCovered) < 0.1f && Mathf.Abs(xCovered) < 0.1f;
    }

    public Transform CalculateZAxisPlacementPosition()
    {
        var multiplicationFactor = 0f;

        if (bottomFigure.position.z > upFigure.position.z)
        {
            multiplicationFactor = 1f;
        }
        else
        {
            multiplicationFactor = -1f;
        }

        var position = upFigure.position;
        var localScale = upFigure.localScale;
        
        upEdgePosition = position.z + localScale.z * 0.5f * multiplicationFactor; 
        bottomEdgePosition = bottomFigure.position.z - bottomFigure.localScale.z * 0.5f * multiplicationFactor; 

        zCovered = Mathf.Abs(upEdgePosition - bottomEdgePosition);
        
        var pos = position;
        var scale = localScale;

        var scaleFallingFigure = scale.z;

        pos.z = upEdgePosition - multiplicationFactor * zCovered * 0.5f;
        position = pos;
        upFigure.position = position;

        scale.z = zCovered;
        localScale = scale;
        upFigure.localScale = localScale;

        pos.z = bottomEdgePosition - multiplicationFactor * Mathf.Abs(scaleFallingFigure - zCovered) * 0.5f;
        fallingFigure.position = pos;

        scale.z = Mathf.Abs(scaleFallingFigure - zCovered);
        fallingFigure.localScale = scale;
        
        return upFigure;
    }

    public Transform CalculateZAxisFallingPosition()
    {
        return fallingFigure;
    }

    public Transform CalculateXAxisPlacementPositions()
    {
        var multiplicationFactor = 0f;

        if (bottomFigure.position.x > upFigure.position.x)
        {
            multiplicationFactor = 1f;
        }
        else
        {
            multiplicationFactor = -1f;
        }

        upEdgePosition = upFigure.position.x + upFigure.localScale.x * 0.5f * multiplicationFactor;
        bottomEdgePosition = bottomFigure.position.x - bottomFigure.localScale.x * 0.5f * multiplicationFactor;

        xCovered = Mathf.Abs(upEdgePosition - bottomEdgePosition);

        var pos = upFigure.position;
        var scale = upFigure.localScale;

        var scaleFallingFigure = scale.x;

        pos.x = upEdgePosition - multiplicationFactor * xCovered * 0.5f;
        upFigure.position = pos;

        scale.x = xCovered;
        upFigure.localScale = scale;

        pos.x = bottomEdgePosition - multiplicationFactor * Mathf.Abs(scaleFallingFigure - xCovered) * 0.5f;
        fallingFigure.position = pos;

        scale.x = Mathf.Abs(scaleFallingFigure - xCovered);
        fallingFigure.localScale = scale;

        return upFigure;
    }

    public Transform CalculateXAxisFallingPosition()
    {
        return fallingFigure;
    }
}

