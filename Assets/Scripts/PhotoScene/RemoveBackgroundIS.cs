using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveBackgroundIS : MonoBehaviour
{
	//public RawImage outputImage;

	public Text PlayerName;
	public GameObject LoadingWindow;
	public string ColorName;
	public string SubstanceName;
	public string AdjectiveName;
	public Texture2D ProcessedTexture;
	public bool Processed;

 //   public void SetImageToRocket(Texture2D texture)
 //   {
	//	outputImage.texture = texture;
	//}

	//public Texture2D RemoveBackgroundOnTexture(Texture2D texture, Action<bool> hasTexture)
	//{
	//	LoadingWindow.SetActive(false);

	//	var hasColor = CheckTexture(texture);

	//	//Если меньше 1000 закрашенных пикселей, то посылаем событие false
	//	if (!hasColor)
	//	{
	//		hasTexture.Invoke(false);
	//		return null;
	//	}
 //       else
 //       {
	//		hasTexture.Invoke(true);
	//	}

	//	//Debug.Log("sizeDelta.x: " + outputImage.rectTransform.sizeDelta.x);
	//	//Debug.Log("sizeDelta.y: " + outputImage.rectTransform.sizeDelta.y);

	//	RemoveAlphaBorder(texture);// после удовлетворительного фото (нажатия на галочку) удалить лишнюю пустоту
	//	var transform = outputImage.rectTransform;
	//	ChangeRawImageDelta(texture, transform);
	//	outputImage.texture = texture;

	//	ProcessedTexture = outputImage.texture as Texture2D;
	//	PlayerName.enabled = true;
	//	Processed = true;

	//	return ProcessedTexture;
	//}

	//private void ChangeRawImageDelta(Texture texture, RectTransform transform)
	//{
	//	Debug.Log("texture.width: " + texture.width + "    texture.height: " + texture.height);
	//	Vector2 vector;

 //       float coeff;

 //       if (texture.width > texture.height)
	//	{
	//		coeff = texture.width / 600.0f;
	//		Debug.Log("texture.width / 600: " + coeff);
	//		vector.x = texture.width / coeff;
	//		vector.y = texture.height / coeff;


	//	}
	//	else
	//	{
	//		coeff = texture.height / 600.0f;
	//		Debug.Log("texture.height / 600.0f: " + coeff);
	//		vector.y = 420.0f;

	//		vector.x = texture.width / coeff;
	//	}

	//	outputImage.rectTransform.sizeDelta = vector;
	//}

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

		var width = xPoints[xPoints.Count - 1] - xPoints[0];
		var height = yPoints[yPoints.Count - 1] - yPoints[0];

		Color[] pix = texture.GetPixels(xPoints[xPoints.Count - 1] - width, yPoints[yPoints.Count - 1] - height, width, height);

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

		Debug.Log("colorPixelsCount: " + colorPixelsCount);
		return colorPixelsCount >= minPixelCount;
	}
}