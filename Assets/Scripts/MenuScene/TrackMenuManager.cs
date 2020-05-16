using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackMenuManager : MonoBehaviour
{
	public TrackItemUI[] trackItems;
	public Button Plus, Minus;
	public Text lapCountText;
	public int LapCounterText = 3;//
	public const int MAXLAP = ApplicationMain.MaxLapCount;

	public void Start()
	{
		Plus.onClick.AddListener(LapPlus);
		Minus.onClick.AddListener(LapMinus);

		foreach (var t in trackItems)
			t.trackMenuManager = this;

		DisableAllOutlines();
		InitializeLapCount();
	}

	private void InitializeLapCount()
	{
		lapCountText.text = LapCounterText.ToString();
		CarLapCounter.MaxLapCount = LapCounterText;
	}

	public void DisableAllOutlines()
	{
		foreach (var t in trackItems)
			t.Outline.enabled = false;
	}

	public void EnableItemOutline(TrackItemUI trackItem)
	{
		trackItem.Outline.enabled = true;
	}

	private void LapPlus()
	{
		if (LapCounterText < MAXLAP)
		{
			LapCounterText++;
		}

		lapCountText.text = LapCounterText.ToString();
		CarLapCounter.MaxLapCount = LapCounterText;
	}

	private void LapMinus()
	{
		if (LapCounterText > 1)
		{
			LapCounterText--;
		}
		Debug.Log("LapCounterText: " + LapCounterText);
		lapCountText.text = LapCounterText.ToString();
		CarLapCounter.MaxLapCount = LapCounterText;
	}
}
