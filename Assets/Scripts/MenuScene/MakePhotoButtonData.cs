using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MakePhotoButtonData : MonoBehaviour
{
	public RaceCarScript RaceCar;
	public RawImage carUIImage;
	public Text PlayerNumber;
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
		if (makePhotoButtonData.RaceCar != null)
		{
			PlayerNumber.text = makePhotoButtonData.RaceCar.CarName;
		}

		TakePhotoText.text = "";
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
		Debug.Log("myIndex: " + myIndex);
		Debug.Log("hasTexture: " + hasTexture);
		SceneManager.LoadScene("Photo");
	}

	public void DeleteItem()
	{
		Debug.Log("DeleteItem");

		ApplicationMain.makePhotoButtonData.Remove(_makePhotoButtonData);
		ApplicationMain.RaceCars.Remove(RaceCar);

		applicationMain.MainMenu.RecalculateItemNumbers();

		Destroy(gameObject);
	}
}
