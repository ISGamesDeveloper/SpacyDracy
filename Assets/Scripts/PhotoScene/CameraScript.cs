using System.Collections.Generic;
using System.Collections;
using OpenCvSharp;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rect = UnityEngine.Rect;

public class CameraScript : MonoBehaviour
{
	public Button takePhotoButton;
	public Button resetButton;
	public Button okButton;

	public Button ColorRedButton;
	public Button ColorGreenButton;
	public Button ColorBlueButton;

	public Slider slider;
	private WebCamTexture webcamTexture;
	public RemoveBackgroundIS RemoveBackground;
	//private MeshRenderer renderer;
	public Texture2D texture;
	public GameObject window;
	public RawImage camTextureRawImage;

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

		ColorRedButton.onClick.AddListener(delegate { ChangeColorAndName(ColorRedButton.image.color); });
		ColorGreenButton.onClick.AddListener(delegate { ChangeColorAndName(ColorGreenButton.image.color); });
		ColorBlueButton.onClick.AddListener(delegate { ChangeColorAndName(ColorBlueButton.image.color); });

		//renderer = GetComponent<MeshRenderer>();
		//renderer.material.mainTexture = webcamTexture;
		camTextureRawImage.texture = webcamTexture;
		ResetPhoto();
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

	void ChangeColorAndName(Color currentColor)
	{
		RemoveBackground.outputImage.color = currentColor;

		if (currentColor.Equals(ColorRedButton.image.color))
		{
			RemoveBackground.CarName.text = "Red dragon";
		}
		else if (currentColor.Equals(ColorGreenButton.image.color))
		{
			RemoveBackground.CarName.text = "Green pantera";
		}
		else if (currentColor.Equals(ColorBlueButton.image.color))
		{
			RemoveBackground.CarName.text = "Blue sea";
		}
	}

	void OkPhoto()
	{
		RemoveAlphaBorder(texture);// после удовлетворительного фото (нажатия на галочку) удалить лишнюю пустоту
		//RescaleTexture(208.0f, texture.width, texture.height);

		//if (texture.height > 128)
		//{
		//	RescaleTexture(128.0f, texture.height, texture.width, true);
		//}
		Debug.Log("ApplicationMain.Instance.CurrentCarIndex: " + ApplicationMain.Instance.CurrentCarIndex);
		RaceCarScript carObject = new RaceCarScript();
		carObject.CarTexture = texture;
		carObject.CarColor = RemoveBackground.outputImage.color;
		carObject.CarName = RemoveBackground.CarName.text;

		if (ApplicationMain.Instance.CurrentCarHasTexture)
		{
			ApplicationMain.RaceCars[ApplicationMain.Instance.CurrentCarIndex] = carObject;
		}
		else
		{
			ApplicationMain.RaceCars.Add(carObject);
			Debug.Log("ApplicationMain.Instance.CarObjects[i]: " + ApplicationMain.RaceCars[ApplicationMain.Instance.CurrentCarIndex] == null);
			Debug.Log("ApplicationMain.Instance.CarObjects[i].CarColor: " + ApplicationMain.RaceCars[ApplicationMain.Instance.CurrentCarIndex].CarColor);
			Debug.Log("ApplicationMain.Instance.CarObjects[i].CarTexture: " + ApplicationMain.RaceCars[ApplicationMain.Instance.CurrentCarIndex].CarTexture == null);
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

	private Texture2D localWebCamTexture;
	private void TakePhoto()
	{
		localWebCamTexture = new Texture2D(webcamTexture.width, webcamTexture.height);
		localWebCamTexture.SetPixels(0, 0, localWebCamTexture.width, localWebCamTexture.height, webcamTexture.GetPixels());
		localWebCamTexture.Apply();

		CallRemoveBackground();
		window.SetActive(true);
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

	private void CallRemoveBackground(float value = 0)
	{
		if (value == 0)
		{
			texture = RemoveBackground.RemoveBackgroundOnTexture(localWebCamTexture);
		}
		else
		{
			texture = RemoveBackground.ChangeValueOnFloodFillTolerance(localWebCamTexture, value);
		}

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
					localTexture.SetPixel(x,y,Color.red);
				}
			}
		}

		localTexture.Apply();
	}

	private Texture2D ConvertTextureToGrayScale(Texture2D t)
	{
		var mat = OpenCvSharp.Unity.TextureToMat(t);
		Cv2.CvtColor(mat, mat, ColorConversionCodes.RGB2GRAY);
		//Cv2.ColorChange(mat, mat, mat, 1f, 1f,1f); юнити грохнется
		var texture = OpenCvSharp.Unity.MatToTexture(mat);
		//WhiteTexture(texture);
		return texture;
	}

	private void RemoveAlphaBorder(Texture2D texture)
	{
		var minX = -1;
		var maxX = -1;

		var minY = -1;
		var maxY = -1;

		List<int> xPoints = new List<int>();
		List<int> yPoints = new List<int>();

		for (int x = 0; x < texture.width; x++) //ширина 
		{
			for (int y = 0; y < texture.height; y++) //высота
			{
				var color = texture.GetPixel(x, y);
				var currentPixel = color.a;

				if (currentPixel > 0)
				{
					xPoints.Add(x);
					yPoints.Add(y);
				}
			}
		}

		xPoints.Sort();
		yPoints.Sort();

		Debug.Log("xPoints 0: " + xPoints[0] + "  yPoints 0: " + yPoints[0]);
		Debug.Log("xPoints max: " + xPoints[xPoints.Count - 1] + "  yPoints max: " + yPoints[yPoints.Count - 1]);

		var width = xPoints[xPoints.Count - 1] - xPoints[0];
		var height = yPoints[yPoints.Count - 1] - yPoints[0];

		//ПО КРАЯМ ПОЯВЛЯЮТСЯ СЛЕДЫ В 1 ПИКСЕЛЬ!!!
		Color[] pix = texture.GetPixels(xPoints[0], yPoints[yPoints.Count - 1] - height, width, height);
		Debug.Log(width);
		Debug.Log(height);
		Debug.Log(pix.Length);
		texture.Resize(width, height);
		texture.SetPixels(pix);
		texture.Apply();
	}

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