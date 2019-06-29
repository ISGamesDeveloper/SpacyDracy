using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class TrackItemUI : MonoBehaviour
{
	public Button Button;
	public MainMenu MainMenu;

	public string TrackName;

	public void Start()
	{
		Button.onClick.AddListener(delegate { OnCooseTrack(); });
	}

	public void OnCooseTrack()
	{
		MainMenu.ApplicationMain.CurrentTrackName = TrackName;
		MainMenu.TrackMenuWindow.SetActive(false);
	}
}
