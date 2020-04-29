using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarLapCounter : MonoBehaviour
{

	public TrackLapTrigger first;
	public Text text;
	public RaceCarScript currentRaceCarScript;
	TrackLapTrigger next;
	public Action<string> onGameEnded;
	public static int MaxLapCount = 99;
	public int CurrentLap;
	public AICarMovement currentAiCarMovement;
	
	private void Awake()
	{
		currentRaceCarScript = gameObject.GetComponent<RaceCarScript>();
		currentAiCarMovement = gameObject.GetComponent<AICarMovement>();
		currentRaceCarScript.CurrentCarLapCounter = this;
	}

	void Start()
	{
		CurrentLap = 1;
		SetNextTrigger(first);
		UpdateText();
	}

	void UpdateText()
	{
		//Debug.Log("MaxLapCount: " + MaxLapCount);
		//Debug.Log("_currentLap: " + _currentLap);

		if (CurrentLap == MaxLapCount + 1)
		{
			onGameEnded?.Invoke(currentRaceCarScript.PlayerName);
			currentAiCarMovement.Finish = true;
		}

		if (text && CurrentLap <= MaxLapCount)
		{
			if (!string.IsNullOrEmpty(currentRaceCarScript.PlayerName))
			{
				text.text = string.Format(currentRaceCarScript.PlayerName + ". Lap {0}", CurrentLap);
			}
			else
			{
				text.text = string.Empty;
			}
		}
	}

	public void OnLapTrigger(TrackLapTrigger trigger)///////
	{
		if (trigger == next)
		{
			if (first == next)
			{
				CurrentLap++;
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

	//public void OnTriggerEnter2D(Collider2D col)
	//{
	//	if (!col.gameObject.name.Equals("wall collision"))
	//		return;

	//	if (disableAicCoroutine == null && currentAiCarMovement.velocity > 11)
	//	{
	//		disableAicCoroutine = StartCoroutine(DisableAI());
	//	}
	//}

	//private Coroutine disableAicCoroutine;

	//private IEnumerator DisableAI()
	//{
	//	Debug.Log("DisableAI");
	//	currentAiCarMovement.enabled = false;
	//	yield return new WaitForSeconds(1);
	//	currentAiCarMovement.enabled = true;
	//	disableAicCoroutine = null;
	//}
}
