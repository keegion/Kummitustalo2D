using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    private GameObject GameManager;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
            GameManager = GameObject.Find("GameManager(Clone)");
            GameManager.GetComponent<GameManager>().livesLeft = GameManager.GetComponent<GameManager>().lives;
            SceneManager.LoadScene("Main");
		}
	}
}
