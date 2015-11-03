using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public List<Transform> pool = new List<Transform>();
    public int currentIndex = 0;
    public string ID;
    public Transform prefab = null;
    public int currentSize = 10;
    public int addSize = 5;

    int CountUnActiveObjects()
    {
        int unactiveCount = 0;
        for (int n = 0; n < pool.Count; n++)
        {
            if (!pool[n].gameObject.activeInHierarchy)
                unactiveCount++;
        }

        return unactiveCount;
    }

    void Awake()
    {
        if (prefab == null)
        {
            Debug.LogError(name + "에서 초기화시 사용할 prefabs이 null 입니다");
            return;
        }

        Transform toMake;
        for (int n = 0; n < currentSize; n++)
        {
            toMake = Instantiate(prefab) as Transform;
            toMake.parent = transform; // 생성한 obj;
            toMake.gameObject.SetActive(false);
            pool.Add(toMake);
        }
    }

    public Transform RequestObject()
    {
        if (currentIndex >= currentSize)
            currentIndex = 0;

        if (pool[currentIndex].gameObject.activeInHierarchy)
        {
            for (int n = 0; n < currentSize; n++)
            {
                if (!pool[n].gameObject.activeInHierarchy)
                {
                    currentIndex = n;
                    return pool[currentIndex++];
                }
            }

            if (addSize == 0)
            {
                Debug.LogError("더이상 새로운 Obj를 만들수없습니다");
                return null;
            }

            Transform toMake;
            for (int n = 0; n < addSize; n++)
            {
                toMake = Instantiate(prefab) as Transform;

                pool.Add(toMake);
                toMake.gameObject.SetActive(false);
                toMake.parent = transform;
            }

            currentIndex = currentSize;
            currentSize += addSize;

        }

        return pool[currentIndex++];
    }


    
};