using System;
using System.Collections;
//using OpenCvSharp;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rect = UnityEngine.Rect;

public class CameraScript : MonoBehaviour
{
	public Button takePhotoButton;
	public Button resetButton;
	public Button okButton;

	public Button ColorButtonPrefab;
	public Transform ColorConteiner;
	public RectTransform RocketTransform;
	private WebCamTexture webcamTexture;
	public RemoveBackgroundIS RemoveBackground;
	//public RawImage outputImage;

	//private MeshRenderer renderer;
	public Texture2D texture;
	public GameObject ResultPanel;
	public GameObject WebCamPanel;
	public GameObject errorWindow;
	public RawImage camTextureRawImage;

	private int currentSubstanceNameIndex;
	private int currentAdjectiveNameIndex;
	private int currentColorIndex;
	private Button firstColorButton;
	private CarNames carNames;

	//private const float Range = 0.2f;
	//private const float Fuzziness = 0.175f;

	//public Text TextRange;
	//public Text TextFuzziness;

	private Material material;

	private Action<bool> _hasTexture;
	public Texture2D localWebCamTexture;
	//public Custom.Texture textureScript;

	void Start()
	{
		webcamTexture = ApplicationMain.Instance.webCamTexture;

		takePhotoButton.onClick.AddListener(TakePhoto);
		resetButton.onClick.AddListener(ResetPhoto);
		okButton.onClick.AddListener(OkPhoto);

		InitRocketData();

		RemoveBackground.PlayerName.enabled = false;
		material = gameObject.GetComponent<MeshRenderer>().material;
		material.SetTexture("_MainTex", webcamTexture);
		camTextureRawImage.texture = webcamTexture;
		//material.SetTexture("_TextureSample0", webcamTexture);
		//material.SetFloat("_Range", Range);
		//material.SetFloat("_Fuzziness", Fuzziness);

		//camTextureRawImage.material = material;

		ResetPhoto();
		errorWindow.SetActive(false);
	}

	private async void InitRocketData()
	{
		carNames = gameObject.AddComponent<CarNames>();

		await carNames.GetCarNames();

		var allCarColors = carNames.GetCarColors;
		var createFirstColorButton = true;
		ApplicationMain.Instance.MainMenu.RecalculateColorCar();

		for (int i = 0; i < allCarColors.Count; i++)
		{
			if (ApplicationMain.CarColorsFree.Count > i && !ApplicationMain.CarColorsFree[i])
			{
				Debug.Log(ApplicationMain.CarColorsFree.Count + " continue: " + ApplicationMain.CarColorsFree[i]);
				continue;
			}

			var buttonColor = Instantiate(ColorButtonPrefab, ColorConteiner);
			buttonColor.image.color = allCarColors[i];

			if (ApplicationMain.CarColors.Count < allCarColors.Count)
			{
				ApplicationMain.CarColors.Add(allCarColors[i]);
				ApplicationMain.CarColorsFree.Add(true);
			}

			var index = i;
			buttonColor.onClick.AddListener(delegate { ChangeColorAndName(buttonColor.image.color, index); });

			if (createFirstColorButton)
			{
				firstColorButton = buttonColor;
				createFirstColorButton = false;
			}
		}

		if (ApplicationMain.SubstanceNames.Count == 0)
		{
			var allCarSubstances = carNames.GetCarSubstanceNames;
			ApplicationMain.SubstanceNames = allCarSubstances;

			for (int i = 0; i < allCarSubstances.Count; i++)
			{
				ApplicationMain.SubstancesFree.Add(true);
			}
		}
		else
		{
			Debug.Log("ELSE Substance");
			Debug.Log(ApplicationMain.Instance.CurrentSubstanceName);
			ApplicationMain.Instance.MainMenu.RecalculateSubstanceNameCar();
		}

			var allCarAdjectives = carNames.GetCarAdjectiveNames;
			ApplicationMain.AdjectiveNames = allCarAdjectives;
	}

