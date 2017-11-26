using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour {
    public GameObject levelPanel;
    public GameObject hiscorePanel;
    public GameObject personalPanel;
    public GameObject globalPanel;
    public Button lvl2;
    public Button lvl3;
    public Button lvl4;
    public Button global;
    public Button personal;
    public Text personal1;
    public Text personal2;
    public Text personal3;
    public Text personal4;
    public Text global1;
    public Text global2;
    public Text global3;
    public Text global4;

    // Use this for initialization
    void Start () {
        CheckifLevelCleared();
        loadPersonalData();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OpenLevelMenu()
    {
        levelPanel.SetActive(true);
    }
    public void OpenHiscorePanel()
    {
        loadPersonalData();
        hiscorePanel.SetActive(true);
        personal.interactable = false;
        global.interactable = true;
        personalPanel.SetActive(true);
    }
    public void closeHiscorePanel()
    {
        hiscorePanel.SetActive(false);
        personal.interactable = true;
        personalPanel.SetActive(false);
        globalPanel.SetActive(false);
    }
    public void openGlobalPanel()
    {
        personalPanel.SetActive(false);
        globalPanel.SetActive(true);
        global.interactable = false;
        personal.interactable = true;
    }
    public void openPersonalPanel()
    {
        loadPersonalData();
        globalPanel.SetActive(false);
        personalPanel.SetActive(true);
        personal.interactable = false;
        global.interactable = true;
    }
    public void CheckifLevelCleared()
    {

        if (PlayerPrefs.GetInt("key_level_02") == 1)
        {
            lvl2.interactable = true;
        }
        if (PlayerPrefs.GetInt("key_level_03") == 1)
        {
            lvl3.interactable = true;
        }
        if (PlayerPrefs.GetInt("key_level_04") == 1)
        {
            lvl4.interactable = true;
        }
    }
    void loadPersonalData()
    {
        if (PlayerPrefs.GetFloat("level_01") != 0)
        {
            personal1.text = PlayerPrefs.GetFloat("level_01").ToString("n2") + " s";
        }
        if (PlayerPrefs.GetFloat("level_02") != 0)
        {
            personal2.text = PlayerPrefs.GetFloat("level_02").ToString("n2") + " s";
        }
        if (PlayerPrefs.GetFloat("level_03") != 0)
        {
            personal3.text = PlayerPrefs.GetFloat("level_03").ToString("n2") + " s";
        }
        if (PlayerPrefs.GetFloat("level_04") != 0)
        {
            personal4.text = PlayerPrefs.GetFloat("level_04").ToString("n2") + " s";
        }

    }
}
