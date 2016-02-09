using UnityEngine;
using System.Collections;

public class Combo : MonoBehaviour
{
    private float   currTime;
    private int     comboCount;
    private float   maxTime;
    public int ComboCount
    {
        get { return comboCount; }
    }

    private ShowNumber      showNumber;
    private VariationScale  variationScale;
    void Awake()
    {

        showNumber = gameObject.AddComponent<ShowNumber>();
        showNumber.LoadNumberResources("InGame/Sprite/UI/Combo/Num/");

        variationScale  =   gameObject.AddComponent<VariationScale>();
        variationScale.Setup(Vector3.zero, Vector3.one, true, false);


        comboCount      =   0;
        maxTime         =   1.5f;
        currTime        =   maxTime;  
    }

    void Update()
    {
        TimeUpdate();

        showNumber.PrintNumber(comboCount);
    }

    private void TimeUpdate()
    {
        if (GameLogic.Instance.isPause)
            return;

        currTime += Time.deltaTime;
        if (CheckLimitTime())
        {
            currTime = maxTime;
            ResetCombo();
        }
        variationScale.CurrTime = currTime / maxTime;
    }

    private bool CheckLimitTime()
    {
        return maxTime <= currTime;
    }


    public void IncrementCombo(int n)
    {
        comboCount += n;
        currTime = 0.0f;
    }

    public void ResetCombo()
    {
        comboCount  =   0;
        currTime    =   maxTime;
    }


    
}
