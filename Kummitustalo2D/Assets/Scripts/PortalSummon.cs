using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSummon : MonoBehaviour {
    public int MaxShards;
    private GameObject gm;
    public GameObject portal;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager(Clone)");

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
            Instantiate(portal, transform.position, transform.rotation);
        }
    }


}
