using UnityEngine;
using System.Collections;

public abstract class BaseObj : MonoBehaviour {


    public abstract void Initialize();

    public virtual void OnOccurredInCorrectAns() { }
    public virtual void OnOccurredCorrectAns() { }
}
