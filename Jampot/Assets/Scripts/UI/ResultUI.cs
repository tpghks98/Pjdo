using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class ResultUI : MonoBehaviour
{
    private float fillSpeed = 70;
    private int scoreSpeed = 1;
    private int scoreScale = 1;
    private List<Image> gauges;


    private ShowNumber scoreResult;
    private int currScore = 0;
    private float[] currPoint = new float[5];


    void OnEnable()
    {
        transform.FindChild("OkButton").GetComponent<Button>().onClick.AddListener(OnResultOkButtonDown);

        scoreResult = transform.FindChild("Score").GetComponent<ShowNumber>();
        scoreResult.LoadNumberResources("InGame/Sprite/UI/ResultNumber/");

        gauges = new List<Image>();
        for (int i = 0; i < transform.FindChild("GuageFrame").childCount; i++)
            gauges.Add(transform.FindChild("GuageFrame").GetChild(i).GetComponent<Image>());

        StartCoroutine(FillScore());
        StartCoroutine(FillGauge());
    }



    IEnumerator FillScore()
    {
        while (currScore <= GameLogic.Instance.score)
        {
            if (currScore + scoreSpeed > GameLogic.Instance.score)
            {
                currScore = GameLogic.Instance.score;
                scoreResult.PrintNumber(currScore);
                yield break;
            }
            currScore += scoreSpeed;
            scoreSpeed += scoreScale;
            scoreResult.PrintNumber(currScore);
            yield return null;
        }
        yield break;
    }


    IEnumerator FillGauge()
    {
        while (!IsFillGauge())
        {
            for (int i = 0; i < gauges.Count; i++)
            {
                gauges[i].fillAmount = currPoint[i] / (float)GameLogic.Instance.maxPoint;
            }
            yield return null;
        }
        yield break;

    }

    bool IsFillGauge()
    {
        int check = 0;
        for (int i = 0; i < 5; i++)
        {
            if (currPoint[i] >= GameLogic.Instance.points[i])
            {
                currPoint[i] =  GameLogic.Instance.points[i];
                check++;
            }
            else
            {
                currPoint[i] += Time.deltaTime * fillSpeed;
            }
        }
        if (check == 5)
        {
            for (int i = 0; i < gauges.Count; i++)
                gauges[i].fillAmount = currPoint[i] / (float)GameLogic.Instance.maxPoint;

            return true;
        }
        return false;
    }



    public void OnResultOkButtonDown()
    {
        GameLogic.Instance.CheckJob();
        StartCoroutine(SceneFader.Instance.FadeOut(2f,"Badge"));
    }
}
