using UnityEngine;
using System.Collections;

public class NormalBlock : Block
{
    private Sprite[] colors;

    private bool once;
    public bool isEffecting { get; set; }
    private bool canTouch;


    [SerializeField]
    private float vibrationTime = 0.2f;

    void Awake()
    {
        colors = new Sprite[5];
        for (int i = 0; i < 5; i++)
        {
            colors[i] = Resources.Load<Sprite>("InGame/Sprite/NormalBlock/" + "Block_" + (i + 1).ToString());
        }


    }

    public override void InitWithGenerator(BlockGenerator gene, int blockX, int blockY, int rand)
    {
        _blockX = blockX;
        _blockY = blockY;

        once        = true;
        _gene       = gene;
        isEffecting = false;
        canTouch    = false;

        transform.position = gene.transform.position;
        gameObject.SetActive(true);

        blockType = (BlockType)rand;
        ChangeColor(blockType);
    }

    public override IEnumerator MoveProcess(float targetY, float moveT)
    {
        canTouch = false;
        float currTime = 0.0f;
        float saveY = _blockY;

        while (currTime < moveT)
        {
            if (saveY != _blockY)
            {
                once = true;
                yield break;
            }

            currTime += Time.deltaTime;
            float y = EasingUtil.easeOutCubic(transform.localPosition.y, targetY, currTime / moveT);
            transform.localPosition = new Vector2(transform.localPosition.x, y);
            yield return null;
        }

        transform.localPosition = new Vector2(transform.localPosition.x, targetY);


        once = true;
        canTouch = true;
        yield break;
    }

    public override BlockType GetBlockColor()
    {
        return blockType;
    }

    void Update()
    {

        if (GameLogic.Instance.linePosY[_blockY] != transform.localPosition.y)
        {
            if (once)
            {
                once = false;
                if (transform.localPosition.y == 0)
                    StartCoroutine(MoveProcess(GameLogic.Instance.linePosY[_blockY], moveTime + 0.2f));
                else
                    StartCoroutine(MoveProcess(GameLogic.Instance.linePosY[_blockY], moveTime));
            }
        }
        else if(!GameLogic.Instance.isStart || GameLogic.Instance.isPause)
            canTouch = false;
        else
            canTouch = true;
    }


    void ChangeColor(BlockType type)
    {
        GetComponent<SpriteRenderer>().sprite = colors[(int)type - 1];
    }


    public override IEnumerator Vibration()
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


    void OnMouseDown()
    {
        if (!canTouch || isEffecting)
            return;

        isChecked = true;
        GameLogic.Instance.selectedBlock.Add(this);
        GameLogic.Instance.StartCheck();
    }

    void OnDisable()
    {
        _gene.DownBlock(_blockY);
        _gene.CurrBlockCount--;
    }
}
