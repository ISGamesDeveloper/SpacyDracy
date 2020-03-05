using System.Collections.Generic;
using UnityEngine;

public class ApplicationMain : Singleton<ApplicationMain>
{
	public WebCamTexture webCamTexture;
	public MainMenu MainMenu;
	public GamePlayManager GamePlayManager;
	public CameraManager CameraManager;

	public static List<RaceCarScript> RaceCars = new List<RaceCarScript>();
	public static List<MakePhotoButtonData> makePhotoButtonData = new List<MakePhotoButtonData>();

	public static List<string> SubstanceNames = new List<string>();
	public static List<bool> SubstancesFree = new List<bool>();
	public static List<string> AdjectiveNames = new List<string>();
	//public static List<bool> AdjectivesFree = new List<bool>();
	public static List<Color> CarColors = new List<Color>();
	public static List<bool> CarColorsFree = new List<bool>();

	public string CurrentMenuState = "MainMenuButton";
	public string CurrentTrackName;
	public int CurrentCarIndex;
	public bool CurrentCarHasTexture;
	public Color CurrentCarColor;
	public string CurrentSubstanceName;
	public string CurrentAdjectiveName;

	public void Start()
	{
		LocalizationSettings.SetLocalizationSettings(Language.en);

		webCamTexture = new WebCamTexture();
		//webCamTexture.requestedWidth = 640;
		//webCamTexture.requestedHeight = 480;
		webCamTexture.requestedWidth = 1920;
		webCamTexture.requestedHeight = 1080;
		webCamTexture.requestedFPS = 30;
	}

	public void GetPlayerNames()
	{
		for (int i = 0; i < RaceCars.Count; i++)
		{
			Debug.Log($"RaceCars PlayerName {i} : " + RaceCars[i].PlayerName);
		}
	}
}
