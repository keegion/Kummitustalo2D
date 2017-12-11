using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    private GameObject GameManager;

    void Start () {
		GameManager = GameObject.Find("GameManager(Clone)");
        // reset all player stuff
		Destroy(GameManager);
    }
	
	void Update () {
		if (Input.anyKey) {
            //GameManager.GetComponent<GameManager>().livesLeft = GameManager.GetComponent<GameManager>().lives;
            SceneManager.LoadScene("MainMenu");
		}
	}
}
