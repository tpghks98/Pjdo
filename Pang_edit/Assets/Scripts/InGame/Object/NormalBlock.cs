using UnityEngine;
using System.Collections;


public class NormalBlock : MonoBehaviour
{
    public Sprite[] colors;
    [SerializeField]
    private BlockColor color;

    [SerializeField]
    private float moveTime = 0.2f;
    [SerializeField]
    private float vibrationTime = 0.2f;

    public  int _blockX;
    public  int _blockY;
    public  bool isChecked = false;
    public  bool isEffecting;

    private BlockGenerator  gene;
    private Animator        ani;
    private InGameData data;
    private bool canTouch = false;
    private BlockColor prevColor = BlockColor.None;
    
    private bool once = true;

    void Awake()
    {
        data = GameObject.FindObjectOfType<InGameData>();
    }

    public void InitWithGenerator(BlockGenerator _gene,int blockX, int blockY, int _rand)
    {
        _blockX = blockX;
        _blockY = blockY;

        once = true;
        gene = _gene;
        isEffecting =   false;

        transform.position = gene.transform.position;
        gameObject.SetActive(true);

        color = (BlockColor)_rand;
        ChangeColor(color);

    }

    public void UpdateBoard()
    {
        gene.DownBlock(_blockY);
        gene.MinusCurrBlockCount(1);
    }

    void OnDisable()
    {
        // data.board[_blockX, _blockY] = (int)BlockColor.None;
        gene.DownBlock(_blockY);
        gene.MinusCurrBlockCount(1);


    }

    void Update()
    {
        if(data.linePosY[_blockY] != transform.localPosition.y)
        {
            if (once)
            {
                once = false;
                if (transform.localPosition.y == 0)
                    StartCoroutine(MoveProcess(data.linePosY[_blockY], moveTime+0.2f));
                else
                    StartCoroutine(MoveProcess(data.linePosY[_blockY],moveTime));
            }
        }
        if(data.isStart)
            canTouch = !data.isPause;
    }
   


    IEnumerator MoveProcess(float targetY,float moveT)
    {
        float currTime = 0.0f;
        float saveY     =   _blockY;

        while (currTime < moveT)
        {
            if (saveY != _blockY)
            {
                once = true;
                yield break;
            }

            currTime += Time.deltaTime;
            float y = EasingUtil.linear(transform.localPosition.y, targetY, currTime / moveT);
            transform.localPosition = new Vector2(transform.localPosition.x, y);
            yield return null;
        }

        transform.localPosition = new Vector2(transform.localPosition.x, targetY);


        once = true;
        yield break;

    }
    public BlockColor GetBlockColor()
    {
        return color;
    }

    public void ChangeColor(BlockColor index)
    {
        GetComponent<SpriteRenderer>().sprite = colors[(int)index-1];
        if (index == BlockColor.Gray)
        {
            prevColor = color;
            StartCoroutine(GrayDelay(2));
        }
    }

    IEnumerator GrayDelay(int duration)
    {
        float currTime = 0.0f;
        while(currTime < duration)
        {
            if (!data.isPause)
                currTime += Time.deltaTime;

            yield return null;
        }

        ChangeColor(prevColor);
        yield return null;

    }

    void OnMouseDown()
    {
        if (!canTouch || isEffecting)
            return;
        //Debug.Log("X :" + _blockX + " Y :" + _blockY + " COLOR : " +color);
        isChecked = true;
        data.selectedBlock.Add(this);
        data.StartCheck();
    }

    public IEnumerator Vibration()
    {
        float currTime = 0.0f;
        while (currTime <= vibrationTime * 2)
        {
            currTime += Time.deltaTime * 2;

            // 진동 거는 부분 
            float x = Mathf.Sin(currTime * 40) / 10;
            transform.localPosition = new Vector2(x, transform.localPosition.y);
            yield return null;
        }

        transform.localPosition = new Vector2(0, transform.localPosition.y);
        yield break;

    }

    #region BlockTouch ( OLD ) 

    //void OnMouseDown()
    //{
    //    if (data.isSelected)
    //        canTouch = false;

    //    if (!canTouch)
    //        return; 


    //    data.isSelected = true;

    //    transform.GetChild(0).gameObject.SetActive(true);
    //    data.selectedColor = color;
    //    data.selectedBlock.Add(this);
    //    data.lastBlock = this;
    //}


    //void OnMouseEnter()
    //{
    //    if (!data.isSelected)
    //        canTouch = false;

    //    if (!canTouch)
    //        return;

    //    for (int i = 0; i < data.selectedBlock.Count; i++)
    //    {
    //        if (data.selectedBlock[i] == this)
    //            return;
    //    }

    //    if (data.selectedColor == color)
    //    {
    //        if (Mathf.Abs(this.transform.position.x - data.lastBlock.transform.position.x) < 1.4f &&
    //            Mathf.Abs(this.transform.position.y - data.lastBlock.transform.position.y) < 1.3f)
    //        {
    //            transform.GetChild(0).gameObject.SetActive(true);
    //            data.selectedBlock.Add(this);
    //            data.lastBlock = this;
    //            return;
    //        }
    //    }
    //}

    //void OnMouseUp()
    //{
    //    data.isSelected = false;

    //    if (!canTouch)
    //        return;

    //    if (data.selectedBlock.Count < 3)
    //    {
    //        character.Mistake();

    //        for (int i = 0; i < data.selectedBlock.Count; i++)
    //        {
    //            data.selectedBlock[i].transform.GetChild(0).gameObject.SetActive(false); 
    //        }
    //        data.selectedColor = BlockColor.None;
    //        data.lastBlock = null;
    //        data.selectedBlock.Clear();
    //        return;
    //    }
    //    // else

    //    character.Pang();
    //    for (int i = 0; i < data.selectedBlock.Count; i++)
    //    {
    //        data.AddPoint(2, color);
    //        data.AddScore(10);
    //        data.selectedBlock[i].transform.GetChild(0).gameObject.SetActive(false);
    //        data.selectedBlock[i].transform.GetChild(1).gameObject.SetActive(true);
    //        // data.selectedBlock[i].gameObject.SetActive(false);
    //    }

    //    data.lastBlock = null;
    //    data.selectedBlock.Clear();
    //    data.selectedColor = BlockColor.None;
    //}

    #endregion
}
