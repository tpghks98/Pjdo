using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayScene : MonoBehaviour
{
    [SerializeField]
    private float playTime;

    private Image           timeImage;
    private List<Image>     gauges;

    private GameObject timeUP;
    private GameObject pauseUI;
    private GameObject resultUI;
    private GameObject beforeGame;

    private ShowNumber score_live;

    private float maxTime;

    void Awake()
    {
        maxTime     =   playTime;

        timeImage   =   transform.FindChild("TimeTab").FindChild("TimeGauge").GetComponent<Image>();

        gauges      =   new List<Image>();
        for (int i = 0; i < transform.FindChild("GaugeTab").childCount; i++)
            gauges.Add(transform.FindChild("GaugeTab").GetChild(i).GetComponent<Image>());


        timeUP      =   transform.FindChild("TimeUP").gameObject;
        pauseUI     =   transform.FindChild("PauseUI").gameObject;
        resultUI    =   transform.FindChild("ResultUI").gameObject;
        beforeGame  =   transform.FindChild("BeforeGame").gameObject;
        score_live  =   transform.FindChild("ScoreTab").FindChild("Score").GetComponent<ShowNumber>();

        transform.FindChild("PauseButton").GetComponent<Button>().onClick.AddListener(OnPauseButtonDown);


        score_live.LoadNumberResources("InGame/Sprite/UI/ScoreNumber/");
        beforeGame.SetActive(true);
    }

    void Start()
    {
    }

    void Update()
    {

        if (GameLogic.Instance.isPause || !GameLogic.Instance.isStart)
            return;

        InputProcess();
        score_live.PrintNumber(GameLogic.Instance.score);

        // Point
        for (int i = 0; i < gauges.Count; i++)
            gauges[i].fillAmount = GameLogic.Instance.points[i] / (float)GameLogic.Instance.maxPoint;

        #region Time

        if (playTime > 0)
        {
            playTime -= Time.deltaTime;
            timeImage.fillAmount = playTime / maxTime;
        }
        else
        {
            GameLogic.Instance.isStart = false;
            timeUP.SetActive(true);
            StartCoroutine(TimeOverRoutine());
        }

        #endregion


    }

    void InputProcess()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameLogic.Instance.isPause = !GameLogic.Instance.isPause;
            pauseUI.SetActive(GameLogic.Instance.isPause);
        }
    }


    float fallingTime = 1.5f;
    float endY = 0;
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

        timeUP.transform.localPosition = new Vector2(0, endY);
        yield return new WaitForSeconds(0.5f);
        timeUP.SetActive(false);

        resultUI.SetActive(true);

        yield break;
    }


    void OnPauseButtonDown()
    {
        if (!GameLogic.Instance.isStart)
            return;

        GameLogic.Instance.isPause = true;
        pauseUI.SetActive(true);
    }


    public void OnApplicationPause(bool pause)
    {
        if (GameLogic.Instance.isPause)
            return;

        GameLogic.Instance.isPause = true;
        pauseUI.SetActive(GameLogic.Instance.isPause);
    }

}
