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
    [HideInInspector]
	public Text[] PlayersWon;
	public Transform PlayerNameContainer;

	void Start()
    {
	    retryButton?.onClick.AddListener(Retry);
	    mainMenuButton?.onClick.AddListener(MainMenu);
		PlayersWon = PlayerNameContainer.GetComponentsInChildren<Text>();

	    for (int i = 0; i < PlayersWon.Length; i++)
	    {
			//PlayersWon[i] = PlayerNameContainer.GetChild(i).GetComponent<Text>();
			PlayersWon[i].gameObject.SetActive(false);
	    }

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
