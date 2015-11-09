using UnityEngine;
using System.Collections;

public class NormalBlock : Block
{
    private BlockColor color = BlockColor.Red;

    #region BlockTouch

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        transform.GetChild(0).gameObject.SetActive(true);
        data.selectedColor = color;
        data.selectedBlock.Add(this);
        data.lastBlock = this;
    }


    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();


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

        if (data.selectedBlock.Count < 3)
        {
            for (int i = 0; i < data.selectedBlock.Count; i++)
            {
                data.selectedBlock[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            data.selectedBlock.Clear();
            return;
        }

        for (int i = 0; i < data.selectedBlock.Count; i++)
        {
            data.selectedBlock[i].transform.GetChild(0).gameObject.SetActive(false);
            data.selectedBlock[i].gameObject.SetActive(false);
        }

        data.lastBlock = null;
        data.selectedBlock.Clear();
        data.selectedColor = BlockColor.None;
    }

    #endregion
}
