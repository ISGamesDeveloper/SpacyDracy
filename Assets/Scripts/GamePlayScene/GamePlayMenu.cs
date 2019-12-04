using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayMenu : MonoBehaviour
{
	public Button playButton;
	public Button pauseButton;
	public Button retryButton;
	public Button mainMenuButton;

	public GameObject Root;

	void Start()
    {
	    playButton?.onClick.AddListener(Play);
		pauseButton?.onClick.AddListener(Pause);
		retryButton?.onClick.AddListener(Retry);
	    mainMenuButton?.onClick.AddListener(MainMenu);

	    Root.SetActive(false);
    }

    private void Retry()
    {
	    Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Play()
    {
	    Time.timeScale = 1;
	    Root.SetActive(false);
	}

    private void Pause()
    {
	    Time.timeScale = 0;
	    Root.SetActive(true);
    }

    private void MainMenu()
    {
	    Time.timeScale = 1;
		SceneManager.LoadScene("menu");
	}
}
