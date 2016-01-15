using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public float playTime;

    public Image timeImage;
    public GameObject pauseUI;
    public GameObject resultUI;
    public Image[] gauges;
    public GameObject beforeGame;
    public GameObject timeUP;

    public GameObject liveScore;

    private ShowNumber score_live;
    private InGameData data;

    private float maxTime;

    void Awake()
    {
        maxTime = playTime;
        score_live = liveScore.GetComponent<ShowNumber>();
        data = GameObject.FindObjectOfType<InGameData>();

        beforeGame.SetActive(true);
    }


    void Update()
    {

        if (data.isPause || !data.isStart)
            return;

        if(Input.GetKey(KeyCode.Escape))
        {
            data.isPause = true;
            pauseUI.SetActive(true);
        }
        // Time
        if (playTime > 0 )
        {
            playTime -= Time.deltaTime;
            timeImage.fillAmount = playTime / maxTime;
        }
        else // TimeOver
        {
            data.isPause = true;
            timeUP.SetActive(true);
            StartCoroutine(TimeOverRoutine());
        }


        // Score
        score_live.PrintNumber(data.score);

        // Point
        for (int i = 0; i < gauges.Length; i++)
            gauges[i].fillAmount = data.point[i] / (float)data.maxPoint;

    }

    public void OnApplicationPause(bool pause)
    {
        if(data.isPause)
            return;

        //  pause는 false 
        data.isPause = true;
        pauseUI.SetActive(data.isPause);
    }


    float fallingTime = 1.5f;
    float endY        = 0;
    IEnumerator TimeOverRoutine()
    {
        float currTime = 0.0f;

        while (currTime < fallingTime)
        {
            currTime += Time.deltaTime;
            float y = EasingUtil.bounce(800, endY, currTime / fallingTime);
            timeUP.transform.localPosition = new Vector2(0, y);
            yield return null;
        }

        timeUP.transform.position = new Vector2(0, endY);
        yield return new WaitForSeconds(0.5f);
        timeUP.SetActive(false);
        ShowResult();
    }

    void ShowResult()
    {
        Debug.Log("Show");

        resultUI.SetActive(true);
    }

    #region ButtonEvent

    // Pause
    public void OnPauseButtonDown()
    {
        if (!data.isStart)
            return;

        data.isPause = true;
        pauseUI.SetActive(true);
    }

    public void OnEffectSoundButtonDown(bool set)
    {

    }

    public void OnBGsoundButtonDown()
    {

    }

    public void OnMainMenuButtonDown()
    {

    }

    public void OnContinueButtonDown()
    {
        data.isPause = false;
        pauseUI.SetActive(false);
    }

    #endregion


}
