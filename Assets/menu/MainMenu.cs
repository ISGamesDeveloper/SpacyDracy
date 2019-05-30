using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public float factorValue = 3.0f;

	public AudioSource mainAudioSource;
	public GameObject MainWindow;
	public GameObject SinglePlayerWindow;
	public GameObject MultiPlayerWindow;
	public GameObject TrackMenuWindow;
	public GameObject SettingsMenuWindow;

	public void PlayGame()
	{
		StartCoroutine(this.fade());
	}

	public void MainMenuButton()
	{
		DisabledAllWindows();
		MainWindow.SetActive(true);
	}

	public void SinglePlayerButton()
	{
		DisabledAllWindows();
		SinglePlayerWindow.SetActive(true);
	}

	public void MultiPlayerButton()
	{
		DisabledAllWindows();
		MultiPlayerWindow.SetActive(true);
	}

	public void SettingsButton()
	{
		DisabledAllWindows();
		SettingsMenuWindow.SetActive(true);
	}

	public void TrackButton()
	{
		DisabledAllWindows();
		TrackMenuWindow.SetActive(true);
	}

	public void QuitGame()
	{
		Debug.Log("Quit!");
		Application.Quit();
	}

	private void DisabledAllWindows()
	{
		MainWindow.SetActive(false);
		SinglePlayerWindow.SetActive(false);
		//MultiPlayerWindow.SetActive(false);
		TrackMenuWindow.SetActive(false);
		SettingsMenuWindow.SetActive(false);
	}

	public IEnumerator fade()
	{
		while (this.mainAudioSource.volume > 0.0f)
		{
			this.mainAudioSource.volume -= Time.deltaTime * this.factorValue;
			yield return new WaitForSeconds(0.1f);
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
