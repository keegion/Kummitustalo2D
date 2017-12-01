using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Loadlvl (string level)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(level);
       
    }
    public void Quit()
    {
        Application.Quit();
    }
}
