using System;
using System.Collections.Generic;
using BackgroundRemovalSample.App;
using OpenCvSharp;
using OpenCvSharp.Demo;
using UnityEngine;
using UnityEngine.UI;

public class RemoveBackgroundIS : MonoBehaviour
{
	public RawImage InputImage;
	public RawImage outputImage;
	public RawImage muskImage, muskImage2;
	public Text PlayerName;
	public GameObject LoadingWindow;
	public string ColorName;
	public string SubstanceName;
	public string AdjectiveName;

	private PaperScanner scanner = new PaperScanner();
	private Vector2 rectSize;

	[HideInInspector]
	public double FloodFillTolerance = 0.018f;
	public const double FloodFillToleranceDefaultValue = 0.018f;
	public int MaskBlurFactor = 5;
	public Texture2D ProcessedTexture;
	public bool Processed;

	private void Start()
	{
		rectSize = outputImage.rectTransform.sizeDelta;
	}

	public Texture2D RemoveBackgroundOnTexture(Texture2D texture, Action<bool> hasTexture)
	{
		LoadingWindow.SetActive(false);
		InputImage.texture = texture;
		//texture = Init(texture);//убрать потом

		var currentTexture = Run(OpenCvSharp.Unity.TextureToMat(texture));
		currentTexture.Apply();

		RemoveBorder(10, currentTexture);

		var hasColor = CheckTexture(currentTexture);

		//Если меньше 1000 закрашенных пикселей, то посылаем событие false
		if (!hasColor)
		{
			hasTexture.Invoke(false);
			return null;
		}

		//Если больше 1000 закрашенных пикселей, то посылаем событие true
		hasTexture.Invoke(true);

		Debug.Log("sizeDelta.x: " + outputImage.rectTransform.sizeDelta.x);
		Debug.Log("sizeDelta.y: " + outputImage.rectTransform.sizeDelta.y);

		RemoveAlphaBorder(currentTexture);// после удовлетворительного фото (нажатия на галочку) удалить лишнюю пустоту
		var transform = outputImage.rectTransform;
		ChangeRawImageDelta(currentTexture, transform);
		outputImage.texture = currentTexture;
		//outputImage.SetNativeSize();

		ProcessedTexture = outputImage.texture as Texture2D;
		PlayerName.enabled = true;
		Processed = true;

		

		return ProcessedTexture;
	}

	private void ChangeRawImageDelta(Texture texture, RectTransform transform)
	{
		Debug.Log("texture.width: " + texture.width + "    texture.height: " + texture.height);
		Vector2 vector;

		var coeff = 0f;

		if (texture.width > texture.height)
		{
			coeff = texture.width / 560.0f;
			Debug.Log("texture.width / 560: " + coeff);
			vector.x = texture.width / coeff;
			vector.y = texture.height / coeff;


		}
		else
		{
			coeff = texture.height / 560.0f;
			Debug.Log("texture.height / 560.0f: " + coeff);
			vector.y = 420.0f;

			vector.x = texture.width / coeff;
		}

		outputImage.rectTransform.sizeDelta = vector;
	}

	private void RemoveAlphaBorder(Texture2D texture)
	{
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

		//Debug.Log("xPoints 0: " + xPoints[0] + "  yPoints 0: " + yPoints[0]);
		//Debug.Log("xPoints max: " + xPoints[xPoints.Count - 1] + "  yPoints max: " + yPoints[yPoints.Count - 1]);

		var width = xPoints[xPoints.Count - 1] - xPoints[0];
		var height = yPoints[yPoints.Count - 1] - yPoints[0];

		//ПО КРАЯМ ПОЯВЛЯЮТСЯ СЛЕДЫ В 1 ПИКСЕЛЬ!!!
		//Color[] pix = texture.GetPixels(xPoints[0], yPoints[yPoints.Count - 1] - height, width, height);
		Color[] pix = texture.GetPixels(xPoints[xPoints.Count - 1] - width, yPoints[yPoints.Count - 1] - height, width, height);
		//if (xPoints.Count > yPoints.Count)
		//{
		//	pix = texture.GetPixels(xPoints[0], yPoints[yPoints.Count - 1] - height, width, height);
		//}
		//else
		//{

		//}
		Debug.Log(width);
		Debug.Log(height);
		texture.Resize(width, height);
		texture.SetPixels(pix);
		texture.Apply();

		RemoveBorder(4, texture);
	}

	public void RemoveBorder(int borderWidth, Texture2D texture)
	{
		for (int x = 0; x < texture.width; x++)
		{
			for (int i = 0; i < borderWidth; i++)
				texture.SetPixel(x, i, Color.clear);
		}

		for (int x = 0; x < texture.width; x++)
		{
			for (int i = 0; i < borderWidth; i++)
				texture.SetPixel(x, texture.height - i, Color.clear);
		}
		for (int y = 0; y < texture.height; y++)
		{
			for (int i = 0; i < borderWidth; i++)
				texture.SetPixel(i, y, Color.clear);
		}
		for (int y = 0; y < texture.height; y++)
		{
			for (int i = 0; i < borderWidth; i++)
				texture.SetPixel(texture.width - i, y, Color.clear);
		}

		texture.Apply();
	}
	private bool CheckTexture(Texture2D texture)
	{
		int colorPixelsCount = 0;
		var colors = texture.GetPixels();
		var minPixelCount = 1000;

		for (int i = 0; i < colors.Length; i+=2)
		{
			if (colors[i].a > 0 && colorPixelsCount < minPixelCount)
				colorPixelsCount++;
			else if (colorPixelsCount >= minPixelCount)
				break;
		}

		//Debug.Log("colorPixelsCount: " + colorPixelsCount);
		return colorPixelsCount >= minPixelCount;
	}

	public Texture2D ChangeValueOnFloodFillTolerance(Texture2D texture, float value)
	{
		//Processed = false;
		//FloodFillTolerance = value;
		//outputImage.texture = Run(OpenCvSharp.Unity.TextureToMat(texture));
		//ProcessedTexture = outputImage.texture as Texture2D;
		//Processed = true;
		return ProcessedTexture;
	}

	public Texture2D Run(Mat mat)
	{
		var filter = new RemoveBackgroundOpenCvFilter
		{
			FloodFillTolerance = FloodFillTolerance,//0.04
			MaskBlurFactor = MaskBlurFactor//5
		};
		var texture = filter.Apply(mat);
		muskImage.texture = filter.SetMask();
		muskImage2.texture = filter.SetMask2();
		return OpenCvSharp.Unity.MatToTexture(texture);


		//Cv2.Threshold(dst, dst, 1, 255, ThresholdTypes.Binary);
	}

	//public void Scanner(Mat material)
	//{
	//	scanner.Settings.NoiseReduction = 0.7;//0.7;
	//	scanner.Settings.EdgesTight = 0.9;//0.9;
	//	scanner.Settings.ExpectedArea = 0.2;// 0.2;
	//	scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.Grayscale;

	//	scanner.Input = material;

	//	scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.HueGrayscale;
	//	outputImage.texture = OpenCvSharp.Unity.MatToTexture(scanner.Output);
	//}

	//public Texture2D Init(Texture2D texture)
	//{
	//	Scanner(OpenCvSharp.Unity.TextureToMat(texture));
	//	return outputImage.texture as Texture2D;
	//}
}