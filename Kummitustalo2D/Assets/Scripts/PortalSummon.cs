using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalSummon : MonoBehaviour {
    public int MaxShards;
    //private GameObject gm;
    public GameObject portal;
    public int levels;
    public GameObject FinishMenu;
    Player player;
    int currentLevel;
    float clearTime;
    string sceneName;
    int cleared;
    float totalTime;
    float currentBest;

    // Use this for initialization
    void Start () {
        //gm = GameObject.Find("GameManager(Clone)");
        player = GetComponent<Player>();
        sceneName = SceneManager.GetActiveScene().name;
        currentBest = PlayerPrefs.GetFloat(sceneName,currentBest);
    }
	
	// Update is called once per frame
	void Update () {

        // tarkistaa onko pelaaja kosketuksessa portaaliin ja jos painaa nuolinäppäintä ylös tai w näppäintä aloittaa uuden tason.
        if (player.portalSummoned)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                //tähän seuraava taso(väliaikaisesti kuolee eli aloittaa tason uudelleen)
                currentLevel = checkCurrentSceneLevel();
                SceneManager.LoadScene("level_0"+ currentLevel);
            }

        }
    }

    //Tarkastaa onko pelaajalla tarvittava shardimäärä, jos on niin luo portalin pelaajan koordinaatteille.
    public void CheckIfSummonPortal(int currentShards)
    {


        if (MaxShards <= currentShards)
        {
            CheckHiscores();
            
            if (CheckifLastLevel())
            {
                OpenFinishPanel();
                TotalHiscore();

            }
            else
            {
                Instantiate(portal, transform.position, transform.rotation);
            }
            

            TotalHiscore();
        }
    }
    public bool CheckifLastLevel()
    {
       
        if (sceneName == "level_0" + levels)
            return true;
            
      
        else
            return false;
    }
    void OpenFinishPanel()
    {
        FinishMenu.SetActive(true);
        Time.timeScale = 0;

    }
    int checkCurrentSceneLevel()
    {
        int currentlvl = 0;

        for(int i = 1; i < levels; i++)
        {
            if (sceneName == "level_0" + i)
                currentlvl = i+1;
        }

        return currentlvl;
    }

    //check if current scene cleartime is better than saved hiscore, if it is or is 0, save new score to playerprefs.
    void CheckHiscores ()
   
    {
        
        clearTime = player.time;
        PlayerPrefs.SetInt("level_0"+ checkCurrentSceneLevel(), 1);
        Debug.Log("level clear time : " + clearTime);
        Debug.Log("current saved time: " + currentBest);
        if (currentBest > clearTime)
        {
            PlayerPrefs.SetFloat(sceneName, clearTime);
            Debug.Log("Saved time: " + PlayerPrefs.GetFloat(sceneName));
            PlayerPrefs.Save();
        }
        else  if (currentBest == 0)
        {
            PlayerPrefs.SetFloat(sceneName, clearTime);
            Debug.Log("0, Saved time: " + PlayerPrefs.GetFloat(sceneName));
            PlayerPrefs.Save();

        }
        
        clearTime = 0;

    }
    void TotalHiscore()
    {
        
        //check if all levels cleared
        for(int i = 1; i < levels; i++)
        {
            if (PlayerPrefs.GetInt("level0_"+i) == 1)
                cleared++;
        }
        //if all levels cleared calculate total time from all levels and add it to playerprefs
        if(cleared == levels)
        {
            for(int i = 1; 1 < levels; i++)
            {
                totalTime += PlayerPrefs.GetFloat("level0_" + i);
            }
            PlayerPrefs.SetFloat("totalTime", totalTime);
            Debug.Log("totaltime saved" + totalTime);
          
            totalTime = 0;
            cleared = 0;
            PlayerPrefs.Save();
        }
    }



}
