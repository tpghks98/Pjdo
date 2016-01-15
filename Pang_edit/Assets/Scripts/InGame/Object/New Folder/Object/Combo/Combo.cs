using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class Combo : BaseObj {


    // 틀렸을때 알아서 리셋 ( -> 0 )
    // 맞았을때 알아서 m_nIncreaseValue 값만큼씩증가
    // 맞았을떄 여러개 증가시키려면 m_nIncreaseValue 건들이면됌
    // 현재 콤보가 몇인지 얻어올려면 ComboCount 호출

    // ComboCount -> 읽
    // IncreaseValue -> 읽 . 쓰

    // Variable
    private int     m_nComboCount;
    private int     m_nIncreaseValue;
    private float   m_fCurrTime;
    private float   m_fMaxTime;

    private Image           m_pComboObj;
    private ShowNumber      m_pShowNumber;
    private VariationScale  m_pVariationScale;
    private InGameData      m_pInGameData;

    // Property
    public int ComoboCount
    {
        get { return m_nComboCount; }
    }
    public int IncreaseValue
    {
        get { return m_nIncreaseValue; }
        set { m_nIncreaseValue = value; }
    }
    
    // Function
    public override void Initialize()
    {
        m_nIncreaseValue = 1;
        m_nComboCount = 0;
        m_fMaxTime = 1.5f;
        m_fCurrTime = m_fMaxTime;        
    }
    public override void OnOccurredInCorrectAns()
    {
        ResetCombo();
    }
    public override void OnOccurredCorrectAns()
    {
        IncrementCombo( m_nIncreaseValue );
    }



	void Start () {
        m_pInGameData   =   FindObjectOfType<InGameData>();
        m_pShowNumber = gameObject.AddComponent<ShowNumber>();
        m_pShowNumber.LoadNumberResources("InGame/Sprite/UI/Combo/Num/");
        m_pShowNumber.Initialize();

        this.SetParent(  gameObject,  GameObject.Find("ComboTab") );



        // Combo Image Load and Create -> 나중에 Mgr로 기능만 이동하면 좋음
        GameObject go = new GameObject();

        this.SetParent(go, gameObject);
        go.transform.localScale = Vector3.one;
        m_pComboObj =  go.AddComponent<Image>();
        m_pComboObj.sprite 
            = Resources.Load("InGame/Sprite/UI/Combo/combo" 
            , typeof( Sprite ) ) as Sprite;
        m_pComboObj.SetNativeSize();


        // 포지션 세팅 밖으로 뺴고싶다
        go.transform.localPosition = transform.localPosition
            + new Vector3( 175.0f, 0.0f, 0.0f );
        Vector3 v = new Vector3(-109, 323, 0);
        transform.localPosition = v;

        m_pVariationScale =
            gameObject.AddComponent<VariationScale>();
        m_pVariationScale.Setup( Vector3.zero, Vector3.one, gameObject, true, false );
        

	}

    // 이것도 Mgr에 이동시키는게 좋음
    private void SetParent(GameObject goChild, GameObject goParent)
    {
        goChild.transform.SetParent(goParent.transform);
        goChild.transform.localScale = Vector3.one;
        goParent.transform.localScale = Vector3.one;
    }
	void Update () {
        TimeUpdate();

        m_pShowNumber.PrintNumber(m_nComboCount);
	}


    private void TimeUpdate()
    {
        if(m_pInGameData.isPause)
            return;

        m_fCurrTime += Time.deltaTime;
        if (CheckLimitTime() )
        {
            m_fCurrTime = m_fMaxTime;
            ResetCombo();
        }
        m_pVariationScale.CurrTime = m_fCurrTime / m_fMaxTime;
    }

    private bool CheckLimitTime()
    {
        return m_fMaxTime <= m_fCurrTime;
    }

    private void IncrementCombo( int n)
    {
        m_nComboCount += n;
        m_fCurrTime = 0.0f;
    }

    private void ResetCombo()
    {
        m_nComboCount = 0;
        m_fCurrTime = m_fMaxTime;
    }



}
