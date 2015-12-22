using UnityEngine;
using System.Collections;

public class BeforeGame : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 endPos;
    [SerializeField] private float duration;
    [SerializeField] private GameObject readyImg;
    [SerializeField] private GameObject startImg;
    private InGameData data;

    void OnEnable()
    {
        data = FindObjectOfType<InGameData>();
        StartCoroutine(Ready());
    }

    IEnumerator Ready()
    {
        float beginTime = Time.time;

        while (Time.time - beginTime <= duration)
        {
            float t = (Time.time - beginTime) / duration;

            float x = EasingUtil.easeOutBack(startPos.x, endPos.x, t);
            float y = EasingUtil.easeOutBack(startPos.y, endPos.y, t);
            transform.localPosition = new Vector2(x, y);

            yield return 0;
        }
        transform.position = endPos;

        readyImg.SetActive(false);
        startImg.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
        data.isStart = true;

        yield break;
        
    }

}
