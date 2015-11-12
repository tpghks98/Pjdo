using UnityEngine;
using System.Collections;

public class NormalBlock : Block
{
    public Sprite[] colors;
    [SerializeField] private BlockColor color;
    private int rand;

    public override void InitWithGenerator(BlockGenerator _gene)
    {
        base.InitWithGenerator(_gene);
        rand = Random.Range(1,6);

        GetComponent<SpriteRenderer>().sprite = colors[rand - 1];
        color = (BlockColor)rand;

    }

    #region BlockTouch

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (!canTouch)
            return;

        transform.GetChild(0).gameObject.SetActive(true);
        data.selectedColor = color;
        data.selectedBlock.Add(this);
        data.lastBlock = this;
    }


    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();
        if (!canTouch)
            return;

        for (int i = 0; i < data.selectedBlock.Count; i++)
        {
            if (data.selectedBlock[i] == this)
                return;
        }

        if (data.selectedColor == color)
        {
            if (Mathf.Abs(this.transform.position.x - data.lastBlock.transform.position.x) < 1.4f &&
                Mathf.Abs(this.transform.position.y - data.lastBlock.transform.position.y) < 1.3f)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                data.selectedBlock.Add(this);
                data.lastBlock = this;
                return;
            }
        }
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (!canTouch)
            return;

        if (data.selectedBlock.Count < 3)
        {
            for (int i = 0; i < data.selectedBlock.Count; i++)
            {
                data.selectedBlock[i].transform.GetChild(0).gameObject.SetActive(false);
 
            }
            data.selectedColor = BlockColor.None;
            data.lastBlock = null;
            data.selectedBlock.Clear();
            return;
        }

        for (int i = 0; i < data.selectedBlock.Count; i++)
        {
            data.AddScore(10);
            data.selectedBlock[i].transform.GetChild(0).gameObject.SetActive(false);
            data.selectedBlock[i].gameObject.SetActive(false);
        }

        data.lastBlock = null;
        data.selectedBlock.Clear();
        data.selectedColor = BlockColor.None;
    }

    #endregion
}
