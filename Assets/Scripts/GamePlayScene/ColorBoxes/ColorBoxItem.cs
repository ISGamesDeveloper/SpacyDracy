using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBoxItem : MonoBehaviour
{
    //[HideInInspector]
    public Image ColorBox;
    //[HideInInspector]
    public RawImage PlayerFace;
    //[HideInInspector]
    public Button button;

    public Action<int> OnSendID;
    public int MyRcsID;

    public void Init()
    {
        ColorBox = gameObject.GetComponentInChildren<Image>();
        PlayerFace = gameObject.GetComponent<RawImage>();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(SendRcsID);
    }

    private void SendRcsID()
    {
        Debug.Log(gameObject.name + "   MyRcsID: " + MyRcsID);

        OnSendID?.Invoke(MyRcsID);
    }

}
