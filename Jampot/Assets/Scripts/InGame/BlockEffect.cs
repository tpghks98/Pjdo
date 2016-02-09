using UnityEngine;
using System.Collections;

public class BlockEffect : MonoBehaviour
{
    void OnEnable()
    {
        transform.parent.GetComponent<NormalBlock>().isEffecting = true;
    }
    public void BlockAlphaDown()
    {
        transform.parent.GetComponent<SpriteRenderer>().color = Color.clear;
    }
    public void DestroyBlock()
    {

        transform.parent.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);

    }
}
