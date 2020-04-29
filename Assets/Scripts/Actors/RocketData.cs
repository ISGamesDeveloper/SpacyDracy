using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketData : MonoBehaviour
{
    public float width;
    public float height;
    public float x;
    public float y;

    public Image RocketImage;
    public RawImage PlayerImage;
    public Texture2D MaskTexture;
    public Toggle rocketButton;

    public Action<RocketData> OnMyRocketData;

    private void Start()
    {
        rocketButton.onValueChanged.AddListener(SendMyData);

        x = PlayerImage.gameObject.GetComponent<RectTransform>().anchoredPosition.x;
        y = PlayerImage.gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        width = PlayerImage.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        height = PlayerImage.gameObject.GetComponent<RectTransform>().sizeDelta.y;
    }

    private void SendMyData(bool b)
    {
        OnMyRocketData?.Invoke(this);
    }
}
