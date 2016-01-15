using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Result : MonoBehaviour 
{
    List<RectTransform> badgeImgList = new List<RectTransform>();
    List<Text>  badgeTxtList    =   new List<Text>();

    Transform badgesParent;
    ScrollRect  scrollRect;

    [SerializeField]
    private float moveCamTime       =   1;
    [SerializeField]
    private float turnTime          =   1.5f;

    private const int badgeHeight   =   250;

    bool    isEffectRoutine     =       false;
    int     eventIdx;
    float   currTime            =       0.0f;
    void Awake()
    {
        badgesParent    =   GameObject.Find("CanvasScroll").transform;
        scrollRect      =   badgesParent.parent.GetComponent<ScrollRect>();

        

        #region Load data
        for (int i = 0; i < badgesParent.childCount; i++)
        {
            badgeImgList.Add(badgesParent.GetChild(i).FindChild("Icon").GetComponent<RectTransform>());
            badgeTxtList.Add(badgesParent.GetChild(i).FindChild("IsAchieve").GetComponent<Text>());

            // isBadgeHave
            if (PlayerData.getInstance.jobList[i].isHave)
            {
                // isFirst -> First Get Badge
                if (PlayerData.getInstance.jobList[i].isFirst)
                {
                    PlayerData.getInstance.jobList[i].isFirst = false;
                    eventIdx = i;
                    isEffectRoutine = true;
                    continue;
                }

                badgeImgList[i].gameObject.SetActive(true);
                badgeTxtList[i].text = PlayerData.getInstance.jobList[i].highScore.ToString();
            }
        }
        #endregion
        eventIdx = 3;
        Debug.Log(GetCamMovePos());

        StartCoroutine(BadgeEffectRoutine());
        // firstGetBadge
        //if (isEffectRoutine)
        //    StartCoroutine(BadgeEffectRoutine());
    }


    // 1. check the badge pos -> move cam
    // 2. check the first
    // 2-1 if first -> setActive(true) -> rotate -> show
    // 2-2 not first -> change scoretext( up to high score) 
    IEnumerator BadgeEffectRoutine()
    {
        yield return StartCoroutine(AdjustCam());
        yield return StartCoroutine(TurnBadge());
        yield return StartCoroutine(ChangeScore(0,10000));
    }


    // scrollRect.verticalNormalizedPosition (defalut) = 1
    // scrollRect.verticalNormalizedPosition (under )  = 0
    IEnumerator AdjustCam()
    {
        currTime  = 0.0f;
        float targetNormalizedPos  = GetCamMovePos();
        float normal;


        while(moveCamTime > currTime)
        {
            currTime += Time.deltaTime;
            normal = Mathf.SmoothStep(1,targetNormalizedPos,currTime/moveCamTime);
            scrollRect.verticalNormalizedPosition = normal;
            yield return null;
            
        }

        scrollRect.verticalNormalizedPosition = targetNormalizedPos;
        yield break;

    }


    IEnumerator TurnBadge()
    {
        currTime = 0.0f;
        float endRot   = 1440;
        float currRot;
        badgeImgList[eventIdx].gameObject.SetActive(true);

        while (turnTime > currTime)
        {
            currTime    +=  Time.deltaTime;
            currRot     =   EasingUtil.easeOutCubic(0,endRot,currTime / turnTime);
            badgeImgList[eventIdx].localEulerAngles = new Vector3(-90,currRot,0);
            yield return null;
        }

        badgeImgList[eventIdx].localEulerAngles = new Vector3(-90, 0, 0);
    }

    
    float scoreSpeed = 1;
    float scoreScale = 1;
    IEnumerator ChangeScore(float startScore, float endScore)
    {
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
        return 1.0f - (float)eventIdx / (badgeImgList.Count -1);
    }
    
    
}

