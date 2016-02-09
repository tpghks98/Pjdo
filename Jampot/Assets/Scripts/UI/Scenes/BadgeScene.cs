using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BadgeScene : MonoBehaviour
{
    List<RectTransform> badgeIconList = new List<RectTransform>();
    List<Text> badgeTxtList = new List<Text>();

    GameObject jobPrefab;
    Transform badgesParent;
    ScrollRect scrollRect;

    [SerializeField]
    private float moveCamTime;
    [SerializeField]
    private float turnTime;

    private const int badgeHeight = 250;


    private bool isGetFirst;
    private int eventIdx = 99;
    private float currTime = 0.0f;

    void Start()
    {
        isGetFirst = false;


        jobPrefab = Resources.Load<GameObject>("Prefabs/JobBadge");
        badgesParent = GameObject.Find("CanvasScroll").transform;
        scrollRect = badgesParent.parent.GetComponent<ScrollRect>();

        Debug.Log(PlayerData.Instance.selectedJob);

        LoadJobs();
        InitBadges();

        PlayerData.Instance.Save();
        StartCoroutine(BadgeEffectRoutine());
    }


    // 1. check the badge pos -> move cam
    // 2. check the first
    // 2-1 if first -> setActive(true) -> rotate -> show
    // 2-2 not first -> change scoretext( up to high score) 
    IEnumerator BadgeEffectRoutine()
    {
        if (eventIdx == 99)
            yield break;


        yield return StartCoroutine(AdjustCam());

        if (isGetFirst)
        {
            yield return StartCoroutine(TurnBadge());
        }

        yield return StartCoroutine(ChangeScore(PlayerData.Instance.jobList[eventIdx].highScore, PlayerData.Instance.currScore));
        PlayerData.Instance.jobList[eventIdx].highScore = PlayerData.Instance.currScore;

        yield break;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerData.Instance.Save();
            SceneManager.LoadScene("InGame");
        }
    }

    #region Routine

    // scrollRect.verticalNormalizedPosition (defalut) = 1
    // scrollRect.verticalNormalizedPosition (under )  = 0
    IEnumerator AdjustCam()
    {
        currTime = 0.0f;
        float targetNormalizedPos = GetCamMovePos();
        float normal;


        while (moveCamTime > currTime)
        {
            currTime += Time.deltaTime;
            normal = Mathf.SmoothStep(1, targetNormalizedPos, currTime / moveCamTime);
            scrollRect.verticalNormalizedPosition = normal;
            yield return null;

        }

        scrollRect.verticalNormalizedPosition = targetNormalizedPos;
        yield break;

    }

    IEnumerator TurnBadge()
    {
        currTime = 0.0f;
        float endRot = 1440;
        float currRot;
        badgeIconList[eventIdx].gameObject.SetActive(true);

        while (turnTime > currTime)
        {
            currTime += Time.deltaTime;
            currRot = EasingUtil.easeOutCubic(0, endRot, currTime / turnTime);
            badgeIconList[eventIdx].localEulerAngles = new Vector3(-90, currRot, 0);
            yield return null;
        }

        badgeIconList[eventIdx].localEulerAngles = new Vector3(-90, 0, 0);
    }

    IEnumerator ChangeScore(float startScore, float endScore)
    {
        float scoreSpeed = 1;
        float scoreScale = 1;

        while (startScore <= endScore)
        {
            if (startScore + scoreSpeed > endScore)
            {
                badgeTxtList[eventIdx].text = endScore.ToString();
                yield break;
            }
            startScore += scoreSpeed;
            scoreSpeed += scoreScale;
            badgeTxtList[eventIdx].text = startScore.ToString();
            yield return null;
        }
        yield break;
    }

    float GetCamMovePos()
    {
        return 1.0f - (float)eventIdx / (badgeIconList.Count - 1);
    }

    #endregion


    #region Load

    void LoadJobs()
    {
        int jobCount = JsonManager.Instance.GetJobCount();

        GameObject temp;

        for (int i = 0; i < jobCount; i++)
        {
            temp = Instantiate(jobPrefab);
            temp.transform.SetParent(badgesParent);
            temp.transform.localScale = Vector3.one;
            temp.transform.localPosition = new Vector3(0, 0 - 256 * i, 0);
            temp.GetComponent<Image>().sprite = Resources.Load<Sprite>("Result/job_" + JsonManager.Instance.GetJobName(i));

            temp.transform.FindChild("Icon").GetChild(0).GetComponent<SpriteRenderer>().sprite
                = Resources.Load<Sprite>("Result/icon_" + JsonManager.Instance.GetJobName(i));
            temp.transform.FindChild("Icon").GetChild(1).GetComponent<SpriteRenderer>().sprite
                = Resources.Load<Sprite>("Result/icon_" + JsonManager.Instance.GetJobName(i));
        }

    }

    void InitBadges()
    {
        for (int i = 0; i < badgesParent.childCount; i++)
        {
            badgeIconList.Add(badgesParent.GetChild(i).FindChild("Icon").GetComponent<RectTransform>());
            badgeTxtList.Add(badgesParent.GetChild(i).FindChild("IsAchieve").GetComponent<Text>());

            if (PlayerData.Instance.jobList[i].isHave)
            {
                badgeIconList[i].gameObject.SetActive(true);
                badgeTxtList[i].text = PlayerData.Instance.jobList[i].highScore.ToString();
            }


            //Debug.Log(PlayerData.Instance.jobList[i].name);
            //Debug.Log(PlayerData.Instance.jobList[i].isHave);
            if (PlayerData.Instance.selectedJob == PlayerData.Instance.jobList[i].name)
            {
                if (!PlayerData.Instance.jobList[i].isHave)
                {
                    PlayerData.Instance.jobList[i].isHave = true;
                    eventIdx = i;
                    isGetFirst = true;
                    continue;
                }
                else if (PlayerData.Instance.jobList[i].highScore < PlayerData.Instance.currScore)
                {
                    eventIdx = i;
                }
            }
        }
    }

    #endregion
}

