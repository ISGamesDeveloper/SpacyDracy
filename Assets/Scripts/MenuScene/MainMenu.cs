using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public float factorValue = 3.0f;

	public Button StartARaceButton;
	public Button ChoseATrackButton;
	public AudioSource mainAudioSource;
	public ApplicationMain ApplicationMain;
	public GameObject MainWindow;
	public GameObject SinglePlayerWindow;
	public GameObject SinglePlayerContent;
	public GameObject TrackMenuWindow;
	public GameObject SettingsMenuWindow;
	public MakePhotoButtonData takePhotoItem;
	public Transform addPlayerButton;
	public Color mainColor;

	private void Awake()
	{
		ApplicationMain = ApplicationMain.Instance;
		ApplicationMain.MainMenu = this;
	}

	private void Start()
	{
		EnableCurrentState(ApplicationMain.CurrentMenuState);
		ChoseATrackButton.onClick.AddListener(ChoseATrack);
		ChoseATrackButton.gameObject.SetActive(false);
		StartARaceButton.gameObject.SetActive(false);

		//if (ApplicationMain.Instance.makePhotoButtonData.Count == 0)
		//{
		//	AddPlayer(addPlayerButton);
		//}
		//else
		//{
		//	InitUITakePhotoButton();
		//}

		InitUITakePhotoButton();

		RecalculateAddPlayerButton();

		//NativeCamera.OpenSettings();
		NativeCamera.RequestPermission();
		Debug.Log("Permission: " + NativeCamera.CheckPermission());
	}

	public void AddPlayer()
	{
		var item = Instantiate(takePhotoItem, SinglePlayerContent.transform);
		var index = ApplicationMain.makePhotoButtonData.Count;

		item.Init(index, item);
		//Item.transform.SetSiblingIndex(addPlayerTransform.GetSiblingIndex());
		//Item.myIndex = ApplicationMain.Instance.makePhotoButtonData.Count;
		//Item.button.onClick.AddListener(Item.MakePhoto);
		item.PlayerText.text = LocalizationSettings.NewPlayer;
		item.TakePhotoText.text = LocalizationSettings.TakePhoto;

		ApplicationMain.makePhotoButtonData.Add(item);

		RecalculateAddPlayerButton();
	}

	public void RecalculateAddPlayerButton()
	{
		ChoseATrackButton.gameObject.SetActive(HasMultiplePlayer());
		addPlayerButton.gameObject.SetActive(ApplicationMain.makePhotoButtonData.Count < 3);
	}

	public void UpdatePlayerData()
	{
		Debug.Log("ApplicationMain.makePhotoButtonData.Count: " + ApplicationMain.makePhotoButtonData.Count);
		for (var i = 0; i < ApplicationMain.makePhotoButtonData.Count; i++)
		{
			ApplicationMain.makePhotoButtonData[i].myIndex = i;
		}

		RecalculateAddPlayerButton();
		RecalculateSubstanceNameCar();
		//RecalculateAdjectiveNameCar();
		RecalculateColorCar();
	}

	public void RecalculateSubstanceNameCar()
	{
		for (var i = 0; i < ApplicationMain.SubstanceNames.Count; i++)
		{
			if (ApplicationMain.SubstanceNames[i].Equals(ApplicationMain.CurrentSubstanceName))
			{
				ApplicationMain.SubstancesFree[i] = true;
				Debug.Log("TRUE NAME: " + ApplicationMain.CurrentSubstanceName);
			}
		}
	}

	//public void RecalculateAdjectiveNameCar()
	//{
	//	for (var i = 0; i < ApplicationMain.AdjectiveNames.Count; i++)
	//	{
	//		if (ApplicationMain.AdjectiveNames[i].Equals(ApplicationMain.CurrentAdjectiveName))
	//		{
	//			ApplicationMain.AdjectivesFree[i] = true;
	//			Debug.Log("TRUE Adjective NAME: " + ApplicationMain.CurrentAdjectiveName);
	//		}
	//	}
	//}

	public void RecalculateColorCar()
	{
		for (var i = 0; i < ApplicationMain.CarColors.Count; i++)
			if (ApplicationMain.CarColors[i].Equals(ApplicationMain.CurrentCarColor))
				ApplicationMain.CarColorsFree[i] = true;
	}

	public bool HasMultiplePlayer()
	{
		var countHasTexture = 0;

		foreach (var data in ApplicationMain.makePhotoButtonData)
		{
			if (data.hasTexture)
				countHasTexture++;
		}

		return countHasTexture > 1;
	}

	private void InitUITakePhotoButton()
	{
		Debug.Log("Count: " + ApplicationMain.makePhotoButtonData.Count);

		for (var i = 0; i < ApplicationMain.makePhotoButtonData.Count; i++)
		{
			ApplicationMain.makePhotoButtonData[i] = Instantiate(takePhotoItem, SinglePlayerContent.transform);
			var item = ApplicationMain.makePhotoButtonData[i];
			item.Init(i, item);

			if (ApplicationMain.RaceCars.Count-1 >= i)
			{
				item.RaceCar.CarTexture = ApplicationMain.RaceCars[i].CarTexture;
				item.RaceCar.CarColor = ApplicationMain.RaceCars[i].CarColor;
				item.RaceCar.SubstanceName = ApplicationMain.RaceCars[i].SubstanceName;
				item.RaceCar.PlayerName = ApplicationMain.RaceCars[i].PlayerName;
				item.RaceCar.CarColorName = ApplicationMain.RaceCars[i].CarColorName;
				item.RaceCar.AdjectiveName = ApplicationMain.RaceCars[i].AdjectiveName;

				ChangetextureScale(item.carUIImage.rectTransform.sizeDelta, ApplicationMain.RaceCars[i].CarTexture);
				item.carUIImage.rectTransform.sizeDelta = new Vector2(ApplicationMain.RaceCars[i].CarTexture.width, ApplicationMain.RaceCars[i].CarTexture.height);
				item.carUIImage.texture = ApplicationMain.RaceCars[i].CarTexture;
				item.carUIImage.color = ApplicationMain.RaceCars[i].CarColor;
				item.PlayerText.text = ApplicationMain.RaceCars[i].PlayerName;
				item.TakePhotoText.text = "";
				item.hasTexture = true;
			}
		}
	}

	private void ChangetextureScale(Vector2 WH, Texture2D texture)
	{
		//if (texture.width >= 208 && texture.height >= 128)
		Debug.Log("texture is null: " + texture == null);
		Debug.Log("WH: " + WH.x + "  " + WH.y);
		Debug.Log("texture w : " + texture.width);
		Debug.Log("texture h : " + texture.height);

		var withCoeff = texture.width / WH.x;
		var heightCoeff = texture.height / WH.y;
		var width = texture.height > texture.width ? texture.width / heightCoeff : texture.width / withCoeff;
		var height = texture.width > texture.height ? texture.height / withCoeff : texture.height / heightCoeff;

		//texture.width / withCoeff


		TextureScale.Bilinear(texture, (int)width, (int)height);

		Debug.Log("ITOG w : " + texture.width);
		Debug.Log("ITOG h : " + texture.height);
	}

	public void PlayGame()
	{
		if (!string.IsNullOrEmpty(ApplicationMain.CurrentTrackName) && ApplicationMain.RaceCars.Count > 0)
		{
			SceneManager.LoadScene(ApplicationMain.CurrentTrackName);
			ApplicationMain.CurrentMenuState = "MainMenuButton";
		}
	}

	public void MainMenuButton()
	{
		ApplicationMain.CurrentMenuState = "MainMenuButton";
		DisabledAllWindows();
		MainWindow.SetActive(true);
	}

	public void SinglePlayerButton()
	{
		//NativeCamera.TakePicture(path => {Debug.Log("path: " + path); }, preferredCamera: NativeCamera.PreferredCamera.Rear);
		ApplicationMain.CurrentMenuState = "SinglePlayerButton";
		DisabledAllWindows();
		SinglePlayerWindow.SetActive(true);
	}

	public void SettingsButton()
	{
		ApplicationMain.CurrentMenuState = "SettingsButton";
		DisabledAllWindows();
		SettingsMenuWindow.SetActive(true);
	}

	public void ChoseATrack()
	{
		ApplicationMain.CurrentMenuState = "CooseATrack";
		DisabledAllWindows();
		TrackMenuWindow.SetActive(true);
	}

	public void QuitGame()
	{
		ApplicationMain.CurrentMenuState = "QuitGame";
		Debug.Log("Quit!");
		Application.Quit();
	}

	private void DisabledAllWindows()
	{
		MainWindow.SetActive(false);
		SinglePlayerWindow.SetActive(false);
		TrackMenuWindow.SetActive(false);
		SettingsMenuWindow.SetActive(false);
	}

	public void EnableCurrentState(string state)
	{
		switch (state)
		{
			case "MainMenuButton":
				MainMenuButton();
				break;
			case "SinglePlayerButton":
				SinglePlayerButton();
				break;
			case "SettingsButton":
				SettingsButton();
				break;
			case "CooseATrack":
				ChoseATrack();
				break;
			case "QuitGame":
				QuitGame();
				break;
		}
	}


}
