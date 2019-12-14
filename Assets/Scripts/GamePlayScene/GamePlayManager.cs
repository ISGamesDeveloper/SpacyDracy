using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
	public List<RaceCarScript> RaceCars = new List<RaceCarScript>();
	[HideInInspector] public Text[] PlayerNameText;
	public Text MaxLapText;
	public GameObject TextContainer;
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
		cameraManager.GamePlayManager = this;
	}

	void Start()
	{
		if (CarsIsEmpty())
			return;
		Debug.Log("ApplicationMain.RaceCars.Count: " + ApplicationMain.RaceCars.Count);
		var MainRaceCars = ApplicationMain.RaceCars;

		Debug.Log("cars count = " + RaceCars.Count);

		DisableAllCars();

		PlayerNameText = new Text[RaceCars.Count];
		MaxLapText.text = "Total laps: " + CarLapCounter.MaxLapCount;

		for (int i = 0; i < MainRaceCars.Count; i++)
		{
			RaceCars[i].gameObject.SetActive(true);
			Debug.Log("OnFinish");
			RaceCars[i].CurrentCarLapCounter.onGameEnded += OnFinish;
			Debug.Log("OnFinish Added");
			if (MainRaceCars[i].CarTexture.width >= 208 && MainRaceCars[i].CarTexture.height >= 128)
			{
				var withCoeff = MainRaceCars[i].CarTexture.width / 208;
				var heightCoeff = MainRaceCars[i].CarTexture.height / 128;

				TextureScale.Bilinear(MainRaceCars[i].CarTexture, MainRaceCars[i].CarTexture.width / withCoeff, MainRaceCars[i].CarTexture.height / heightCoeff);
			}
			else
			{
				TextureScale.Bilinear(MainRaceCars[i].CarTexture, 208, 128);
			}

			RaceCars[i].CarSpriteRenderer.sprite =
				Sprite.Create(MainRaceCars[i].CarTexture, new Rect(0, 0, MainRaceCars[i].CarTexture.width, MainRaceCars[i].CarTexture.height), new Vector2(0.5f, 0.5f));

			RaceCars[i].CarSpriteRenderer.sprite.name = MainRaceCars[i].CarName + "_sprite";
			RaceCars[i].CarSpriteRenderer.color = MainRaceCars[i].CarColor;
			RaceCars[i].CarSpriteRenderer.material.mainTexture = MainRaceCars[i].CarTexture;
			RaceCars[i].CarSpriteRenderer.material.shader = Shader.Find("Sprites/Default");
			RaceCars[i].gameObject.AddComponent<PolygonCollider2D>();
			RaceCars[i].CarName = MainRaceCars[i].CarName/*ApplicationMain.makePhotoButtonData[i].PlayerNumber.text*/;

			RaceCars[i].cameraManager = cameraManager;

			RaceCars[i].CarTexture = MainRaceCars[i].CarTexture;
			RaceCars[i].CarColor = MainRaceCars[i].CarColor;

			PlayerNameText[i] = TextContainer.transform.GetChild(i).GetComponent<Text>();
			PlayerNameText[i].text = RaceCars[i].CarName + ". Lap 1";
		}
	}

	private void DisableAllCars()
	{
		if (CarsIsEmpty())
			return;

		for (int i = 0; i < RaceCars.Count; i++)
		{
			RaceCars[i].gameObject.SetActive(false);
		}
	}

	private bool CarsIsEmpty()
	{
		return RaceCars == null || RaceCars.Count == 0 ? true : false;
	}
}
