using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBoxContainer : MonoBehaviour
{ 
    //[HideInInspector]
    public Image[] ColorBox;
    //[HideInInspector]
    public RawImage[] PlayerFace;

    private void Awake()
    {
        ColorBox = new Image[transform.childCount];
        PlayerFace = new RawImage[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            ColorBox[i] = transform.GetChild(i).GetComponent<Image>();
            PlayerFace[i] = transform.GetChild(i).transform.GetChild(0).GetComponent<RawImage>();
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void InitColorBox(Texture2D faceTexture, Color colorBox, int index)
    {
        ColorBox[index].color = colorBox;
        PlayerFace[index].texture = faceTexture;

        transform.GetChild(index).gameObject.SetActive(true);
    }

}
