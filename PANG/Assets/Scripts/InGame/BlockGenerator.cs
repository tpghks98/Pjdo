using UnityEngine;
using System.Collections;

public class BlockGenerator : MonoBehaviour
{
    private int currBlockCount = 0;
    private int blockMaxCount  = 7;

    private Transform temp;
    void Awake()
    {
        StartCoroutine(LineCheck());
        
    }

    IEnumerator LineCheck()
    {
        while (true)
        {
            if(currBlockCount < blockMaxCount)
            {

                temp = FIndUnActive();
                ++currBlockCount;

                temp.GetComponent<NormalBlock>().InitWithGenerator(this);

                yield return new WaitForSeconds(0.1f);
            }
                yield return null;
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
