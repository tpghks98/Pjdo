using UnityEngine;
using System.Collections;

/// <summary>
/// 구름들 관리 스크립트
/// </summary>
public class Clouds : MonoBehaviour
{

    [SerializeField]
    private bool isMovingRight;
    [SerializeField]
    private GameObject[] clouds;

    private InGameData data;

    public  float cloudInterval;

    private float[] randSpeed;
    private Vector2 startPos;
    private float startX;
    private float currTime;
    void Awake()
    {
        data = FindObjectOfType<InGameData>();

        randSpeed = new float[clouds.Length];
        if (isMovingRight)
            startX = -4.2f;
        else
            startX = 4.2f;

        // 랜덤 스피드
        for (int i = 0; i < clouds.Length; i++)
            randSpeed[i] = Random.Range(0.2f, 0.3f);


        StartCoroutine(CloudUpdate());
    }

    IEnumerator CloudUpdate()
    {
        while (true)
        {
            while (!data.isPause)
            {

                currTime += Time.deltaTime;

                for (int i = 0; i < clouds.Length; i++)
                {
                    clouds[i].transform.Translate((isMovingRight ? 1 : -1) * randSpeed[i] * Time.deltaTime * Time.timeScale, 0, 0);
                    IsFinished(clouds[i]);
                }
                yield return null;
            }
            yield return null;
        }
    }

    void IsFinished(GameObject obj)
    {
        if (isMovingRight ?
            obj.transform.localPosition.x >= -startX : obj.transform.localPosition.x <= -startX)
        {
            if (currTime > cloudInterval)
            {
                currTime = 0;
                obj.transform.localPosition = SetStartPos();
            }
        }
    }

    Vector2 SetStartPos()
    {
        startPos = new Vector2(startX, Random.Range(0.28f, 1));
        return startPos;
    }
}
