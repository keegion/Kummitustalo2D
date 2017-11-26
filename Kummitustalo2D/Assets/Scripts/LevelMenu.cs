using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
    string[] items1;
    string[] items2;
    string[] items3;
    string[] items4;
    bool loaded;
    public GameObject loadIcon;
    // Use this for initialization
    void Start () {
        CheckifLevelCleared();
        loadPersonalData();
        Time.timeScale = 1;

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
        if(!loaded)
        StartCoroutine(ConnectToDatabase());
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
    private IEnumerator ConnectToDatabase()
    {
        WWW itemsData1 = new WWW("http://mihkeltanel.com/brokenmomoirs/top1.php");
        WWW itemsData2 = new WWW("http://mihkeltanel.com/brokenmomoirs/top2.php");
        WWW itemsData3 = new WWW("http://mihkeltanel.com/brokenmomoirs/top3.php");
        WWW itemsData4 = new WWW("http://mihkeltanel.com/brokenmomoirs/top4.php");
        
        while (!itemsData1.isDone && !itemsData2.isDone && !itemsData3.isDone && !itemsData4.isDone)
        {

           // errorText.text = "";
           loadIcon.SetActive(true);
           loadIcon.transform.Rotate(new Vector3(0, 0, Time.deltaTime * -150));
           yield return new WaitForSeconds(0.01f);

        }
        if (!string.IsNullOrEmpty(itemsData1.error) && !string.IsNullOrEmpty(itemsData2.error) && !string.IsNullOrEmpty(itemsData3.error) && !string.IsNullOrEmpty(itemsData4.error))
        {

            // errorText.text = "Failed connecting to database.";
            //yield return new WaitForSeconds(3.14f);
            Debug.Log(itemsData1.error);
            loadIcon.SetActive(false);
        }
        else
            yield return new WaitForSeconds(0.51f);
        {
           loadIcon.SetActive(false);

            string itemsDataString1 = itemsData1.text;
            string itemsDataString2 = itemsData2.text;
            string itemsDataString3 = itemsData3.text;
            string itemsDataString4 = itemsData4.text;
            items1 = itemsDataString1.Split(';');
            items2 = itemsDataString2.Split(';');
            items3 = itemsDataString3.Split(';');
            items4 = itemsDataString4.Split(';');
            for (int i = 0; i <= items1.Length - 2; i++)
            {
                if(i==9)
                global1.text += i + 1 + ". ";
                else
                global1.text += i+1 +".   ";
                global1.text += GetDataValue(items1[i], "name:")+ " - ";
                float s = float.Parse(GetDataValue(items1[i], "time:"));
                global1.text += s.ToString("n2") +" s" + "\r\n";

            }
            for (int i = 0; i <= items2.Length - 2; i++)
            {
                if (i == 9)
                    global2.text += i + 1 + ". ";
                else
                global2.text += i + 1 + ".   ";
                global2.text += GetDataValue(items2[i], "name:") + " - ";
                float s = float.Parse(GetDataValue(items2[i], "time:"));
                global2.text += s.ToString("n2") + " s" + "\r\n";

            }
            for (int i = 0; i <= items3.Length - 2; i++)
            {
                if (i == 9)
                    global3.text += i + 1 + ". ";
                else
                global3.text += i + 1 + ".   ";
                global3.text += GetDataValue(items3[i], "name:") + " - ";
                float s = float.Parse(GetDataValue(items3[i], "time:"));
                global3.text += s.ToString("n2") + " s" + "\r\n";

            }
            for (int i = 0; i <= items4.Length - 2; i++)
            {
                if (i == 9)
                    global4.text += i + 1 + ". ";
                else
                global4.text += i + 1 + ".   ";
                global4.text += GetDataValue(items4[i], "name:") + " - ";
                float s = float.Parse(GetDataValue(items4[i], "time:"));
                global4.text += s.ToString("n2") + " s" + "\r\n";

            }
            loaded = true;

        }



    }
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }
}


