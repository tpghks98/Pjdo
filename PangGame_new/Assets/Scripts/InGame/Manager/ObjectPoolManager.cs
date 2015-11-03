using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public Dictionary<string, ObjectPool> objectPoolList = new Dictionary<string, ObjectPool>();

    void Awake()
    {
        ObjectPool childPool = null;

        for (int n = 0; n < transform.childCount; n++)
        {
            childPool = transform.GetChild(n).GetComponent<ObjectPool>();
            objectPoolList.Add(childPool.ID, childPool); 
        }
    }

    // ID를 통해 Pool을 반환 받는 형식
    public static ObjectPool GetPoolbyID(string id)
    {
        ObjectPool refer = null;
        if (Instance.objectPoolList.TryGetValue(id, out refer))
            return refer;
        else
        {
            Debug.LogError("해당하는 ID를 가진 POOL이 없습니다 ID : " + id);
            return null;
        }
    }
}
