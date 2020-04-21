using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MakePhotoButtonData : MonoBehaviour
{
	public RaceCarScript RaceCar;
	public RawImage carUIImage;
	public RawImage RocketImage;
	public Text PlayerText;
	public Text TakePhotoText;
	public Button button;
	public Button DeleteItemButton;
	public int myIndex;
	public bool hasTexture;
	public bool hasDefaultTexture;
	private ApplicationMain applicationMain;
	private MakePhotoButtonData _makePhotoButtonData;

	public void Init(int index,  MakePhotoButtonData makePhotoButtonData)
	{
		applicationMain = ApplicationMain.Instance;
		_makePhotoButtonData = makePhotoButtonData;
		myIndex = index;
		transform.SetSiblingIndex(index);

		Debug.Log("makePhotoButtonData.carObject: " + makePhotoButtonData.RaceCar == null);
		if (makePhotoButtonData.RaceCar != null && !string.IsNullOrEmpty(makePhotoButtonData.RaceCar.PlayerName))
		{
			PlayerText.text = makePhotoButtonData.RaceCar.PlayerName;
		}

		button.onClick.AddListener(MakePhoto);
		DeleteItemButton.onClick.AddListener(DeleteItem);
		hasDefaultTexture = true;
		RaceCar = new RaceCarScript();
	}

	public void MakePhoto()
	{
		applicationMain.CurrentMenuState = "Photo";
		applicationMain.CurrentCarIndex = myIndex;
		applicationMain.CurrentCarHasTexture = hasTexture;
		//при создании игрока передаю его имя и цвет, что бы в листах поставить true
		applicationMain.CurrentSubstanceName = _makePhotoButtonData.RaceCar.SubstanceName;
		applicationMain.CurrentCarColor = _makePhotoButtonData.carUIImage.color;
		//

		SceneManager.LoadScene("Photo");
	}

	public void DeleteItem()
	{
		//при удалении игрока передаю его имя и цвет, что бы в листах поставить true
		applicationMain.CurrentSubstanceName = _makePhotoButtonData.RaceCar.SubstanceName;
		applicationMain.CurrentCarColor = _makePhotoButtonData.RaceCar.CarColor;
		//
		Debug.Log("MyIndex: " + myIndex);
		if (ApplicationMain.RaceCars.Count > myIndex)
		{
			ApplicationMain.RaceCars.RemoveAt(myIndex);
		}

		ApplicationMain.makePhotoButtonData.RemoveAt(myIndex);

		applicationMain.MainMenu.UpdatePlayerData();

		Destroy(gameObject);
	}
}