	public void SetWebCamTexture(WebCamTexture wct)
	{
		webcamTexture = wct;
		webcamTexture.Play();
	}

	void ResetPhoto()
	{
		webcamTexture.Play();
		ResultPanel.SetActive(false);
		WebCamPanel.SetActive(true);
		RemoveBackground.PlayerName.enabled = false;
	}

	void ChangeColorAndName(Color currentColor, int index)
	{
		RemoveBackground.ColorName = carNames.GetCarColorNames[index];
		//RemoveBackground.outputImage.color = currentColor;
		rocketData.RocketImage.color = currentColor;
		RemoveBackground.SubstanceName = getSubstanceName(ApplicationMain.SubstanceNames);
		RemoveBackground.AdjectiveName = getAdjectiveName(ApplicationMain.AdjectiveNames);
		RemoveBackground.PlayerName.text = RemoveBackground.ColorName + " " + RemoveBackground.AdjectiveName + " "+ RemoveBackground.SubstanceName;
		RemoveBackground.PlayerName.color = currentColor;

		currentColorIndex = index;
	}

	private string getSubstanceName(IReadOnlyList<string> names)
	{
		currentSubstanceNameIndex++;

		if (currentSubstanceNameIndex == names.Count)
		{
			currentSubstanceNameIndex = 0;
		}

		if (!ApplicationMain.SubstancesFree[currentSubstanceNameIndex])
		{
			getSubstanceName(names);
		}

		return names[currentSubstanceNameIndex];
	}

	private string getAdjectiveName(IReadOnlyList<string> adjectiveNames)
	{
		currentAdjectiveNameIndex++;

		if (currentAdjectiveNameIndex == adjectiveNames.Count)
		{
			currentAdjectiveNameIndex = 0;
		}

		return adjectiveNames[currentAdjectiveNameIndex];
	}
	public RocketData rocketData;
	public Texture2D teeeeee;
	void OkPhoto()
	{
		//outputImage.texture = null;
		RemoveBackground.LoadingWindow.SetActive(true);

		var rocketTex = rocketData.RocketImage.mainTexture as Texture2D;
		Texture2D rocketTexture = new Texture2D(rocketTex.width, rocketTex.height);
		rocketTexture.SetPixels(rocketTex.GetPixels());
		rocketTexture.Apply();

		TextureScale.Point(texture, rocketData.SizePlayerImage, rocketData.SizePlayerImage);
		Texture2DUtility.SplitTextures(ref rocketTexture, texture, (int)rocketData.x, (int)rocketData.y);

		//texture = rocketData.RocketImage.mainTexture as Texture2D;
		teeeeee = rocketTexture;
		RaceCarScript carObject = new RaceCarScript();
		carObject.CarTexture = rocketTexture;
		carObject.CarColor = rocketData.RocketImage.color;
		carObject.SubstanceName = RemoveBackground.SubstanceName;
		carObject.CarColorName = RemoveBackground.ColorName;
		carObject.AdjectiveName = RemoveBackground.AdjectiveName;
		carObject.PlayerName = carObject.CarColorName + " " +carObject.AdjectiveName + " " + carObject.SubstanceName;
		carObject.SubstanceIndex = currentSubstanceNameIndex;

		ApplicationMain.SubstancesFree[currentSubstanceNameIndex] = false;
		ApplicationMain.CarColorsFree[currentColorIndex] = false;

		if (ApplicationMain.Instance.CurrentCarHasTexture)
		{
			ApplicationMain.RaceCars[ApplicationMain.Instance.CurrentCarIndex] = carObject;
		}
		else
		{
			ApplicationMain.RaceCars.Add(carObject);
		}

		ApplicationMain.Instance.CurrentMenuState = "SinglePlayerButton";

		SceneManager.LoadScene("Menu");
	}

