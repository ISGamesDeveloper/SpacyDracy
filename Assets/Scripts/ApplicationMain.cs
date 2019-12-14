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

	public string CurrentMenuState = "MainMenuButton";
	public string CurrentTrackName;
	public int CurrentCarIndex;
	public bool CurrentCarHasTexture;

	public void Start()
	{
		webCamTexture = new WebCamTexture();
		//webCamTexture.requestedWidth = 640;
		//webCamTexture.requestedHeight = 480;
		webCamTexture.requestedWidth = 1920;
		webCamTexture.requestedHeight = 1080;
		webCamTexture.requestedFPS = 30;
	}
}
