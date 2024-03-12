using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGameHideElements : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stackLabel;
    [SerializeField] private TextMeshProUGUI tapToPlayLabel;

    private void Start()
    {
        GameManager.Instance.OnGameStart += StartHideLabel;
    }

    private void StartHideLabel()
    {
        StartCoroutine(ChangeAlpha(stackLabel, 0.3f, 0f, 0f));
        StartCoroutine(ChangeAlpha(tapToPlayLabel, 0.3f, 0f, 0f));
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
