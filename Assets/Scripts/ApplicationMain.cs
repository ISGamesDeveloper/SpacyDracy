using System.Collections.Generic;
using UnityEngine;

public class ApplicationMain : Singleton<ApplicationMain>
{
	public WebCamTexture webCamTexture;
	public MainMenu MainMenu;

	public List<Texture2D> cars = new List<Texture2D>();
	public List<MakePhotoButtonData> makePhotoButtonData = new List<MakePhotoButtonData>();

	public string CurrentMenuState = "MainMenuButton";
	public string CurrentTrackName;
	public int CurrentCarIndex;

	public void Start()
	{
		webCamTexture = new WebCamTexture();
		webCamTexture.requestedWidth = 640;
		webCamTexture.requestedHeight = 480;
		webCamTexture.requestedFPS = 15;


		Debug.Log("cars: " + cars.Count);
	}
}