	public Texture2D TakePhotoInUI(RectTransform rectT, int width, int height)
	{
		Vector2 temp = rectT.transform.position;
		var startX = temp.x - width / 2;
		var startY = temp.y - height / 2;
		Debug.Log("temp: " + temp);
		Debug.Log("startX: " + startX);
		Debug.Log("startY: " + startY);
		var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
		tex.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
		tex.Apply();

		return tex;
	}

	private void TakePhoto()
    {
		Texture2D playerTexture = new Texture2D(webcamTexture.width, webcamTexture.height);
		playerTexture.SetPixels(webcamTexture.GetPixels());
		playerTexture.Apply();

		Texture2DUtility.RescaleTextureByHeight(playerTexture, rocketData.MaskTexture.height);

		Texture2DUtility.SplitTextures(rocketData.MaskTexture, playerTexture);

		texture = playerTexture;
		rocketData.PlayerImage.texture = playerTexture;
		//RemoveBackground.SetImageToRocket(snap);//RemoveBackground.RemoveBackgroundOnTexture(localWebCamTexture, _hasTexture);

		ResultPanel.SetActive(true);
		WebCamPanel.SetActive(false);
		firstColorButton.onClick.Invoke();
		RemoveBackground.PlayerName.enabled = true;

	}

	//private void TakePhoto()
	//{
	//	webcamTexture.Pause();
	//	_hasTexture = HasTexture;

	//	var tex = textureScript.Capture(webcamTexture.width, webcamTexture.height, material);

	//	Debug.Log("tex: " + tex == null);
	//	RenderTexture.active = tex;
	//	localWebCamTexture = new Texture2D(webcamTexture.width, webcamTexture.height);
	//	localWebCamTexture.ReadPixels(new Rect(0, 0, webcamTexture.width, webcamTexture.height), 0, 0);
	//	localWebCamTexture.Apply();

	//	//webcamTexture.Play();

	//	texture = RemoveBackground.RemoveBackgroundOnTexture(localWebCamTexture, _hasTexture);
	//	//Далее переходим в метод HasTexture через Action _hasTexture
	//}

	public Color[] pixels(Texture text)
	{
		Texture mainTexture = text;
		Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

		RenderTexture currentRT = RenderTexture.active;

		RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 24);
		Graphics.Blit(mainTexture, renderTexture);

		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		texture2D.Apply();

		var colors = texture2D.GetPixels();

		RenderTexture.active = currentRT;

		return colors;

		
	}

	public Texture2D ToTexture2D(Texture texture)
	{
		return Texture2D.CreateExternalTexture(
			texture.width,
			texture.height,
			TextureFormat.RGB24,
			false, true,
			texture.GetNativeTexturePtr());
	}

	private void HasTexture(bool hasTexture)
	{
		if (hasTexture)
		{
			ResultPanel.SetActive(true);
			WebCamPanel.SetActive(false);
			firstColorButton.onClick.Invoke();
		}
		else
		{
			StartCoroutine(ErrorWindow());
		}
	}

	IEnumerator ErrorWindow()
	{
		errorWindow.SetActive(true);
		yield return new WaitForSeconds(2);
		errorWindow.SetActive(false);
	}

    //Проверить без этого как будет показываться изображение на android и ios

	//private void Update()
	//{
	//	if (webcamTexture.width < 100)
	//	{
	//		//Debug.Log("Still waiting another frame for correct info...");
	//		return;
	//	}

	//	int cwNeeded = webcamTexture.videoRotationAngle;
	//	int ccwNeeded = -cwNeeded;
	//	if (webcamTexture.videoVerticallyMirrored) ccwNeeded += 180;

	//	if (webcamTexture.videoVerticallyMirrored)
	//		camTextureRawImage.uvRect = new Rect(1, 0, -1, 1);  // means flip on vertical axis
	//	else
	//		camTextureRawImage.uvRect = new Rect(0, 0, 1, 1);  // means no flip

	//}
}