using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
	[HideInInspector] public Transform Target;

	public Button ChangeCameraButton;
	public Camera MainCamera;
	public RaceCarScript[] raceCarScripts;
	public GamePlayManager GamePlayManager;
	public Transform[] ColorBoxContainer;

	private int currentCameraNumber;
	private ApplicationMain applicationMain; //сдеалть несколько камер (основная и переключаемая)
	//private Text[] playerTextUI;
	private bool autoUpdateCamera;
	private int currentIndexForCamera;
	private const int CameraHeight = -5;
	private readonly Vector3 MainCameraDefaultPosition = new Vector3(26, 0, CameraHeight);
	private float orthographicSize = 5f;
	private float defaultOrthographicSize = 25;

	private void Awake()
	{
		applicationMain = ApplicationMain.Instance;
		applicationMain.CameraManager = this;
	}

	void Start()
	{
		//playerTextUI = applicationMain.GamePlayManager.PlayerNameText;
		ChangeCameraButton.onClick.AddListener(ChangeStateCameras);

		raceCarScripts = new RaceCarScript[ApplicationMain.RaceCars.Count];
		Debug.Log("ApplicationMain.RaceCars: " + ApplicationMain.RaceCars.Count);
		for (var i = 0; i < raceCarScripts.Length; i++)
		{
			Debug.Log("i: " + i);
			raceCarScripts[i] = GamePlayManager.RaceCars[i];

			ChangeColorBackgroundCar(raceCarScripts[i].CarColor, /*playerTextUI[i], */raceCarScripts[i]);
		}

		MainCamera.orthographicSize = orthographicSize;
		
		autoUpdateCamera = true;

		if (autoUpdateCamera == false)
		{
			currentRaceCarTransform = raceCarScripts[0].transform;
		}
		else
		{
			ChangeStateCameras();
		}
	}

	public void CheckCurrentRankin()
	{


		if (raceCarScripts.Length > 2)
		{

			if (raceCarScripts[0].SubstanceRank > raceCarScripts[1].SubstanceRank
				&& raceCarScripts[0].SubstanceRank > raceCarScripts[2].SubstanceRank)
			{
				currentIndexForCamera = 0;
				ColorBoxContainer[0].SetSiblingIndex(0);

				if (raceCarScripts[1].SubstanceRank > raceCarScripts[2].SubstanceRank)
				{
					ColorBoxContainer[1].SetSiblingIndex(1);
					ColorBoxContainer[2].SetSiblingIndex(2);
				}
				else if (raceCarScripts[1].SubstanceRank < raceCarScripts[2].SubstanceRank)
				{
					ColorBoxContainer[2].SetSiblingIndex(1);
					ColorBoxContainer[1].SetSiblingIndex(2);
				}
			}

			else if (raceCarScripts[1].SubstanceRank > raceCarScripts[0].SubstanceRank
				&& raceCarScripts[1].SubstanceRank > raceCarScripts[2].SubstanceRank)
			{
				currentIndexForCamera = 1;
				ColorBoxContainer[1].SetSiblingIndex(0);

				if (raceCarScripts[0].SubstanceRank > raceCarScripts[2].SubstanceRank)
				{
					ColorBoxContainer[0].SetSiblingIndex(1);
					ColorBoxContainer[2].SetSiblingIndex(2);
				}
				else if (raceCarScripts[0].SubstanceRank < raceCarScripts[2].SubstanceRank)
				{
					ColorBoxContainer[2].SetSiblingIndex(1);
					ColorBoxContainer[0].SetSiblingIndex(2);
				}
			}

			else if (raceCarScripts[2].SubstanceRank > raceCarScripts[0].SubstanceRank
				&& raceCarScripts[2].SubstanceRank > raceCarScripts[1].SubstanceRank)
			{
				currentIndexForCamera = 2;
				ColorBoxContainer[2].SetSiblingIndex(0);

				if (raceCarScripts[0].SubstanceRank > raceCarScripts[1].SubstanceRank)
				{
					ColorBoxContainer[0].SetSiblingIndex(1);
					ColorBoxContainer[1].SetSiblingIndex(2);
				}
				else if (raceCarScripts[0].SubstanceRank < raceCarScripts[1].SubstanceRank)
				{
					ColorBoxContainer[1].SetSiblingIndex(1);
					ColorBoxContainer[0].SetSiblingIndex(2);
				}
			}
		}
		else
		{
			//000112121
			//111002020
			//222001010
			//001010
			if (raceCarScripts[0].SubstanceRank > raceCarScripts[1].SubstanceRank)
			{
				currentIndexForCamera = 0;
				ColorBoxContainer[0].SetSiblingIndex(0);
				ColorBoxContainer[1].SetSiblingIndex(1);
			}
			else if (raceCarScripts[0].SubstanceRank < raceCarScripts[1].SubstanceRank)
			{
				currentIndexForCamera = 1;
				ColorBoxContainer[1].SetSiblingIndex(0);
				ColorBoxContainer[0].SetSiblingIndex(1);
			}
		}

		var color = Color.clear;

		for (int i = 0; i < ColorBoxContainer.Length; i++)
		{
			ColorBoxContainer[i].GetComponent<Outline>().effectColor = color;
		}

		if (autoUpdateCamera)
		{
			color = Color.black;
			ColorBoxContainer[currentIndexForCamera].GetComponent<Outline>().effectColor = color;
			currentRaceCarTransform = raceCarScripts[currentIndexForCamera].transform;
		}

	}

	public void ChangeStateCameras()
	{
		currentCameraNumber++;

		for (int i = 0; i < ColorBoxContainer.Length; i++)
		{
			var color = ColorBoxContainer[i].GetComponent<Outline>().effectColor;
			color.a = 0;
		}

		if (currentCameraNumber == ApplicationMain.RaceCars.Count + 1)
		{
			currentCameraNumber = 0;
			autoUpdateCamera = true;
		}
		else
		{
			var color = ColorBoxContainer[currentCameraNumber - 1].GetComponent<Outline>().effectColor;
			color.a = 1;

			currentRaceCarTransform = raceCarScripts[currentCameraNumber - 1].transform;
			autoUpdateCamera = false;
		}
	}

	private Transform currentRaceCarTransform;

	private void Update()
	{
		if (raceCarScripts.Length == 0/* || currentCameraNumber == 0*/)
			return;

		var position = currentRaceCarTransform.position;
		MainCamera.transform.localPosition = new Vector3(position.x, position.y, CameraHeight);
	}

	private void ChangeColorBackgroundCar(Color color, /*Text textPlayerUi, */RaceCarScript raceCarScript)
	{
		if (raceCarScripts == null)
			return;

		var main = raceCarScript.Fire.main;
		main.startColor = color;
		//textPlayerUi.color = color;
	}
}
