using UnityEngine;
using System.Collections;

public class QuitApplication : MonoBehaviour
{
    void OnApplicationQuit()
    {
        PlayerData.Instance.Save();
    }
}
