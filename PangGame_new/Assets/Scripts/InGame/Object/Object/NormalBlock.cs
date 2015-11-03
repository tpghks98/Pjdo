using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class NormalBlock : MonoBehaviour
{
    public Sprite[] colorSprite;
    BlockGenerator gene;
    GameCore core;
    NormalBlock currBlock;

    int randomColor;
    BlockColor blockColor;

    void Awake()
    {
        gene = transform.parent.GetComponent<BlockGenerator>();
        core = GameObject.FindObjectOfType<GameCore>();
    }


    public void InitWithGenerator(BlockGenerator gene)
    {
        // 0은 None으로 두고 1부터 5까지 색깔 
        randomColor = Random.Range(1, 6);
        blockColor = (BlockColor)randomColor;
        
        GetComponent<SpriteRenderer>().sprite = colorSprite[randomColor-1];

        transform.position = gene.transform.position;
        gameObject.SetActive(true);
    }


    void FixedUpdate()
    {
        transform.position = new Vector2(gene.transform.position.x, transform.position.y);
    }



    public BlockColor GetColor()
    {
        return blockColor;
    }


    public void OnDisable()
    {
        gene.currBlockCount--;
    }


    // 터치 관련 부분
    void OnMouseDown()
    {
        if (core.isSelected)
            return;

        Debug.Log("DOWN");
        core.isSelected = true;
        core.selectedColor = blockColor;
        core.selectedBlock.Add(this);
        core.lastBlock = this;
        
    }

    void OnMouseEnter()
    {
        if (!core.isSelected)
            return;

        // 내가 갔었던 곳인지
        for (int i = 0; i < core.selectedBlock.Count; i++)
        {
            if (core.selectedBlock[i] == this)
                return;
        }

        if(core.selectedColor == blockColor)
        {
            if(Mathf.Abs(this.transform.position.x - core.lastBlock.transform.position.x) < 1.2f &&
                Mathf.Abs(this.transform.position.y - core.lastBlock.transform.position.y) < 1.3f) 
            {
                // 조건에 다 맞는것들 여기서 처리
                core.selectedBlock.Add(this);
                core.lastBlock = this;
                return;
            }
        }

    }

    void OnMouseUp()
    {
        
        core.isSelected = false;

        if (core.selectedBlock.Count < 3)
        {
            core.selectedBlock.Clear();
            return;
        }


        for (int i = 0; i < core.selectedBlock.Count; i++)
        {
            core.selectedBlock[i].gameObject.SetActive(false);
            core.lastBlock = null;
        }

        core.selectedBlock.Clear();
        core.selectedColor = BlockColor.None;


    }





}
