using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBoxContainer : MonoBehaviour
{
    public ColorBoxItem[] ColorBoxItems;

    public Button Auto;
    public Image AutoButtonArc;
    public Action<int> OnChangeStateCamera;
    public Action OnAutoCamera;

    private void Awake()
    {
        ColorBoxItems = gameObject.GetComponentsInChildren<ColorBoxItem>();
        Auto.onClick.AddListener(SetAutoChangeStateCamera);

        for (int i = 0; i < ColorBoxItems.Length; i++)
        {
            ColorBoxItems[i].Init();
            ColorBoxItems[i].gameObject.SetActive(false);
            ColorBoxItems[i].OnSendID += SendID;
        }

        DisableArc();
    }

    private void SendID(int id)
    {
        AutoButtonArc.enabled = false;
        OnChangeStateCamera?.Invoke(id);
    }

    private void SetAutoChangeStateCamera()
    {
        AutoButtonArc.enabled = true;
        OnAutoCamera?.Invoke();
    }

    public void SetColorBox(Texture2D faceTexture, Color colorBox, int index, int RcsID, bool focused)
    {
        ColorBoxItems[index].ColorBox.color = colorBox;
        ColorBoxItems[index].PlayerFace.texture = faceTexture;
        ColorBoxItems[index].MyRcsID = RcsID;

        if(focused)
        {
            EnableArc(index);
        }
    }

    public void EnableArc(int index)
    {
        if (ColorBoxItems[index].ColorBox.enabled == true)
            return;

        DisableArc();

        ColorBoxItems[index].ColorBox.enabled = true;
    }

    public void DisableArc()
    {
        for (int i = 0; i < ColorBoxItems.Length; i++)
        {
            ColorBoxItems[i].ColorBox.enabled = false;
        }
    }

    public void EnableColorBox(int index)
    {
        ColorBoxItems[index].gameObject.SetActive(true);
    }


}
