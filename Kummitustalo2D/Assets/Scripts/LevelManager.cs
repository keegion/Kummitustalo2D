using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public GameObject pauseMenu;
    public Text leveltxt;
	// Use this for initialization
	void Start () {
		// Tehty omana skriptinään erilliseen canvakseen (levelin nimi omana graffana per leveli...)
       //leveltxt.text = SceneManager.GetActiveScene().name;
        //StartCoroutine(TextCD());
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
	}
    public void Resume ()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }
    public void Exit()
    {
        Application.Quit();
    }
    IEnumerator TextCD ()
    {
        yield return new WaitForSeconds(1f);
        leveltxt.text = "";
    }
}
