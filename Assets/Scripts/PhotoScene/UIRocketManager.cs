using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRocketManager : MonoBehaviour
{
    public RocketData[] rocketDatas;
    public Action<RocketData> OnRocketChanged;

    public void Awake()
    {
        for (int i = 0; i < rocketDatas.Length; i++)
        {
            rocketDatas[i].OnMyRocketData += OnRocketChanged;
        }
    }

    private void Start()
    {
        OnRocketChanged?.Invoke(rocketDatas[0]);
    }
}
