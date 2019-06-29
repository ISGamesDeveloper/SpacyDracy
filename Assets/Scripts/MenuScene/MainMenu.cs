using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public float factorValue = 3.0f;

	public Button StartARaceButton;
	public AudioSource mainAudioSource;
	public ApplicationMain ApplicationMain;
	public GameObject MainWindow;
	public GameObject SinglePlayerWindow;
	public GameObject SinglePlayerContent;
	public GameObject MultiPlayerWindow;
	public GameObject TrackMenuWindow;
	public GameObject SettingsMenuWindow;
	public MakePhotoButtonData takePhotoItem;
	public Transform addPlayerButton;

	private void Awake()
	{
		ApplicationMain = ApplicationMain.Instance;
		ApplicationMain.MainMenu = this;
	}

	private void Start()
	{
		EnableCurrentState(ApplicationMain.CurrentMenuState);

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
	}

	public void AddPlayer()
	{
		var item = Instantiate(takePhotoItem, SinglePlayerContent.transform);
		var index = ApplicationMain.Instance.makePhotoButtonData.Count;

		item.Init(index, item);
		//Item.transform.SetSiblingIndex(addPlayerTransform.GetSiblingIndex());
		//Item.myIndex = ApplicationMain.Instance.makePhotoButtonData.Count;
		//Item.button.onClick.AddListener(Item.MakePhoto);
		item.PlayerNumber.text = "NEW PLAYER";
		item.TakePhotoText.text = "TAKE PHOTO";

		ApplicationMain.Instance.makePhotoButtonData.Add(item);

		RecalculateAddPlayerButton();
	}

	private void RecalculateAddPlayerButton()
	{
		if (ApplicationMain.Instance.makePhotoButtonData.Count < 4)
		{
			addPlayerButton.gameObject.SetActive(true);
		}
		else
		{
			addPlayerButton.gameObject.SetActive(false);
		}
	}

	public void RecalculateItemNumbers()
	{
		for (int i = 0; i < ApplicationMain.Instance.makePhotoButtonData.Count; i++)
		{
			ApplicationMain.Instance.makePhotoButtonData[i].myIndex = i;
			ApplicationMain.Instance.makePhotoButtonData[i].PlayerNumber.text = "Player " + (i + 1);
		}

		RecalculateAddPlayerButton();
	}

	private void InitUITakePhotoButton()
	{
		Debug.Log("Count: " + ApplicationMain.Instance.makePhotoButtonData.Count);
		for (int i = 0; i < ApplicationMain.Instance.makePhotoButtonData.Count; i++)
		{
			Debug.Log("i: " + i);
			ApplicationMain.Instance.makePhotoButtonData[i] = Instantiate(takePhotoItem, SinglePlayerContent.transform);
			var item = ApplicationMain.Instance.makePhotoButtonData[i];
			item.Init(i, item);

			Debug.Log("ApplicationMain.Instance.cars: " + ApplicationMain.Instance.cars.Count);

			item.CurrentCar.texture = ApplicationMain.Instance.cars[i];
		}
	}

	public void PlayGame()
	{
		if (!string.IsNullOrEmpty(ApplicationMain.CurrentTrackName) && ApplicationMain.Instance.cars.Count > 0)
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
		ApplicationMain.CurrentMenuState = "SinglePlayerButton";
		DisabledAllWindows();
		SinglePlayerWindow.SetActive(true);
	}

	public void MultiPlayerButton()
	{
		ApplicationMain.CurrentMenuState = "MultiPlayerButton";
		DisabledAllWindows();
		MultiPlayerWindow.SetActive(true);
	}

	public void SettingsButton()
	{
		ApplicationMain.CurrentMenuState = "SettingsButton";
		DisabledAllWindows();
		SettingsMenuWindow.SetActive(true);
	}

	public void CooseATrack()
	{
		ApplicationMain.CurrentMenuState = "CooseATrack";
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
		//MultiPlayerWindow.SetActive(false);
		TrackMenuWindow.SetActive(false);
		SettingsMenuWindow.SetActive(false);
	}

	public void AddCar(Texture2D spriteCar)
	{
		ApplicationMain.cars.Add(spriteCar);
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
			case "MultiPlayerButton":
				MultiPlayerButton();
				break;
			case "SettingsButton":
				SettingsButton();
				break;
			case "CooseATrack":
				CooseATrack();
				break;
			case "QuitGame":
				QuitGame();
				break;
		}
	}


}
