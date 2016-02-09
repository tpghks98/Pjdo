using UnityEngine;
using System.Collections;

public class BeforeGame : MonoBehaviour
{
    [SerializeField]
    private Vector2 startPos;
    [SerializeField]
    private Vector2 endPos;
    [SerializeField]
    private float duration;
    [SerializeField]
    private GameObject readyImg;
    [SerializeField]
    private GameObject startImg;


    void OnEnable()
    {
        transform.localPosition = startPos;
        StartCoroutine(Ready());
    }

    //IEnumerator Ready()
    //{
    //    float beginTime = Time.unscaledTime;

    //    while (Time.unscaledTime - beginTime <= duration)
    //    {
    //        float t = (Time.unscaledTime - beginTime) / duration;
    //        Debug.Log(Time.unscaledTime - beginTime);
    //        float x = EasingUtil.easeOutBack(startPos.x, endPos.x, t);
    //        float y = EasingUtil.easeOutBack(startPos.y, endPos.y, t);
    //        transform.localPosition = new Vector2(x, y);

    //        yield return null;
    //    }
    //    transform.localPosition = endPos;

    //    readyImg.SetActive(false);
    //    startImg.SetActive(true);
    //    yield return StartCoroutine(WaitForRealSeconds(0.5f));

    //    Time.timeScale = 1;
    //    gameObject.SetActive(false);

    //    yield break;
    //}

    IEnumerator Ready()
    {
        float currTime = 0.0f;

        while(currTime < duration)
        {

            currTime += Time.deltaTime;
            float x = EasingUtil.easeOutBack(startPos.x, endPos.x, currTime / duration);
            float y = EasingUtil.easeOutBack(startPos.y, endPos.y, currTime / duration);

            transform.localPosition = new Vector2(x,y);
            yield return null;
        }
        transform.localPosition = endPos;

        readyImg.SetActive(false);
        startImg.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
        GameLogic.Instance.isStart = true;


    }


}
