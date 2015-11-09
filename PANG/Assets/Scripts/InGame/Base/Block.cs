using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour 
{
    protected BlockGenerator gene;
    protected InGameData data;
    protected void Awake()
    {
        data = GameObject.FindObjectOfType<InGameData>();
    }

    public void InitWithGenerator(BlockGenerator _gene)
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
        data.isSelected = false;
    }

    protected virtual void OnMouseEnter()
    {
        if (!data.isSelected)
            return;


    }

    protected virtual void OnMouseUp()
    {
        data.isSelected = false;

    }


}
