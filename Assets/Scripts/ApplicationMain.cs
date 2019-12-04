using System.Collections.Generic;
using UnityEngine;

public class ApplicationMain : Singleton<ApplicationMain>
{
	public WebCamTexture webCamTexture;
	public MainMenu MainMenu;
	public GamePlayManager GamePlayManager;
	public CameraManager CameraManager;
	public List<Texture2D> cars = new List<Texture2D>();
	public static List<MakePhotoButtonData> makePhotoButtonData = new List<MakePhotoButtonData>();

	public string CurrentMenuState = "MainMenuButton";
	public string CurrentTrackName;
	public int CurrentCarIndex;
	public bool CurrentCarHasTexture;

	public void Start()
	{
		webCamTexture = new WebCamTexture();
		webCamTexture.requestedWidth = 640;
		webCamTexture.requestedHeight = 480;
		webCamTexture.requestedFPS = 15;
	}
}
