using System.Collections;
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
	public GameObject MultiPlayerWindow;
	public GameObject TrackMenuWindow;
	public GameObject SettingsMenuWindow;

	private void Start()
	{

		ApplicationMain = ApplicationMain.Instance;
		ApplicationMain.MainMenu = this;

		Debug.Log("ApplicationMain.cars.Count: " + ApplicationMain.cars.Count);
		Debug.Log("ApplicationMain.CurrentMenuState: " + ApplicationMain.CurrentMenuState);
		EnableCurrentState(ApplicationMain.CurrentMenuState);
	}

	public void PlayGame()
	{
		if (!string.IsNullOrEmpty(ApplicationMain.CurrentTrackName))
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

	public void MakePhoto()
	{
		ApplicationMain.CurrentMenuState = "Photo";
		SceneManager.LoadScene("Photo");
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

	public void AddCar(int itemNumber, Texture2D spriteCar)
	{
		ApplicationMain.cars.Add(itemNumber, spriteCar);
	}

	public void RemoveCar(int itemNumber)
	{
		ApplicationMain.cars.Remove(itemNumber);
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
			case "MakePhoto":
				MakePhoto();
				break;
			case "QuitGame":
				QuitGame();
				break;
		}
	}
}
