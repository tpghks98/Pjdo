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


    public GameObject liveScore;
    public GameObject resultScore;

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
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (!data.isStart)
        //        return;
        //    Debug.Log("Pause!");
        //    data.isPause = !data.isPause; 
        //    pauseUI.SetActive(data.isPause);

        //}

        if (data.isPause || !data.isStart)
            return;
        // Time
        if (playTime > 0 )
        {
            playTime -= Time.deltaTime;
            timeImage.fillAmount = playTime / maxTime;
        }
        else // TimeOver
        {
            TimeOver();
        }


        // Score
        score_live.PrintNumber(data.score);

        // Point
        for (int i = 0; i < gauges.Length; i++)
            gauges[i].fillAmount = data.point[i] / (float)data.maxPoint;

    }

    public void OnApplicationPause(bool pause)
    {
        //  pause는 false 
        data.isPause = true;
        pauseUI.SetActive(data.isPause);
    }


    void TimeOver()
    {
        data.isPause = true;
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
