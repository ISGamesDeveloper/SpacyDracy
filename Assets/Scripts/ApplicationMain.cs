using System.Collections.Generic;
using UnityEngine;

public class ApplicationMain : Singleton<ApplicationMain>
{
	public WebCamTexture webCamTexture;
	public MainMenu MainMenu;

	public Dictionary<int, Texture2D> cars = new Dictionary<int, Texture2D>();

	public string CurrentMenuState = "MainMenuButton";
	public string CurrentTrackName;

	public void Start()
	{
		webCamTexture = new WebCamTexture();
		webCamTexture.requestedWidth = 640;
		webCamTexture.requestedHeight = 480;
		webCamTexture.requestedFPS = 15;


		Debug.Log("cars: " + cars.Count);
	}
}
