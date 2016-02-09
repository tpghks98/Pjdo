using UnityEngine;
using System.Collections;

public abstract class Block : MonoBehaviour
{
    public int _blockX { get; protected set; }
    public int _blockY { get; set; }

    public bool isChecked { get; set; } // first false

    protected BlockGenerator _gene { get; set; }
    protected BlockType blockType { get; set; }

    protected readonly float moveTime = 0.2f;


    public abstract void InitWithGenerator(BlockGenerator gene, int blockX, int blockY, int rand);

    public abstract IEnumerator MoveProcess(float targetY, float moveT);

    public abstract IEnumerator Vibration();

    public abstract BlockType GetBlockColor();




}
