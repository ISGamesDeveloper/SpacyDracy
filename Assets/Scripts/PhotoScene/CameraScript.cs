using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
	public Button takePhotoButton;
	public Button resetButton;
	public Button okButton;

	public Slider slider;
	private WebCamTexture webcamTexture;
	public RemoveBackgroundIS RemoveBackground;
	private MeshRenderer renderer;
	public Texture2D texture;
	public GameObject window;

	void Start()
	{
		webcamTexture = ApplicationMain.Instance.webCamTexture;

		slider.onValueChanged.AddListener(Slider);
		slider.minValue = 0f;
		slider.maxValue = 0.08f;

		takePhotoButton.onClick.AddListener(TakePhoto);
		resetButton.onClick.AddListener(ResetPhoto);
		okButton.onClick.AddListener(OkPhoto); 

		renderer = GetComponent<MeshRenderer>();
		renderer.material.mainTexture = webcamTexture;

		ResetPhoto();
	}

	void ResetPhoto()
	{
		slider.value = (float)RemoveBackgroundIS.FloodFillToleranceDefaultValue;
		webcamTexture.Play();
		window.SetActive(false);
	}

	void OkPhoto()
	{
		RemoveAlphaBorder(texture);// после удовлетворительного фото (нажатия на галочку) удалить лишнюю пустоту
		RescaleTexture(208.0f, texture.width, texture.height);

		if (texture.height > 128)
		{
			RescaleTexture(128.0f, texture.height, texture.width, true);
		}
		Debug.Log("ApplicationMain.Instance.CurrentCarIndex: " + ApplicationMain.Instance.CurrentCarIndex);

		ApplicationMain.Instance.MainMenu.AddCar(texture);
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

	private void TakePhoto()
	{
		texture = new Texture2D(renderer.material.mainTexture.width, renderer.material.mainTexture.height);
		texture.SetPixels(0, 0, texture.width, texture.height, webcamTexture.GetPixels());
		texture.Apply();

		texture = RemoveBackground.RemoveBackgroundOnTexture(texture);

		window.SetActive(true);
	}

	private void Slider(float value)
	{
		if (texture == null || !RemoveBackground.Processed)
		{
			Debug.Log("return");
			return;
		}

		texture = RemoveBackground.ChangeValueOnFloodFillTolerance(texture, value);
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
}