using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketData : MonoBehaviour
{
    [HideInInspector]
    public int SizePlayerImage = 256;
    [HideInInspector]
    public int x = 130;
    [HideInInspector]
    public int y = 520;
    public Image RocketImage;
    public RawImage PlayerImage;
    public Texture2D MaskTexture;
}
