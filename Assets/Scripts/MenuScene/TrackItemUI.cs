using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class TrackItemUI : MonoBehaviour
{
	public Button Button;
	public MainMenu MainMenu;
	public Outline Outline;
	public TrackMenuManager trackMenuManager;

	public string TrackName;

	public void Start()
	{
		Button.onClick.AddListener(delegate { OnCooseTrack(); });
	}

	public void OnCooseTrack()
	{
		MainMenu.ApplicationMain.CurrentTrackName = TrackName;
		trackMenuManager.DisableAllOutlines();
		trackMenuManager.EnableItemOutline(this);
		MainMenu.StartARaceButton.gameObject.SetActive(true);
	}
}
