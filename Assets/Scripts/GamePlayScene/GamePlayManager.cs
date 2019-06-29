using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
	public List<Texture2D> cars = new List<Texture2D>();
	public RaceCarScript[] carObject;

	void Start()
	{
		if (CarsIsEmpty())
			return;

		cars = ApplicationMain.Instance.cars;

		Debug.Log("cars count = " + cars.Count);

		DisableAllCars();

		for (int i = 0; i < cars.Count; i++)
		{
			carObject[i].gameObject.SetActive(true);

			//if (cars[i].width >= 208 && cars[i].height >= 128)
			//{
			//	var withCoeff = cars[i].width / 208;
			//	var heightCoeff = cars[i].height / 128;

			//	TextureScale.Bilinear(cars[i], cars[i].width / withCoeff, cars[i].height / heightCoeff);
			//}
			//else
			//{
			//	TextureScale.Bilinear(cars[i], 208, 128);
			//}
	
			carObject[i].CarSpriteRenderer.sprite =
				Sprite.Create(cars[i], new Rect(0, 0, cars[i].width, cars[i].height), new Vector2(0.5f, 0.5f));

			carObject[i].CarSpriteRenderer.sprite.name = cars[i].name + "_sprite";
			carObject[i].CarSpriteRenderer.material.mainTexture = cars[i] as Texture;
			carObject[i].CarSpriteRenderer.material.shader = Shader.Find("Sprites/Default");
			carObject[i].gameObject.AddComponent<PolygonCollider2D>();
			carObject[i].PlayerName = ApplicationMain.Instance.makePhotoButtonData[i].PlayerNumber.text;
		}
	}

	private void DisableAllCars()
	{
		if (CarsIsEmpty())
			return;

		for (int i = 0; i < carObject.Length; i++)
		{
			carObject[i].gameObject.SetActive(false);
		}
	}

	private bool CarsIsEmpty()
	{
		return carObject == null || carObject.Length == 0 ? true : false;
	}
}
