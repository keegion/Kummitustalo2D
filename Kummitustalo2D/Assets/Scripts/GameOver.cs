using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    private GameObject GameManager;

    void Start () {
		GameManager = GameObject.Find("GameManager(Clone)");
        // reset all player stuff
		Destroy(GameManager);
    }
	
	void Update () {
		//if (Input.anyKey) {
  //          SceneManager.LoadScene("MainMenu");
		//}
	}

	public void MainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
	public void Exit()
	{
		Application.Quit();
	}
}
