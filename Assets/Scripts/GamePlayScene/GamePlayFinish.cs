using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayFinish : MonoBehaviour
{
	public Button retryButton;
	public Button mainMenuButton;
	public GameObject Root;
	public Text PlayerWon;

	void Start()
    {
	    retryButton?.onClick.AddListener(Retry);
	    mainMenuButton?.onClick.AddListener(MainMenu);

	    Root.SetActive(false);
    }

    private void Retry()
    {
	    Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void MainMenu()
    {
	    Time.timeScale = 1;
		SceneManager.LoadScene("menu");
	}
}
