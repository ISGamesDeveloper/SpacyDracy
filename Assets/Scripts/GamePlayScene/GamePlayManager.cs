using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
	public List<RaceCarScript> RaceCars = new List<RaceCarScript>();
	//[HideInInspector] public Text[] PlayerNameText;
	public Text MaxLapText;
	//public GameObject TextContainer;
	public GameObject ColorBoxContainer;
	public CameraManager cameraManager;
	private ApplicationMain applicationMain;
	public GameObject OpenFinishWindow;
	public GamePlayFinish GamePlayFinish;
	public List<string> NameFinishRockets;
	public Dictionary<string, bool> AllNameRockets = new Dictionary<string, bool>();

	public void SetAllNameRockets(string carName)
	{

	}

	public void OnFinish(string playerName)
	{
		AllNameRockets[playerName] = true;
		Debug.Log("add player name: " + playerName);
		NameFinishRockets.Add(playerName);
		Debug.Log(NameFinishRockets.Count + "  " + ApplicationMain.RaceCars.Count);

		if (NameFinishRockets.Count == ApplicationMain.RaceCars.Count - 1)
		{
			for (int i = 0; i < NameFinishRockets.Count; i++)
			{
				GamePlayFinish.PlayersWon[i].gameObject.SetActive(true);
				GamePlayFinish.PlayersWon[i].text = NameFinishRockets[i] + " " + (i + 1);
			}

			var myKey = AllNameRockets.FirstOrDefault(x => x.Value == false).Key;
			GamePlayFinish.PlayersWon[NameFinishRockets.Count].gameObject.SetActive(true);
			GamePlayFinish.PlayersWon[NameFinishRockets.Count].text = myKey + " " + (NameFinishRockets.Count + 1);

			OpenFinishWindow.SetActive(true);

			Time.timeScale = 0;
		}
	}

	private void Awake()
	{
		applicationMain = ApplicationMain.Instance;
		applicationMain.GamePlayManager = this;
		cameraManager.GamePlayManager = this;

		cameraManager.ColorBoxContainer = new Transform[ColorBoxContainer.transform.childCount];

		for (int i = 0; i < ColorBoxContainer.transform.childCount; i++)
		{
			cameraManager.ColorBoxContainer[i] = ColorBoxContainer.transform.GetChild(i);
		}
	}

	void Start()
	{
		if (CarsIsEmpty())
			return;

		var MainRaceCars = ApplicationMain.RaceCars;

		DisableAllCars();

		//PlayerNameText = new Text[RaceCars.Count];
		MaxLapText.text = "Total laps: " + CarLapCounter.MaxLapCount;

		for (int i = 0; i < ColorBoxContainer.transform.childCount; i++)
		{
			ColorBoxContainer.transform.GetChild(i).gameObject.SetActive(false);
		}

		for (int i = 0; i < MainRaceCars.Count; i++)
		{
			RaceCars[i].gameObject.SetActive(true);
			RaceCars[i].CurrentCarLapCounter.onGameEnded += OnFinish;

			//if (MainRaceCars[i].CarTexture.width >= 208 && MainRaceCars[i].CarTexture.height >= 128)
			//{
				//var withCoeff = MainRaceCars[i].CarTexture.width / 208;
				//var heightCoeff = MainRaceCars[i].CarTexture.height / 128;

				//TextureScale.Bilinear(MainRaceCars[i].CarTexture, MainRaceCars[i].CarTexture.width / withCoeff, MainRaceCars[i].CarTexture.height / heightCoeff);
				var texture = MainRaceCars[i].CarTexture;
				Debug.Log("texture width: " + texture.width + "  texture heigth: " + texture.height);
				var withCoeff = texture.width / 208.0f;
				var heightCoeff = texture.height / 208.0f;
				Debug.Log("withCoeff: " + withCoeff + "  heightCoeff: " + heightCoeff);
				var width = texture.height > texture.width ? texture.width / heightCoeff : texture.width / withCoeff;
				var height = texture.width > texture.height ? texture.height / withCoeff : texture.height / heightCoeff;
				Debug.Log("width: " + width + "  height: " + height);
				TextureScale.Bilinear(texture, (int)width, (int)height);
				Debug.Log("ITOG texture width: " + texture.width + "  ITOG texture heigth: " + texture.height);
			//}
			//else
			//{
			//	TextureScale.Bilinear(MainRaceCars[i].CarTexture, 208, 128);
			//}

			RaceCars[i].CarSpriteRenderer.sprite =
				Sprite.Create(MainRaceCars[i].CarTexture, new Rect(0, 0, MainRaceCars[i].CarTexture.width, MainRaceCars[i].CarTexture.height), new Vector2(0.5f, 0.5f));

			RaceCars[i].CarSpriteRenderer.sprite.name = MainRaceCars[i].PlayerName + "_sprite";
			RaceCars[i].CarSpriteRenderer.color = MainRaceCars[i].CarColor;
			RaceCars[i].CarSpriteRenderer.material.mainTexture = MainRaceCars[i].CarTexture;
			RaceCars[i].CarSpriteRenderer.material.shader = Shader.Find("Sprites/Outline");
			RaceCars[i].SubstanceName = MainRaceCars[i].SubstanceName/*ApplicationMain.makePhotoButtonData[i].PlayerNumber.text*/;
			RaceCars[i].CarColorName = MainRaceCars[i].CarColorName;
			RaceCars[i].PlayerName = MainRaceCars[i].PlayerName;
			RaceCars[i].CarTexture = MainRaceCars[i].CarTexture;
			RaceCars[i].CarColor = MainRaceCars[i].CarColor;
			RaceCars[i].cameraManager = cameraManager;
			RaceCars[i].gameObject.AddComponent<BoxCollider2D>();
			//PlayerNameText[i] = TextContainer.transform.GetChild(i).GetComponent<Text>();
			//PlayerNameText[i].text = RaceCars[i].PlayerName + ". Lap 1";

			ColorBoxContainer.transform.GetChild(i).gameObject.SetActive(true);
			ColorBoxContainer.transform.GetChild(i).GetComponent<Image>().color = RaceCars[i].CarColor;

			AllNameRockets.Add(RaceCars[i].PlayerName, false);
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
