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

    // Use this for initialization
    void Start () {
        //gm = GameObject.Find("GameManager(Clone)");
        player = GetComponent<Player>();
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
                SceneManager.LoadScene("level_0"+ currentLevel+1);
            }

        }
    }

    //Tarkastaa onko pelaajalla tarvittava shardimäärä, jos on niin luo portalin pelaajan koordinaatteille.
    public void CheckIfSummonPortal(int currentShards)
    {


        if (MaxShards <= currentShards)
        {

            Debug.Log("Summon Portal");
            if(CheckifLastLevel())
            {
                OpenFinishPanel();
            }
            else
            Instantiate(portal, transform.position, transform.rotation);
           
        }
    }
    public bool CheckifLastLevel()
    {
       
        if (SceneManager.GetActiveScene().name == "level_0" + levels)
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

        for(int i = 0; i > levels; i++)
        {
            if (SceneManager.GetActiveScene().name == "level_0" + i)
                currentlvl = i;
        }

        return currentlvl;
    }



}
