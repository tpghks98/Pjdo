using UnityEngine;
using System.Collections;

public class VariationScale : MonoBehaviour {


    // Variable
    private GameObject m_goTarget;

    private bool    m_IsAutoTime;
    private bool    m_IsStartBack;

    private float   m_fCurrTime;
    private Vector3 m_vStartScale;
    private Vector3 m_vEndScale;

    // Property

    public float CurrTime
    {
        get { return m_fCurrTime;   }
        set { m_fCurrTime = value;  }
    }



    public void Setup(Vector3 vStart, Vector3 vEnd,
        GameObject go, bool IsStartBack, bool IsAutoTime)
    {
        m_vStartScale = vStart;
        m_vEndScale = vEnd;
        m_goTarget = go;

        m_IsStartBack = IsStartBack;
        m_IsAutoTime = IsAutoTime;
    }

    void Awake()
    {
        m_IsAutoTime    = false;
        m_IsStartBack   = false;
        m_vStartScale   = Vector3.zero;
        m_vEndScale     = Vector3.zero;

        m_goTarget      = null;
    }


	void Update () {
        if (m_IsAutoTime)
        {
            m_fCurrTime += Time.deltaTime;
        }
        if (m_fCurrTime >= 1.0f)
        {
            m_fCurrTime = 1.0f;
        }

        if (m_IsStartBack)
        {
            float fLerpTime = 0.0f;
            if (m_fCurrTime < 0.5f)
            {
                fLerpTime = m_fCurrTime * 2.0f;
                transform.localScale = Vector3.Lerp(m_vStartScale, m_vEndScale
                    , fLerpTime * EasingUtil.easeOutBack(0, 1, fLerpTime) * 3.5f);
            }
            else
            {
                fLerpTime = 1.0f - ( ( m_fCurrTime - 0.5f ) * 2.0f );
                transform.localScale = Vector3.Lerp(m_vStartScale, m_vEndScale
                               , fLerpTime * EasingUtil.easeInCubic(0, 1, fLerpTime) * 3.5f);
            }
        }
        else
        {
            transform.localScale = Vector3.Lerp(m_vStartScale, m_vEndScale, m_fCurrTime);
        }

	}
}
