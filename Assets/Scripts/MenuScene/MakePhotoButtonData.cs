using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MakePhotoButtonData : MonoBehaviour
{
	public RawImage CurrentCar;
	public Text PlayerNumber;
	public Text TakePhotoText;
	public Button button;
	public Button DeleteItemButton;
	public int myIndex;

	private MakePhotoButtonData _makePhotoButtonData;

	public void Init(int index,  MakePhotoButtonData makePhotoButtonData)
	{
		_makePhotoButtonData = makePhotoButtonData;
		myIndex = index;
		transform.SetSiblingIndex(index);
		PlayerNumber.text = "Player " + (index + 1);
		TakePhotoText.text = "";
		button.onClick.AddListener(MakePhoto);
		DeleteItemButton.onClick.AddListener(DeleteItem);
	}

	public void MakePhoto()
	{
		ApplicationMain.Instance.CurrentMenuState = "Photo";
		ApplicationMain.Instance.CurrentCarIndex = myIndex;
		Debug.Log("myIndex: " + myIndex);
		SceneManager.LoadScene("Photo");
	}

	public void DeleteItem()
	{
		Debug.Log("DeleteItem");

		ApplicationMain.Instance.makePhotoButtonData.Remove(_makePhotoButtonData);
		ApplicationMain.Instance.cars.Remove(CurrentCar.texture as Texture2D);
		ApplicationMain.Instance.MainMenu.RecalculateItemNumbers();

		Destroy(gameObject);
	}
}
