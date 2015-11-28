using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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


public class InGameData : MonoBehaviour
{
    public List<NormalBlock> selectedBlock = new List<NormalBlock>();

    public bool isStart = false;
    public int score = 0;
    public bool isPause = false;
    public int maxPoint = 100;
    public float[] point = new float[5];
    public NormalBlock[,] board = new NormalBlock[6, 7];


    private NormalBlock baseBlock;
    void Awake()
    {

        Debug.Log("Data Awake");
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
            Debug.Log("PANG");
            FindObjectOfType<Character>().Pang();
        }
        else
            FindObjectOfType<Character>().Mistake();


        for (int i = 0; i < selectedBlock.Count; i++)
        {

            // check 풀기
            selectedBlock[i].isChecked = false;
            // 터지는 조건 
            if (selectedBlock.Count > 1)
            {
                selectedBlock[i].transform.GetChild(0).gameObject.SetActive(true);
                // 점수추가 해야됨 
                AddPoint(1, selectedBlock[i].GetBlockColor());
                AddScore(10);
            }
            else
            {
                // 틀렸을때
                selectedBlock[i].ChangeColor(BlockColor.Gray);
                AddPoint(-3, selectedBlock[i].GetBlockColor()); 
                AddScore(-10);
            }

        }

        selectedBlock.Clear();
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
}
