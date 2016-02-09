using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour
{
    void Awake()
    {

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(SceneFader.Instance.FadeOut(0.6f,"InGame"));
    }



}
