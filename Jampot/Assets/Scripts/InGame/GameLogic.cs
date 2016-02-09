using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum BlockType
{
    None = 0,
    Red,
    Yellow,
    Green,
    Blue,
    Purple,
    Item
};


public class GameLogic : Singleton<GameLogic>
{
    public List<Block> selectedBlock { get; set; }

    public int[]    points { get; private set; }
    public int      score { get; private set; }
    public int      multiplyScore{ get; private set; }
    public Block[,] board { get; private set; }

    private int         blockCheckCount;
    private Block       baseBlock;
    private Character   character;
    private Combo       comboObj;

    public bool isStart { get; set; }
    public bool isPause { get; set; }
    public readonly int maxPoint = 100;

    public float[] linePosY { get; private set; }

    void Awake()
    {
        selectedBlock   =   new List<Block>();

        board           =   new Block[6,7];
        character       =   FindObjectOfType<Character>();



        blockCheckCount =   2;
        points          =   new int[5];
        score           =   0;
        multiplyScore   =   1;

        isStart         =   false;
        isPause         =   false;

        linePosY        =   new float[7] {-0.8f, -2, -3.1f, -4.3f, -5.5f, -6.7f, -7.8f};

    }


    void Start()
    {
        comboObj = GameObject.Find("Combo").GetComponent<Combo>();
    }


    public void StartCheck()
    {
        baseBlock   =   selectedBlock[0];

        CheckAround(baseBlock._blockX, baseBlock._blockY);  

        if (selectedBlock.Count >= blockCheckCount)
        {
            character.Pang();
        }
        else
        {
            character.Mistake();
        }

        // Block Process
        BlockProcess();
    }

    private void BlockProcess()
    {
        for (int i = 0; i < selectedBlock.Count; i++)
        {

            // check 풀기
            selectedBlock[i].isChecked = false;
            // 터지는 조건 
            if (selectedBlock.Count >= blockCheckCount)
            {
                selectedBlock[i].transform.GetChild(0).gameObject.SetActive(true);
                // 점수추가 해야됨 
                if (i == 1)
                {
                    CheckCombo();
                    TransmitAnsToCombo(true);
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
                TransmitAnsToCombo(false);
            }

        }

        selectedBlock.Clear();
    }

    private void CheckAround(int x, int y)
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

    private bool IsSameColor(int pX, int pY)
    {
        // 범위 바깥
        if (pX < 0 || pY < 0 || pX > 5 || pY > 6)
            return false;

        // 색깔이 같을경우 
        if (board[pX, pY].GetBlockColor() == baseBlock.GetBlockColor())
        {
            if (!board[pX, pY].isChecked)
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
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                baseBlock = board[j,i];
                selectedBlock.Clear();
                CheckAround(j,i);

                for(int k = 0; k < selectedBlock.Count; k++)
                    selectedBlock[k].isChecked = false;

                if (selectedBlock.Count >= blockCheckCount)
                {
                    selectedBlock.Clear();
                    return true;
                }
            }
        }

        selectedBlock.Clear();
        return false;
    }

    public void ResetBoard()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
                board[j, i].gameObject.SetActive(false);
        }
    }

    public void AddScore(int add)
    {
        score   += add;
    }

    public void AddPoint(int add, BlockType color)
    {
        if (points[(int)color - 1] + add < maxPoint)
            points[(int)color - 1] += add;
        else
            points[(int)color - 1] = maxPoint;
    }

    private void CheckCombo()
    {
        int temp = comboObj.ComboCount;
        
        // 콤보카운트가 10일때 , 30 미만일때
        if (temp % 10 == 0 && temp > 0 && temp < 30)
        {
            multiplyScore *= 2;
        }
    }

    private void TransmitAnsToCombo(bool isAns)
    {
        if (isAns)
            comboObj.IncrementCombo(1);
        else
            comboObj.ResetCombo();
    }


    public void CheckJob()
    {
        // 모든 직업을 탐색을 하면서 직업의 요구수치와 같으면 0 ,멀수록 1당 1증가,
        // 가장 수치가 낮은 것이 직업이됨
        // 같은 수치가 나온경우 -> 리스트에 담은뒤 랜덤으로 출력
        List<string> passJob = new List<string>();
        List<string> PriorityJob = new List<string>();
        string selectedJob = null;
        int gapTotal = 0;
        int check = 0;
        int gapMin = 9999;

        for (int i = 0; i < JsonManager.Instance.GetJobCount(); i++)
        {
            check = 0;
            gapTotal = 0;
            for (int j = 0; j < 5; j++)
            {
                if (points[j] - JsonManager.Instance.GetRequirement(i, j) >= 0)
                    check++;
            }

            if (check == 5)
            {

                for (int j = 0; j < 5; j++)
                    gapTotal += points[j] - JsonManager.Instance.GetRequirement(i, j);

                if (gapTotal == gapMin)
                    passJob.Add(JsonManager.Instance.GetJobName(i));
                else if (gapTotal < gapMin)
                {
                    passJob.Clear();
                    passJob.Add(JsonManager.Instance.GetJobName(i));
                    gapMin = gapTotal;
                }
            }
        }
        if (passJob.Count > 0)
        {
            for (int j = 0; j < passJob.Count; j++)
            {
                // 가지고있는 경우 제외 ( 갖고 있지 않는것부터 우선적으로
                if (!PlayerData.Instance.IsHave(passJob[j]))
                    PriorityJob.Add(passJob[j]);
            }
            if (PriorityJob.Count != 0)
                selectedJob = PriorityJob[Random.Range(0, PriorityJob.Count)];
            else
                selectedJob = passJob[Random.Range(0, passJob.Count)];
        }

        PlayerData.Instance.currScore = score;
        PlayerData.Instance.selectedJob = selectedJob;
    }
}
