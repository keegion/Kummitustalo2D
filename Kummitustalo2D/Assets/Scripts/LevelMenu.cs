using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelMenu : MonoBehaviour {
    public GameObject levelPanel,namePanel;
    public GameObject hiscorePanel;
    public Button lvl2;
    public Button lvl3;
    public Button lvl4;
    public Button global;
    public Button personal;
    bool syncDone;
    public Text data,name;
    public InputField inputName;
    string[] items;
    string[] names;
    float[] times;
    

    
    public GameObject loadIcon;
    // Use this for initialization
    void Start () {
        CheckifLevelCleared();
        loadPersonalData(1);
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
        loadPersonalData(1);
        hiscorePanel.SetActive(true);
        personal.interactable = false;
        global.interactable = true;
    }
    public void closeHiscorePanel()
    {
        hiscorePanel.SetActive(false);
        personal.interactable = true;
       
    }
    public void openGlobalPanel()
    {
        SyncDB();
        StartCoroutine(Load());
        StartCoroutine(ConnectToDatabase(1));
        global.interactable = false;
        personal.interactable = true;
    }
    public void openPersonalPanel()
    {
        loadPersonalData(1);
        personal.interactable = false;
        global.interactable = true;
    }
   
    public void GlobalButton(int button)
    {
        if(global.interactable == false)
            StartCoroutine(ConnectToDatabase(button));
        if (personal.interactable == false)
            loadPersonalData(button);

    }
    public void EnterName()
    {
        if (inputName.text =="" || inputName.text.Length>10)
        {
            name.text = "Invalid name, try again, Sire.";
        }
        else
        {
            PlayerPrefs.SetString("name", inputName.text);
            namePanel.SetActive(false);
            SyncDB();
        }
    }
    public void TotalHS()
    {
        if (global.interactable == false)
        {
            StartCoroutine(ConnectToDatabase(5));
        }
            
        else if (personal.interactable == false)
        {
            float totalTime = PlayerPrefs.GetFloat("totalTime");
            if (totalTime == 0)
                data.text = "You have not cleared all levels yet!";
            else
               data.text = "Your total clear time is : " + "\r\n" + "\r\n" + totalTime.ToString("n2")+ " s";
        }
            

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
    public void loadPersonalData(int nmbr)
    {
        if (PlayerPrefs.GetFloat("level_0"+nmbr) != 0)
        {
            
            data.text = "Level " + nmbr + " best clear time: " + "\r\n" + "\r\n";
            data.text += PlayerPrefs.GetFloat("level_0"+nmbr).ToString("n2") + " s";
        }
        else
        {
            data.text = "Level has not been cleared yet";
        }


    }
    private IEnumerator ConnectToDatabase(int nmbr)
    {
        WWW itemsData = new WWW("http://mihkeltanel.com/brokenmomoirs/top"+ nmbr + ".php");

        
        while (!itemsData.isDone)
        {

           loadIcon.SetActive(true);
           loadIcon.transform.Rotate(new Vector3(0, 0, Time.deltaTime * -150));
           yield return new WaitForSeconds(0.01f);

        }
        if (!string.IsNullOrEmpty(itemsData.error))
        {
            Debug.Log(itemsData.error);
            loadIcon.SetActive(false);
        }
        else
            yield return new WaitForSeconds(0.51f);
        {
           loadIcon.SetActive(false);

            string itemsDataString = itemsData.text;
            items = itemsDataString.Split(';');
            names = new string[items.Length];
            times = new float[items.Length];
                for(int i = 0; i < items.Length-1; i++)
            {
                names[i] = GetDataValue(items[i], "name:");
                times[i] = float.Parse(GetDataValue(items[i], "time:"));
            }

            bool didSwap;
            do
            {
                didSwap = false;
                for (int i = 0; i < times.Length-2; i++)
                {
                    if (times[i] > times[i + 1])
                    {
                        float tempFloat = times[i + 1];
                        string tempString = names[i + 1];
                        times[i + 1] = times[i];
                        names[i + 1] = names[i];
                        times[i] = tempFloat;
                        names[i] = tempString;
                        didSwap = true;
                    }
                }
            } while (didSwap);
            if (nmbr == 5)
            {
                data.text = "Global total best times : " + "\r\n" + "\r\n";
            }
            else
            {
                data.text = "Global level " + nmbr + " best times : " + "\r\n" + "\r\n";
            }
            int totalLvls = times.Length - 1;
            if (totalLvls > 10)
                totalLvls = 10;
            for (int i = 0; i < totalLvls; i++)
            {
                
                if(i==9)
                data.text += i + 1 + ". ";
                else
                data.text += i+1 +".   ";
                data.text += names[i] + " - ";
                data.text += times[i].ToString("n2") +" s" + "\r\n";

            }
           
            

        }



    }
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    void SyncDB ()
    {
        if(PlayerPrefs.GetString("name")=="")
        {
            namePanel.SetActive(true);
            return;
        }
        for (int i = 1; i < 6; i++)
        {
            if (i == 5 && PlayerPrefs.GetFloat("totalTime") != PlayerPrefs.GetFloat("atotalTime"))
            {
                StartCoroutine(AddDataToDB(PlayerPrefs.GetFloat("totalTime"), PlayerPrefs.GetString("name"), i));
            }
            else if(PlayerPrefs.GetFloat("level_0"+i) != PlayerPrefs.GetFloat("alevel_0"+i))
            {
                StartCoroutine(AddDataToDB(PlayerPrefs.GetFloat("level_0" + i),PlayerPrefs.GetString("name"),i));
                
            }
        }
        syncDone = true;
    }

    IEnumerator AddDataToDB(float enteredTime, string name, int lvl)
    {
        string username = "DezetoGod";
        username = name;
        WWWForm form = new WWWForm();
        form.AddField("nicknamePost", username);
        form.AddField("timePost", enteredTime.ToString());
        form.AddField("lvl", lvl.ToString());
        WWW www = new WWW("http://mihkeltanel.com/brokenmomoirs/addData.php", form);
        while (!www.isDone)
        {

            yield return new WaitForSeconds(0.01f);

        }

        if (!string.IsNullOrEmpty(www.error))
        {
           
            Debug.Log(www.error);
        }
        else
        {
     
            Debug.Log(username + " sent to DB");
            if (lvl == 5)
                PlayerPrefs.SetFloat("atotalTime", PlayerPrefs.GetFloat("totalTime"));
            else
                PlayerPrefs.SetFloat("alevel_0" + lvl, PlayerPrefs.GetFloat("level_0" + lvl));
        
        }
    }
    IEnumerator Load()
    {
        while (!syncDone)
        {

            loadIcon.SetActive(true);
            loadIcon.transform.Rotate(new Vector3(0, 0, Time.deltaTime * -150));
            yield return new WaitForSeconds(0.01f);

        }
        loadIcon.SetActive(false);
        syncDone = false;
    }

}


