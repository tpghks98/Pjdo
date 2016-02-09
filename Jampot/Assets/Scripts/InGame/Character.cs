using UnityEngine;
using System.Collections;


public class Character : MonoBehaviour
{
    Animator ani;

    [SerializeField]
    private float delay;

    void Awake()
    {
        ani = GetComponent<Animator>();


    }

    IEnumerator IdleDelay()
    {
        float currTime = 0.0f;
        while (true)
        {
            if (currTime < delay)
                currTime += Time.deltaTime;
            else
            {
                ani.Play("idle");
                currTime = 0.0f;
            }
            yield return 0;

        }
    }

    public void Pang()
    {
        SetAnimationIndex(2);
    }

    public void Mistake()
    {
        SetAnimationIndex(3);
    }

    public void ReturnNone()
    {
        SetAnimationIndex(0);
    }

    public void SetAnimationIndex(int index)
    {
        ani.SetInteger("AnimationIndex", index);
    }


}
