using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseUI : MonoBehaviour
{

    void Awake()
    {
        transform.FindChild("BGSound").GetComponent<Toggle>().onValueChanged.AddListener(OnEffectSoundButtonDown);
        transform.FindChild("EffectSound").GetComponent<Toggle>().onValueChanged.AddListener(OnBGsoundButtonDown);

        transform.FindChild("MainMenu").GetComponent<Button>().onClick.AddListener(OnMainMenuButtonDown);
        transform.FindChild("Continue").GetComponent<Button>().onClick.AddListener(OnContinueButtonDown);
    }
    void OnEffectSoundButtonDown(bool set)
    {
    }

    void OnBGsoundButtonDown(bool set)
    {

    }

    void OnMainMenuButtonDown()
    {

    }

    void OnContinueButtonDown()
    {
        GameLogic.Instance.isPause = false;
        gameObject.SetActive(false);
    }
}
