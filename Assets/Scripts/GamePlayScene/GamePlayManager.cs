using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
	[HideInInspector] public List<Texture2D> cars = new List<Texture2D>();
	[HideInInspector] public Text[] PlayerNameText;
	public Text MaxLapText;
	public GameObject TextContainer;
	public RaceCarScript[] raceCarScript;
	public CameraManager cameraManager;
	private ApplicationMain applicationMain;
	public GameObject OpenFinishWindow;
	public GamePlayFinish GamePlayFinish;

	public void OnFinish(string playerName)
	{
		OpenFinishWindow.SetActive(true);
		GamePlayFinish.PlayerWon.text = playerName + " WON";

		Time.timeScale = 0;
	}

	private void Awake()
	{
		applicationMain = ApplicationMain.Instance;
		applicationMain.GamePlayManager = this;
	}

	void Start()
	{
		if (CarsIsEmpty())
			return;

		cars = ApplicationMain.Instance.cars;

		Debug.Log("cars count = " + cars.Count);

		DisableAllCars();

		PlayerNameText = new Text[cars.Count];
		MaxLapText.text = "Total laps: " + CarLapCounter.MaxLapCount;

		for (int i = 0; i < cars.Count; i++)
		{
			raceCarScript[i].gameObject.SetActive(true);
			Debug.Log("OnFinish");
			raceCarScript[i].CurrentCarLapCounter.onGameEnded += OnFinish;
			Debug.Log("OnFinish Added");
			//if (cars[i].width >= 208 && cars[i].height >= 128)
			//{
			//	var withCoeff = cars[i].width / 208;
			//	var heightCoeff = cars[i].height / 128;

			//	TextureScale.Bilinear(cars[i], cars[i].width / withCoeff, cars[i].height / heightCoeff);
			//}
			//else
			//{
			//	TextureScale.Bilinear(cars[i], 208, 128);
			//}

			raceCarScript[i].CarSpriteRenderer.sprite =
				Sprite.Create(cars[i], new Rect(0, 0, cars[i].width, cars[i].height), new Vector2(0.5f, 0.5f));

			raceCarScript[i].CarSpriteRenderer.sprite.name = cars[i].name + "_sprite";
			raceCarScript[i].CarSpriteRenderer.material.mainTexture = cars[i] as Texture;
			raceCarScript[i].CarSpriteRenderer.material.shader = Shader.Find("Sprites/Default");
			raceCarScript[i].gameObject.AddComponent<PolygonCollider2D>();
			raceCarScript[i].PlayerName = ApplicationMain.makePhotoButtonData[i].PlayerNumber.text;
			raceCarScript[i].cameraManager = cameraManager;

			PlayerNameText[i] = TextContainer.transform.GetChild(i).GetComponent<Text>();
			PlayerNameText[i].text = raceCarScript[i].PlayerName + ". Lap 1";
		}
	}

	private void DisableAllCars()
	{
		if (CarsIsEmpty())
			return;

		for (int i = 0; i < raceCarScript.Length; i++)
		{
			raceCarScript[i].gameObject.SetActive(false);
		}
	}

	private bool CarsIsEmpty()
	{
		return raceCarScript == null || raceCarScript.Length == 0 ? true : false;
	}
}
