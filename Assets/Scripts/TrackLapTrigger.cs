using System;
using UnityEngine;
using System.Collections;

public class TrackLapTrigger : MonoBehaviour
{
	public TrackLapTrigger next;
	private CarLapCounter carLapCounter;

	public int CurrentRank;

	private void Awake()
	{
		CurrentRank = transform.GetSiblingIndex();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		carLapCounter = other.gameObject.GetComponent<CarLapCounter>();

		if (carLapCounter) {
			carLapCounter.OnLapTrigger(this);
		}

		var rank = CurrentRank + (carLapCounter.CurrentLap * 100);

		carLapCounter.currentRaceCarScript.SubstanceRank = rank;
		//Debug.Log(carLapCounter.currentRaceCarScript.PlayerName + " rank: " + rank);
		ApplicationMain.Instance.CameraManager.CheckCurrentRankin();
	}
}
