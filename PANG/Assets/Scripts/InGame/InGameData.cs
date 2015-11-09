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


public class InGameData : Singleton<InGameData>
{
    public bool isSelected
    {
        get;
        set;
    }

    public BlockColor selectedColor
    {
        get;
        set;
    }

    public List<Block> selectedBlock = new List<Block>();
    public NormalBlock lastBlock;


    public Sprite selected;
    void Awake()
    {

    }
}
