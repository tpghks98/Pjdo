using UnityEngine;
using System.Collections;

public class VariationScale : MonoBehaviour
{

    private bool    isAutoTime;
    private bool    isStartBack;

    private float   currTime;
    private Vector3 startScale;
    private Vector3 endScale;

    public float CurrTime
    {
        get { return currTime; }
        set { currTime = value; }
    }


    void Awake()
    {
        isAutoTime  =   false;
        isStartBack =   false;
        startScale  =   Vector3.zero;
        endScale    =   Vector3.zero;

    }

    public void Setup(Vector3 vStart, Vector3 vEnd, bool IsStartBack, bool IsAutoTime)
    {
        startScale = vStart;
        endScale = vEnd;
        
        isStartBack = IsStartBack;
        isAutoTime = IsAutoTime;
    }

    void Update()
    {
        if (isAutoTime)
        {
            currTime += Time.deltaTime;
        }
        if (currTime >= 1.0f)
        {
            currTime = 1.0f;
        }

        if (isStartBack)
        {
            float fLerpTime = 0.0f;
            if (currTime < 0.5f)
            {
                fLerpTime = currTime * 2.0f;
                transform.localScale = Vector3.Lerp(startScale, endScale
                    , fLerpTime * EasingUtil.easeOutBack(0, 1, fLerpTime) * 3.5f);
            }
            else
            {
                fLerpTime = 1.0f - ((currTime - 0.5f) * 2.0f);
                transform.localScale = Vector3.Lerp(startScale, endScale
                               , fLerpTime * EasingUtil.easeInCubic(0, 1, fLerpTime) * 3.5f);
            }
        }
        else
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, currTime);
        }

    }
}
