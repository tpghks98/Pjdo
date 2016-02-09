using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


/// <summary>
///  UI에서 이미지 숫자폰트 사용할떄 쓰는 스크립트 v 0.1
///  1. 얘는 Canvas에서 RectTransfom으로 부모가 들고있는다.
///  2. 부모를 중심으로 시작점을 정한다.
///  3. 숫자 이미지의 크기는 모두 같아야함
///  4. 이미지 스크립트는 자동으로 생성
///  5. 지금은 오른쪽에서부터 시작하는것만댐 
///  
/// use
/// - 스크립트에서 PrintNumber 호출하면끗
/// 
/// TODO : 지금 걍 첫번째 이미지의 가로를 가지고 간격을 조정 하는데
///        이제 생성될때 자동으로 거리 계산해서 출력하게 해야됨 
/// </summary>
public class ShowNumber : MonoBehaviour
{
    public enum Sort { Left, Right };
    public Sort sort;
    // 0 ~ 9
    public Sprite[] numbers = new Sprite[10];   // 번호 들 0~9
    // ,
    public Sprite comma;                        // 컴마
    public bool isComma;                        // 컴마 유무

    private List<Image> imgList = new List<Image>();    // 이미지리스트 ( 이미지 갯수 )
    private float numberWidth;                          // 숫자 가로사이즈
    private float commaWidth;                           // 콤마 가로사이즈
    private int positionalNumver = 0;                   // 숫자 자리수


    public void LoadNumberResources(string path)
    {
        for (int n = 0; n < 10; ++n)
        {
            numbers[n] = Resources.Load(path + n, typeof(Sprite)) as Sprite;
        }

        if(isComma)
            comma = Resources.Load(path + ",", typeof(Sprite)) as Sprite;

        Initialize();

    }

    void Initialize()
    {
        if (numbers[0] == null)
        {
            return;
        }
        
        numberWidth = numbers[0].bounds.size.x * 100;

        if(isComma)
            commaWidth = comma.bounds.size.x * 100;

        AddImage();
    }




    public void PrintNumber(int num)
    {
        if (num < 0)
        {
            Print(0);
            return;
        }
        Print(num);
    }

    void Print(int num)
    {
        positionalNumver = 1;

        for (int i = 0; i < imgList.Count; i++)
        {
            imgList[i].enabled = false;
        }
        while (num >= 0)
        {
            // 이미지 숫자 갯수가 자릿수보다 적을때
            if (imgList.Count <= positionalNumver)
            {
                // 이미지 증가
                if (positionalNumver % 4 == 0 && positionalNumver != 0 && isComma)
                    AddImage();
                else
                    AddImage();
            }

            // 컴마위치 표시할때
            if (isComma)
            {
                if (positionalNumver % 4 == 0 && positionalNumver != 0)
                {
                    ImgSet(imgList[positionalNumver - 1], comma, true);
                    positionalNumver++;
                }
            }

            // 실질적인 숫자 이미지출력 

            ImgSet(imgList[positionalNumver - 1], numbers[num % 10], true);
            positionalNumver++;

            if (num / 10 == 0)
                break;

            num /= 10;
        }

    }

    void ImgSet(Image img, Sprite spr, bool enbled)
    {
        img.sprite = spr;
        img.SetNativeSize();
        img.enabled = enabled;
    }
    void AddImage()
    {
        GameObject temp;
        temp = new GameObject("number");
        temp.AddComponent(typeof(Image));
        temp.transform.SetParent(transform, false);

        temp.transform.localPosition = new Vector2(-imgList.Count * numberWidth, 0);

        temp.GetComponent<Image>().enabled = false;

        imgList.Add(temp.GetComponent<Image>());
    }


}
