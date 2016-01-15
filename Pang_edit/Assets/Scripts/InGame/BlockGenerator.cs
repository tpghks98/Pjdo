using UnityEngine;
using System.Collections;

public class BlockGenerator : MonoBehaviour
{
    public int generatorIdx; // 보드판 idx

    private int currBlockCount = 0;
    private int blockMaxCount  = 7;
    private Transform temp;
    private InGameData data;
    private int sameInterval;
    private int currIdx = 1;
    private int prevRand = 0;

    private bool onceCheck = true;
    void Awake()
    {
        data = GameObject.FindObjectOfType<InGameData>();
    }
    void Start()
    {
        sameInterval    =   Random.Range(5,9);
        StartCoroutine(LineCheck());
        
    }

    IEnumerator LineCheck()
    {
        int rand = 0;
        while (true)
        {
            if(currBlockCount < blockMaxCount)
            {
                onceCheck = true;
                temp = FIndUnActive();
                
                rand = Random.Range(1, 6);

                if(currIdx == sameInterval)
                {
                    rand = prevRand;
                    currIdx = 0;
                }

                data.board[generatorIdx, 6 - currBlockCount] = 
                    temp.GetComponent<NormalBlock>();

                

                temp.GetComponent<NormalBlock>().InitWithGenerator(this, generatorIdx, 6 - currBlockCount, rand);

                currIdx++;
                prevRand = rand;
                ++currBlockCount;
                yield return new WaitForSeconds(0.1f);
            }
            else if (onceCheck)
            {
                onceCheck = false;
                if (!data.IsCanClick())
                {
                    // 클릭할수 있는곳이 없을때
                    data.ResetBoard();
                }
                
            }

            yield return null;
        }
    }


    public void DownBlock(int pivot)
    {
        NormalBlock[] temps = transform.GetComponentsInChildren<NormalBlock>();

        for (int i = pivot; i >0; i--)
        {
            data.board[generatorIdx, i] = data.board[generatorIdx, i - 1];
        }
        for (int i = 0; i < temps.Length; i++)
        {
            if (temps[i]._blockY < pivot)
                temps[i]._blockY++;
        }

    }
    public void MinusCurrBlockCount(int minusValue)
    {
        if (currBlockCount - minusValue <= 0)
            currBlockCount = 0;
        else
            currBlockCount -= minusValue;
    }



    Transform FIndUnActive()
    {
        Transform [] tempGO = transform.GetComponentsInChildren<Transform>(true);

        for (int i = 1; i < tempGO.Length; i += 2)
        {
            if(!tempGO[i].gameObject.activeSelf)
            {
                return tempGO[i];
            }
        }
        // Empty
        Debug.LogError("None Block");
        return null;
    }
}
