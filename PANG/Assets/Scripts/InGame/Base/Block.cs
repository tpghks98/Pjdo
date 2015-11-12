using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour 
{
    protected BlockGenerator gene;
    protected InGameData data;
    protected bool canTouch = true;

    protected void Awake()
    {
        data = GameObject.FindObjectOfType<InGameData>();
    }

    public virtual void InitWithGenerator(BlockGenerator _gene)
    {
        gene  = _gene;

        transform.position = gene.transform.position;
        gameObject.SetActive(true);
    }

    protected void OnDisable()
    {
        gene.MinusCurrBlockCount(1);
    }

    protected virtual void OnMouseDown()
    {
        canTouch = !data.isPause;


        if (data.isSelected)
            canTouch = false;

        data.isSelected = true;
    }

    protected virtual void OnMouseEnter()
    {
        canTouch = !data.isPause;

        if (!data.isSelected)
            canTouch = false;

    }

    protected virtual void OnMouseUp()
    {
        canTouch = !data.isPause;

        data.isSelected = false;

    }


}
