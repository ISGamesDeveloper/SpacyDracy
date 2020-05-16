using System.Collections.Generic;
using UnityEngine;

public class ApplicationMain : Singleton<ApplicationMain>
{
    public WebCamTexture webCamTexture;
    public WebCamTexture frontWebCamTexture;
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
    public bool GameIsPlaying;

    public WebCamDevice[] wdcs;

    bool isFace;

    public const int MaxLapCount = 99;
    public const int MaxPlayerCount = 10;

    public void Awake()
    {
        //NativeCamera.OpenSettings();
        NativeCamera.RequestPermission();

        InitDeviceCamera();

        LocalizationSettings.SetLocalizationSettings(Language.en);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void InitDeviceCamera()
    {
        wdcs = WebCamTexture.devices;
        webCamTexture = new WebCamTexture();
        webCamTexture.deviceName = wdcs[0].name;
        //webCamTexture.Play();

        webCamTexture.requestedWidth = Screen.width;
        webCamTexture.requestedHeight = Screen.height;
        webCamTexture.requestedFPS = 25;
    }


    public void ChangeDeviceCamera()
    {
        webCamTexture.Stop();

        if (wdcs.Length == 1)
        {
            webCamTexture.deviceName = wdcs[0].name;
        }
        else
        {
            webCamTexture.deviceName = (webCamTexture.deviceName == wdcs[0].name) ? wdcs[1].name : wdcs[0].name;
        }

        webCamTexture.Play();
    }

    public void GetPlayerNames()
    {
        for (int i = 0; i < RaceCars.Count; i++)
        {
            Debug.Log($"RaceCars PlayerName {i} : " + RaceCars[i].PlayerName);
        }
    }
}
