using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tapToPlay;

    private void Start()
    {
        GameManager.Instance.OnGameFailed += ShowTapToPlay;
        GameManager.Instance.OnGameResetFromBegining += OnResetGame;
        
    }

    private void ShowTapToPlay()
    {
        //var Col = tapToPlay.GetComponent<Text>().color;
        //Col.a = 1f;
        //tapToPlay.GetComponent<Text>().color = Col ;
        StartCoroutine(ChangeAlpha(tapToPlay, 1f, 0.2f, 1f));
    }

    private void OnResetGame()
    {
        //var Col = tapToPlay.GetComponent<Text>().color;
        //Col.a = 0f;
        //tapToPlay.GetComponent<Text>().color = Col;

        StartCoroutine(ChangeAlpha(tapToPlay, 1f, 0f, 0f));
    }

    // to do вынести ChangeAlpha в отдельный класс
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
