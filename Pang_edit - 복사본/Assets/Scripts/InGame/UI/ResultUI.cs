using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultUI : MonoBehaviour
{
    public float fillSpeed;
    public int scoreSpeed;
    public int scoreScale;
    public GameObject resultScore;
    public Image[] gauges;

    private ShowNumber score_result;
    private InGameData data;
    private int currScore = 0;
    private float[] currPoint = new float[5];
    void OnEnable()
    {
        data = GameObject.FindObjectOfType<InGameData>();
        score_result = resultScore.GetComponent<ShowNumber>();
        StartCoroutine(FillScore());
        StartCoroutine(FillGauge());
    }


    IEnumerator FillScore()
    {
        while (currScore <= data.score)
        {
            if (currScore + scoreSpeed > data.score)
            {
                currScore = data.score;
                score_result.PrintNumber(currScore);
                yield break;
            }
            currScore += scoreSpeed;
            scoreSpeed += scoreScale;
            score_result.PrintNumber(currScore);
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    IEnumerator FillGauge()
    {
        while (!IsFillGauge())
        {
            for (int i = 0; i < gauges.Length; i++)
            {
                gauges[i].fillAmount = currPoint[i] / (float)data.maxPoint;
            }
            yield return new WaitForEndOfFrame();
        }
        yield break;

    }

    bool IsFillGauge()
    {
        int check = 0;
        for (int i = 0; i < 5; i++)
        {
            if (currPoint[i] >= data.point[i])
            {
                currPoint[i] = data.point[i];
                check++;
            }
            else
            {
                currPoint[i] += Time.deltaTime * fillSpeed;
            }
        }
        if (check == 5)
        {
            for (int i = 0; i < gauges.Length; i++)
                gauges[i].fillAmount = currPoint[i] / (float)data.maxPoint;

            return true;
        }
        return false;
    }
    public void OnResultOkButtonDown()
    {
        Application.LoadLevel(0);
    }
}
