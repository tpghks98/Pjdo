using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public enum BlockColor
{
    None = 0,
    Red,
    Yellow,
    Green,
    Blue,
    Purple,
    Gray
};
public enum OBJID
{
    Combo,
    Max
};


public class InGameData : MonoBehaviour
{
    private List<BaseObj> m_lstObject = new List<BaseObj>();

    public List<NormalBlock> selectedBlock = new List<NormalBlock>();

    public float[] linePosY = {-0.8f, -2, -3.1f,-4.3f, -5.5f, -6.7f,-7.8f};
    public bool isStart = false;
    public int score = 0;
    public bool isPause = false;
    public int maxPoint = 100;
    public int[] point = new int[5];
    public int multiplyScore = 1;
    public NormalBlock[,] board = new NormalBlock[6, 7];

    
    private JsonData    jobData;
    private TextAsset   jobText;
    private NormalBlock baseBlock;


    void Awake()
    {
        jobText     =   Resources.Load<TextAsset>("TextFile/JobsRequirement");
        jobData     =   JsonMapper.ToObject(jobText.text);

        PlayerData.getInstance.selectedJob = null;

        this.CreateObject(OBJID.Combo);
    }

    public void AddScore(int add)
    {
        score += add;
    }

    public void AddPoint(int add,BlockColor color)
    {
        if (point[(int)color - 1] + add < maxPoint)
            point[(int)color - 1] += add;
        else
            point[(int)color - 1] = maxPoint;
    }

    public void StartCheck()
    {
        baseBlock = selectedBlock[0];
        CheckAround(baseBlock._blockX, baseBlock._blockY);

        if (selectedBlock.Count > 1)
        {
            FindObjectOfType<Character>().Pang();
        }
        else
        {
            FindObjectOfType<Character>().Mistake();
        }


        for (int i = 0; i < selectedBlock.Count; i++)
        {

            // check 풀기
            selectedBlock[i].isChecked = false;
            // 터지는 조건 
            if (selectedBlock.Count > 1)
            {
                selectedBlock[i].transform.GetChild(0).gameObject.SetActive(true);
                // 점수추가 해야됨 
                if (i == 1)
                {
                    CheckCombo();
                    TransmitAnsToObj(true);
                }
                AddPoint(1, selectedBlock[i].GetBlockColor());
                AddScore(10 * multiplyScore);



            }
            else
            {
                // 틀렸을때
                StartCoroutine(selectedBlock[i].Vibration());
                AddPoint(-3, selectedBlock[i].GetBlockColor()); 
                AddScore(-10);
                multiplyScore = 1;
                TransmitAnsToObj( false );
            }

        }

        selectedBlock.Clear();

    }


    void CheckCombo()
    {
        for (int i = 0; i < m_lstObject.Count; i++)
        {
            if (m_lstObject[i].GetComponent<Combo>() != null)
            {
                int combo = m_lstObject[i].GetComponent<Combo>().ComoboCount;
                // 콤보카운트가 10일때 , 30 미만일때
                if (combo % 10 == 0 && combo > 0 && combo < 30)
                {
                    multiplyScore *= 2;
                }
            }
        }
    }
    // 상하좌우 순
    private void CheckAround(int x,int y)
    {
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    if (IsSameColor(x, y - 1))
                        CheckAround(x, y - 1);
                    break;
                case 1:
                    if (IsSameColor(x, y + 1))
                        CheckAround(x, y + 1);
                    break;
                case 2:
                    if (IsSameColor(x - 1, y))
                        CheckAround(x - 1, y);
                    break;
                case 3:
                    if (IsSameColor(x + 1, y))
                        CheckAround(x + 1, y);
                    break;

            }
        }
    }

    private bool IsSameColor(int pX,int pY)
    {
        // 범위 바깥
        if (pX < 0 || pY < 0 || pX > 5 || pY > 6)
            return false;

        // 색깔이 같을경우 
        if (board[pX, pY].GetBlockColor() == baseBlock.GetBlockColor())
        {
            if(!board[pX,pY].isChecked)
            {
                board[pX, pY].isChecked = true;
                selectedBlock.Add(board[pX, pY]);
                return true;
            }
        }
        return false;
    }

    public bool IsCanClick()
    {

        BlockColor prevColor = BlockColor.None;

        for (int i = 0; i < 7; i++)
        {
            prevColor = BlockColor.None;
            for (int j = 0; j < 6; j++)
            {
                if(board[j,i].GetBlockColor() == prevColor)
                    return true;
                else
                    prevColor   =   board[j,i].GetBlockColor();
            }
        }
        for (int j = 0; j < 6; j++)
        {
            prevColor = BlockColor.None;
            for (int i = 0; i < 7; i++)
            {
                if (board[j, i].GetBlockColor() == prevColor)
                    return true;
                else
                    prevColor   =   board[j,i].GetBlockColor();
            }
        }

        return false;
    }

    public void ResetBoard()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
                board[j,i].gameObject.SetActive(false);
        }
    }


    public void CheckJob()
    {
        // 모든 직업을 탐색을 하면서 직업의 요구수치와 같으면 0 ,멀수록 1당 1증가,
        // 가장 수치가 낮은 것이 직업이됨
        // 같은 수치가 나온경우 -> 리스트에 담은뒤 랜덤으로 출력
        List<string> passJob = new List<string>();
        string selectedJob = null;
        int gapTotal = 0;
        int check = 0;
        int gapMin = 9999;

        for (int i = 0; i < jobData.Count; i++)
        {
            check = 0;
            gapTotal = 0;
            for (int j = 0; j < 5; j++)
            {
                if (point[j] - (int)jobData[i]["Requirement"][j] > 0)
                    check++;
            }

            if (check == 5)
            {

                for (int j = 0; j < 5; j++)
                    gapTotal += point[j] - int.Parse(jobData[i]["Requirement"][j].ToString());

                if (gapTotal == gapMin)
                    passJob.Add(jobData[i]["Name"].ToString());
                else if (gapTotal < gapMin)
                {
                    passJob.Clear();
                    gapMin = gapTotal;
                    selectedJob = jobData[i]["Name"].ToString();
                }
            }
        }
        if(passJob.Count >  0)
            selectedJob = passJob[Random.Range(0,passJob.Count)];

        foreach (var item in PlayerData.getInstance.jobList)
        {
            if (item.name == selectedJob)
            {
                item.isHave = true;
                item.highScore = score;
            }
        }
        PlayerData.getInstance.selectedJob = selectedJob;

    }


    // Object
    public BaseObj CreateObject( OBJID ID)
    {
        GameObject go;
        BaseObj pObj = null;
        go = new GameObject();


        switch (ID)
        {
            case OBJID.Combo:
                pObj = go.AddComponent<Combo>();
                break;
            default:
                GameObject.Destroy(go);
                return null;
        }
        go.transform.position = Vector3.zero;
        go.transform.localScale = Vector3.one;

        pObj.Initialize();
        m_lstObject.Add(pObj);


        go.name = ID.ToString();
        return pObj;
    }
    
    // To all object transmit ans or wrong ans
    private void TransmitAnsToObj( bool IsAns)
    {
        int nCount = m_lstObject.Count;

        if (IsAns)
        {
            for (int n = 0; n < nCount; ++n)
            {
                m_lstObject[n].OnOccurredCorrectAns();
            }
        }
        else
        {
            for (int n = 0; n < nCount; ++n)
            {
                m_lstObject[n].OnOccurredInCorrectAns();
            }
        }
    }
    
}
