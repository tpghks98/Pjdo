using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockGenerator : MonoBehaviour
{
    public string blockID = "BLOCK";
    public int generatorNumber;
    [HideInInspector] public int currBlockCount = 0;

    private int blockMaxCount = 7;
    ObjectPool blockPool = null;
    Transform temp;



    void Start()
    {
        blockPool = ObjectPoolManager.GetPoolbyID(blockID);

        StartCoroutine(SpawnBlock());
    }

    IEnumerator SpawnBlock()
    {
        while (currBlockCount < blockMaxCount)
        {
            temp = blockPool.RequestObject();
            temp.parent = transform;

            ++currBlockCount;
            temp.GetComponent<NormalBlock>().InitWithGenerator(this);

            yield return new WaitForSeconds(0.2f);
        }
        yield return StartCoroutine(LineCheck());
    }


    IEnumerator LineCheck()
    {
        while (true)
        {
            if (currBlockCount < blockMaxCount)
            {
                Debug.Log("떨어지는중");
                temp = FindUnActive();
                ++currBlockCount;
                temp.GetComponent<NormalBlock>().InitWithGenerator(this);

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

    }

    Transform FindUnActive()
    {
        Transform[] tempTS = transform.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < tempTS.Length; i++)
        {
            if (!tempTS[i].gameObject.activeSelf)
            {
                return tempTS[i];
            }
        }
        Debug.LogError("자식중 활성화된 것이 없습니다");
        return null;
    }

}
