using UnityEngine;
using System.Collections;

public class LogoScene : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(LogoProcess());
    }

    IEnumerator LogoProcess()
    {
        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        yield return new WaitForSeconds(2f);
        StartCoroutine(SceneFader.Instance.FadeOut(0.4f,"Title"));
    }

}
