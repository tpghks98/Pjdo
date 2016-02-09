using UnityEngine;
using System.Collections;

public class BlockGenerator : MonoBehaviour
{
    private int generatorIdx; // 보드판 idx

    private int currBlockCount = 0;
    private int blockMaxCount = 7;
    private Transform temp;
    private int sameInterval;
    private int currIdx = 1;
    private int prevRand = 0;
    private bool onceCheck = true;
    private int randomBlockIdx;


    public int CurrBlockCount
    {
        get { return currBlockCount; }
        set
        {
            if (currBlockCount - value <= 0)
                currBlockCount = 0;
            else
                currBlockCount = value;
        }
    }

    void Start()
    {
        generatorIdx    =   int.Parse(name.Substring(name.Length-1,1));
        sameInterval    =   Random.Range(5, 9);
        StartCoroutine(LineCheck());
    }

    IEnumerator LineCheck()
    {
        while (true)
        {
            if (currBlockCount < blockMaxCount)
            {
                
                temp = FIndUnActive();

                randomBlockIdx = Random.Range(1, 6);

                if (currIdx == sameInterval)
                {
                    randomBlockIdx = prevRand;
                    currIdx = 0;
                }

                GameLogic.Instance.board[generatorIdx, 6 - currBlockCount] =
                    temp.GetComponent<NormalBlock>();



                temp.GetComponent<Block>().InitWithGenerator(this, generatorIdx, 6 - currBlockCount, randomBlockIdx);

                ++currIdx;
                prevRand = randomBlockIdx;
                ++currBlockCount;
                if(currBlockCount == blockMaxCount)
                    onceCheck = true;

                yield return new WaitForSeconds(0.1f);
            }
            else if (onceCheck)
            {
                onceCheck = false;
                if (!GameLogic.Instance.IsCanClick())
                {
                    // 클릭할수 있는곳이 없을때
                    GameLogic.Instance.ResetBoard();
                }

            }
            yield return null;
        }
    }

    public void DownBlock(int pivot)
    {
        NormalBlock[] temps = transform.GetComponentsInChildren<NormalBlock>();

        for (int i = pivot; i > 0; i--)
        {
            GameLogic.Instance.board[generatorIdx, i] = GameLogic.Instance.board[generatorIdx, i - 1];
        }
        for (int i = 0; i < temps.Length; i++)
        {
            if (temps[i]._blockY < pivot)
                temps[i]._blockY++;
        }

    }



    Transform FIndUnActive()
    {
        Transform[] tempGO = transform.GetComponentsInChildren<Transform>(true);

        for (int i = 1; i < tempGO.Length; i += 2)
        {
            if (!tempGO[i].gameObject.activeSelf)
            {
                return tempGO[i];
            }
        }
        // Empty
        Debug.LogError("None Block");
        return null;
    }




}
