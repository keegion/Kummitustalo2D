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

    // Use this for initialization
    void Start () {
        //gm = GameObject.Find("GameManager(Clone)");
    }
	
	// Update is called once per frame
	void Update () {
		
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
       
        if (SceneManager.GetActiveScene().name == "level_" + levels)
            return true;
            
      
        else
            return false;
    }
    void OpenFinishPanel()
    {
        FinishMenu.SetActive(true);
        Time.timeScale = 0;

    }



}
