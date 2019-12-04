using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
	[HideInInspector] public Transform Target;

	public Button ChangeCameraButton;
	public Camera MainCamera;
	public RaceCarScript[] raceCarScripts;

	private int currentCameraNumber;
	private ApplicationMain applicationMain; //сдеалть несколько камер (основная и переключаемая)
	private Text[] playerTextUI;
	private bool update;

	private readonly Vector3 MainCameraDefaultPosition = new Vector3(26, 0, -7);
	private float orthographicSize = 7;
	private float defaultOrthographicSize = 25;

	private void Awake()
	{
		applicationMain = ApplicationMain.Instance;
		applicationMain.CameraManager = this;
	}

	void Start()
	{
		playerTextUI = applicationMain.GamePlayManager.PlayerNameText;
		ChangeCameraButton.onClick.AddListener(ChangeStateCameras);

		raceCarScripts = new RaceCarScript[applicationMain.cars.Count];

		for (var i = 0; i < applicationMain.cars.Count; i++)
		{
			raceCarScripts[i] = applicationMain.GamePlayManager.raceCarScript[i];

			ChangeColorBackgroundCar(raceCarScripts[i].CarColor,playerTextUI[i],raceCarScripts[i]);
		}
	}

	public void ChangeStateCameras()
	{
		currentCameraNumber++;
		
		if (currentCameraNumber == applicationMain.cars.Count+1)
		{
			currentCameraNumber = 0;
			MainCamera.orthographicSize = defaultOrthographicSize;
			MainCamera.transform.localPosition = MainCameraDefaultPosition;

			update = false;
		}
		else
		{
			update = true;
		}

		Debug.Log("currentCameraNumber: " + currentCameraNumber);
	}

	private void ChangeColorBackgroundCar(Color color, Text textPlayerUi, RaceCarScript raceCarScript)
	{
		if (raceCarScripts == null)
			return;

		var main = raceCarScript.Fire.main;
		main.startColor = color;
		textPlayerUi.color = color;
	}

	private void Update()
	{
		if(!update)
			return;

		if (raceCarScripts.Length == 0 || currentCameraNumber == 0)
			return;

		var transform = raceCarScripts[currentCameraNumber-1].transform.position;
		MainCamera.orthographicSize = orthographicSize;
		MainCamera.transform.localPosition = new Vector3(transform.x, transform.y, -7);
	}
}
