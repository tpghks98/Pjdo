using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BlockColor
{
    None = 0,
    Red,
    Yellow,
    Green,
    Blue,
    Purple
};

public class GameCore : Singleton<GameCore> 
{
    public int[,] board = new int[7,6];


    [HideInInspector] public bool isSelected;  // 눌린지 안눌린지 판단
    [HideInInspector] public BlockColor selectedColor;  // 같은색깔만 해야되니까
    [HideInInspector] public List<NormalBlock> selectedBlock = new List<NormalBlock>(); // 다모으고 터뜨리기위해
    [HideInInspector] public NormalBlock lastBlock;
    Vector2 touchPos;
    RaycastHit2D hitInfo;

    void Awake()
    {
        isSelected = false;
        selectedColor = BlockColor.None;
    }

    void Update()
    {

    }



    public void AddRow(BlockColor blockColor, int generatorNumber,int currBlockIdx)
    {
        // 2가지 방법이있다 1. 왼쪽아래가 0,0 인방법 2. 왼쪽위가 0,0 인 방법 
        // 지금은 2번을 씀
        board[6-currBlockIdx, generatorNumber] = (int)blockColor;
    }
}

