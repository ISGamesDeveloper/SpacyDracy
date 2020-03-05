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

	public Slider slider;
	private WebCamTexture webcamTexture;
	public RemoveBackgroundIS RemoveBackground;
	//private MeshRenderer renderer;
	public Texture2D texture;
	public GameObject window;
	public GameObject errorWindow;
	public RawImage camTextureRawImage;

	private int currentSubstanceNameIndex;
	private int currentAdjectiveNameIndex;
	private int currentColorIndex;
	private Button firstColorButton;
	private CarNames carNames;

	public Slider Range;
	public Slider Fuzziness;

	public Text TextRange;
	public Text TextFuzziness;

	private Material material;

	void Start()
	{
		webcamTexture = ApplicationMain.Instance.webCamTexture;

		slider.onValueChanged.AddListener(Slider);
		slider.minValue = 0f;
		slider.maxValue = 0.08f;
		slider.gameObject.SetActive(false);

		takePhotoButton.onClick.AddListener(TakePhoto);
		resetButton.onClick.AddListener(ResetPhoto);
		okButton.onClick.AddListener(OkPhoto);

		StartCoroutine(InitRocketData());

		//renderer = GetComponent<MeshRenderer>();
		//renderer.material.mainTexture = webcamTexture;
		RemoveBackground.PlayerName.enabled = false;
		material = gameObject.GetComponent<MeshRenderer>().material;
		material.SetTexture("_TextureSample0", webcamTexture);
		
		camTextureRawImage.texture = material.mainTexture;
		//cam.targetTexture = (RenderTexture)renderTexture.mainTexture;
		Range.maxValue = 1;
		Fuzziness.maxValue = 1;

		Range.minValue = 0;
		Fuzziness.minValue = 0;

		Range.value = material.GetFloat("_Range");
		Fuzziness.value = material.GetFloat("_Fuzziness");

		Range.onValueChanged.AddListener((value) => {
			material.SetFloat("_Range", value);
			TextRange.text = "Range: " + value;
		});

		Fuzziness.onValueChanged.AddListener((value) => {
			material.SetFloat("_Fuzziness", value);
			TextFuzziness.text = "Fuzziness: " + value;
		});

		ResetPhoto();
		errorWindow.SetActive(false);
	}

	private IEnumerator InitRocketData()
	{
		carNames = gameObject.AddComponent<CarNames>();

		yield return StartCoroutine(carNames.GetData());

		var allCarColors = carNames.GetCarColors;
		var createFirstColorButton = true;
		ApplicationMain.Instance.MainMenu.RecalculateColorCar();
		Debug.Log("allCarColors: " + allCarColors.Count);
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

			//Debug.Log("createFirstColorButton: " + createFirstColorButton);
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

		//if (ApplicationMain.AdjectiveNames.Count == 0)
		//{
			var allCarAdjectives = carNames.GetCarAdjectiveNames;
			ApplicationMain.AdjectiveNames = allCarAdjectives;

		//	for (int i = 0; i < allCarAdjectives.Count; i++)
		//	{
		//		ApplicationMain.AdjectivesFree.Add(true);
		//	}
		//}
		//else
		//{
		//	Debug.Log("ELSE Adjective");
		//	Debug.Log(ApplicationMain.Instance.CurrentAdjectiveName);
		//	ApplicationMain.Instance.MainMenu.RecalculateAdjectiveNameCar();
		//}
	}

	public void SetWebCamTexture(WebCamTexture wct)
	{
		webcamTexture = wct;
		webcamTexture.Play();
	}

	void ResetPhoto()
	{
		slider.value = (float)RemoveBackgroundIS.FloodFillToleranceDefaultValue;
		webcamTexture.Play();
		window.SetActive(false);
	}

	void ChangeColorAndName(Color currentColor, int index)
	{
		RemoveBackground.ColorName = carNames.GetCarColorNames[index];
		RemoveBackground.outputImage.color = currentColor;
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

		//if (!ApplicationMain.AdjectivesFree[currentAdjectiveNameIndex])
		//{
		//	getAdjectiveName(adjectiveNames);
		//}

		return adjectiveNames[currentAdjectiveNameIndex];
	}

	void OkPhoto()
	{
		RemoveBackground.outputImage.texture = null;
		RemoveBackground.LoadingWindow.SetActive(true);

		RescaleTexture(208.0f, texture.width, texture.height);

		//if (texture.height > 128)
		//{
		//	RescaleTexture(128.0f, texture.height, texture.width, true);
		//}
		Debug.Log("ApplicationMain.Instance.CurrentCarIndex: " + ApplicationMain.Instance.CurrentCarIndex);

		RaceCarScript carObject = new RaceCarScript();
		carObject.CarTexture = texture;
		carObject.CarColor = RemoveBackground.outputImage.color;
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
			//Debug.Log("ApplicationMain.Instance.CarObjects[i]: " + ApplicationMain.RaceCars[ApplicationMain.Instance.CurrentCarIndex] == null);
			//Debug.Log("ApplicationMain.Instance.CarObjects[i].CarColor: " + ApplicationMain.RaceCars[ApplicationMain.Instance.CurrentCarIndex].CarColor);
			//Debug.Log("ApplicationMain.Instance.CarObjects[i].CarTexture: " + ApplicationMain.RaceCars[ApplicationMain.Instance.CurrentCarIndex].CarTexture == null);
		}

		ApplicationMain.Instance.CurrentMenuState = "SinglePlayerButton";

		SceneManager.LoadScene("Menu");
	}

	private void RescaleTexture(float rescaleFactor, int sizeOne, int sizeTwo, bool isHeight = false)
	{
		float coeff = sizeOne / rescaleFactor;
		float x = sizeOne / coeff;
		float y = sizeTwo / coeff;

		if (isHeight)
		{
			TextureScale.Point(texture, (int)y, (int)x);
		}
		else
		{
			TextureScale.Point(texture, (int)x, (int)y);
		}

	}

	private Action<bool> _hasTexture;
	public Texture2D localWebCamTexture;

	public Custom.Texture textureScript;
	//public RenderTexture renderTexture;
	private async void TakePhoto()
	{
		webcamTexture.Pause();
		_hasTexture = HasTexture;
		//var t = material.GetTexture("_TextureSample0");

		//renderTexture = new RenderTexture(webcamTexture.width, webcamTexture.height, 24);
		//await UniTask.DelayFrame(1);

		//Graphics.Blit(t, new RenderTexture(webcamTexture.width, webcamTexture.height, 24), material);

		//localWebCamTexture = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);
		//localWebCamTexture.ReadPixels(new Rect(0, 0, webcamTexture.width, webcamTexture.height), 0, 0);
		//localWebCamTexture.Apply();//TextureToTexture2D(renderTexture);


		var tex = textureScript.Capture(webcamTexture.width, webcamTexture.height, material);

		Debug.Log("tex: " + tex == null);
		RenderTexture.active = tex;
		localWebCamTexture = new Texture2D(webcamTexture.width, webcamTexture.height);
		localWebCamTexture.ReadPixels(new Rect(0, 0, webcamTexture.width, webcamTexture.height), 0, 0);
		localWebCamTexture.Apply();

		webcamTexture.Play();

		CallRemoveBackground(localWebCamTexture, _hasTexture);
		//Далее переходим в метод HasTexture через Action _hasTexture
	}

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

	//private Texture2D TextureToTexture2D(Texture texture)
	//{
	//	Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
	//	RenderTexture currentRT = RenderTexture.active;
	//	RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
	//	Graphics.Blit(texture, renderTexture);

	//	RenderTexture.active = renderTexture;
	//	texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
	//	texture2D.Apply();

	//	RenderTexture.active = currentRT;
	//	RenderTexture.ReleaseTemporary(renderTexture);
	//	return texture2D;
	//}

	private void HasTexture(bool hasTexture)
	{
		//Debug.Log("HasTexture: " + hasTexture);

		if (hasTexture)
		{
			window.SetActive(true);
			firstColorButton.onClick.Invoke(); //кликаем на первый цвет в списке
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

	private void Slider(float value)
	{
		//if (texture == null || !RemoveBackground.Processed)
		//{
		//	Debug.Log("return");
		//	return;
		//}

		//CallRemoveBackground(value);
	}

	private void CallRemoveBackground(Texture2D camTexture, Action<bool> hasTexture, float value = 0)
	{
		if (value == 0)
		{
			texture = RemoveBackground.RemoveBackgroundOnTexture(camTexture, hasTexture);
		}
		else
		{
			texture = RemoveBackground.ChangeValueOnFloodFillTolerance(camTexture, value);
		}


		//for (int y = 0; y < texture.height; y++)
		//{
		//	for (int x = 0; x < texture.width; x++)
		//	{
		//		Color color = ((x & y) != 0 ? Color.white : Color.gray);
		//		texture.SetPixel(x, y, color);
		//	}
		//

		//RemoveBackground.RemoveBorder(10, texture);

		//texture = ConvertTextureToGrayScale(texture);

		//WhiteTexture(texture); закрашивает текстуру
	}

	

	private void WhiteTexture(Texture2D localTexture)
	{
		for (int x = 0; x < localTexture.width; x++) //ширина 
		{
			for (int y = 0; y < localTexture.height; y++) //высота
			{
				if (localTexture.GetPixel(x, y).a > 0)
				{
					localTexture.SetPixel(x, y, Color.red);
				}
			}
		}

		localTexture.Apply();
	}

	//private Texture2D ConvertTextureToGrayScale(Texture2D t)
	//{
	//	var mat = OpenCvSharp.Unity.TextureToMat(t);
	//	Cv2.CvtColor(mat, mat, ColorConversionCodes.RGB2GRAY);
	//	//Cv2.ColorChange(mat, mat, mat, 1f, 1f,1f); юнити грохнется
	//	var texture = OpenCvSharp.Unity.MatToTexture(mat);
	//	//WhiteTexture(texture);
	//	return texture;
	//}

	

	private void Update()
	{
		if (webcamTexture.width < 100)
		{
			Debug.Log("Still waiting another frame for correct info...");
			return;
		}

		int cwNeeded = webcamTexture.videoRotationAngle;
		int ccwNeeded = -cwNeeded;
		if (webcamTexture.videoVerticallyMirrored) ccwNeeded += 180;

		if (webcamTexture.videoVerticallyMirrored)
			camTextureRawImage.uvRect = new Rect(1, 0, -1, 1);  // means flip on vertical axis
		else
			camTextureRawImage.uvRect = new Rect(0, 0, 1, 1);  // means no flip

	}
}