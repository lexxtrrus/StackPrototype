using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuEffect : MonoBehaviour
{
    [SerializeField] private Image blackPanel;
    [SerializeField] private Text stackLabel;
    [SerializeField] private Text tapToPlayLabel;

    private void OnEnable()
    {
        GameManager.Instance.OnStartMenuShowed += ShowStartMenuEffect;
    }

    private void ShowStartMenuEffect()
    {
        StartCoroutine(ChangeAlpha(stackLabel, 1.5f, 0.3f, 1f));
        StartCoroutine(ChangeAlpha(blackPanel, 1f, 1.8f, 0f));
        StartCoroutine(ChangeAlpha(tapToPlayLabel, 0.7f, 3f, 1f));
    }

    private IEnumerator ChangeAlpha(Graphic colorObject, float timeDuration, float delayBeforeStart, float targetAlpha)
    {
        yield return new WaitForSeconds(delayBeforeStart);

        var startTime = Time.time;
        var timer = startTime + timeDuration;

        Color startColor = colorObject.color;
        Color nextColor = startColor;
        nextColor.a = targetAlpha;
        
        while (Time.time < timer)
        {
            float u = (Time.time - startTime) / timeDuration;
            colorObject.color = Color.Lerp(startColor, nextColor, u);
            yield return null;
        }

        colorObject.color = nextColor;
    }
}
