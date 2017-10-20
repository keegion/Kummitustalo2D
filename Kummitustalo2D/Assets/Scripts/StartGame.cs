using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    private GameObject GameManager;

    void Start () {
        // reset all player stuff
        Destroy(GameObject.Find("GameManager(Clone)"));
    }
	
	void Update () {
		if (Input.anyKey) {
            GameManager = GameObject.Find("GameManager(Clone)");
            //GameManager.GetComponent<GameManager>().livesLeft = GameManager.GetComponent<GameManager>().lives;
            SceneManager.LoadScene("Main");
		}
	}
}
