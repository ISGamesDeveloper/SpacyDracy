using System;
using UnityEngine;
using UnityEngine.UI;

public class CarLapCounter : MonoBehaviour
{

	public TrackLapTrigger first;
	public Text text;
	private RaceCarScript currentRaceCarScript;
	TrackLapTrigger next;
	public Action<string> onGameEnded;
	public static int MaxLapCount = 99;
	private int _currentLap;

	private void Awake()
	{
		currentRaceCarScript = gameObject.GetComponent<RaceCarScript>();
		currentRaceCarScript.CurrentCarLapCounter = this;
	}

	void Start()
	{
		_currentLap = 1;
		SetNextTrigger(first);
		UpdateText();

		Debug.Log("MaxLapCount: " + MaxLapCount);
	}

	void UpdateText()
	{
		Debug.Log("0000");
		//Debug.Log("MaxLapCount: " + MaxLapCount);
		//Debug.Log("_currentLap: " + _currentLap);

		if (_currentLap == MaxLapCount + 1)
		{
			onGameEnded?.Invoke(currentRaceCarScript.CarName);
		}

		if (text && _currentLap <= MaxLapCount)
		{
			if (!string.IsNullOrEmpty(currentRaceCarScript.CarName))
			{
				Debug.Log("1111");
				text.text = string.Format(currentRaceCarScript.CarName + ". Lap {0}", _currentLap);
			}
			else
			{
				text.text = string.Empty;
			}
		}
	}

	public void OnLapTrigger(TrackLapTrigger trigger)
	{
		if (trigger == next)
		{
			if (first == next)
			{
				_currentLap++;
				UpdateText();
			}
			SetNextTrigger(next);
		}
	}

	void SetNextTrigger(TrackLapTrigger trigger)
	{
		next = trigger.next;
		SendMessage("OnNextTrigger", next, SendMessageOptions.DontRequireReceiver);
	}
}
