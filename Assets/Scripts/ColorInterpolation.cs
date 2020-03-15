using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorInterpolation : SingletonGameobject<ColorInterpolation>
{
    [SerializeField] private Gradient gradient = new Gradient();
    [SerializeField] private Color finalColor = new Color();
    [SerializeField] private bool IsFirstPalitre = true;

    private GradientColorKey[] colorKeys;
    private GradientAlphaKey[] alphaKeys;   

    private void Awake()
    {
        Instance = this;
        colorKeys = new GradientColorKey[2];
        alphaKeys = new GradientAlphaKey[2];
    }

    public Gradient GetPalitre()
    {
        GenerateGradient();
        return gradient;
    }

    private void GenerateGradient()
    {
        if(IsFirstPalitre)
        {
            GenerateFirstColor();
            GenerateLastColor();
            gradient.SetKeys(colorKeys, alphaKeys);
            IsFirstPalitre = false;
            return;
        }

        gradient = null;
        gradient = new Gradient();
        SetFirstColor();
        GenerateLastColor();
        gradient.SetKeys(colorKeys, alphaKeys);
    }

    private void GenerateFirstColor()
    {
        Color random = Color.black;

        while(random.r + random.b + random.g < 1f)
        {
            random = new Color(Random.value, Random.value, Random.value);
        }

        colorKeys[0].color = random;
        colorKeys[0].time = 0f;
        alphaKeys[0].alpha = 1f;
        alphaKeys[0].time = 0f;
    }

    private void SetFirstColor()
    {
        colorKeys[0].color = finalColor;
        colorKeys[0].time = 0f;
        alphaKeys[0].alpha = 1f;
        alphaKeys[0].time = 0f;
    }

    private void GenerateLastColor()
    {
        Color random = Color.black;

        while (random.r + random.b + random.g < 1f)
        {
            random = new Color(Random.value, Random.value, Random.value);
        }

        colorKeys[1].color = random;
        colorKeys[1].time = 1f;
        alphaKeys[1].alpha = 1f;
        alphaKeys[1].time = 1f;

        finalColor = colorKeys[1].color;
    }
}
