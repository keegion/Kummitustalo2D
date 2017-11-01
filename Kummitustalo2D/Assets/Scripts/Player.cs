﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float hp;
    public float maxHp;
	public int maxShards;
	public GUIStyle myGUIStyle;
    public Image healthMeter;
	public Transform memories;
	public GameObject GameManagerPrefab;
	private GameObject GameManager;
    Rigidbody2D rb;
    public bool OnStairs, portalSummoned = false;
    CharController charctr;

    Animator animator;

    PortalSummon summonPortal;
    
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
        charctr = GetComponent<CharController>();
        summonPortal = GetComponent<PortalSummon>();
		myGUIStyle.fontSize = 24;
		myGUIStyle.normal.textColor = Color.white;
		// create GameManager if one doesn't exist in scene
		GameManager = GameObject.Find("GameManager(Clone)");

		if (GameManager == null){
			GameManager = Instantiate(GameManagerPrefab);
		}

        animator = healthMeter.GetComponent<Animator>();
        animator.Play("HealthBar", 0, 0.99f);
        animator.speed = 0;

		//Debug.Log(memories.GetChild(0).Find("shards").GetChild(0));
		//Transform[] shards;
    }
	
	// Update is called once per frame
	void Update () {
        CheckHP();
	}

    void CheckHP()
    {
        if (hp <= 0)
            Die();
    }

    public void Die()
    {
		if (GameManager.GetComponent<GameManager>().livesLeft < 1)
		{
			Debug.Log("Game over man");
			SceneManager.LoadScene("Test_start_scene", LoadSceneMode.Single);
		} else {
			GameManager.GetComponent<GameManager>().livesLeft--;
			Debug.Log("You have died");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
    }

	private void AddShard()
	{
		int shardCount = GameManager.GetComponent<GameManager>().shards;
		if (shardCount < maxShards)
		{
			memories.GetChild(0).Find("shards").GetChild(shardCount).gameObject.SetActive(false);
		}

		GameManager.GetComponent<GameManager>().shards++;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="EnemyBullet")
        {
            //Debug.Log("Health" + hp/maxHp);
            hp -= 1;
            float roundedHealth = hp / maxHp;
            //Debug.Log("rounded: " + roundedHealth);
            animator.Play("HealthBar", -1, roundedHealth);
            animator.speed = 0;
        }
        if(collision.tag =="Muistisiru")
        {
			AddShard();

            summonPortal.CheckIfSummonPortal(GameManager.GetComponent<GameManager>().shards);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Tikkaat" && charctr.isGrounded)
        {
            rb.isKinematic = true;
            OnStairs = true;
        }
        if (collision.tag =="Portal")
        {
            portalSummoned = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Tikkaat")
        {
            rb.isKinematic = false;
            OnStairs = false;
        }
        if (collision.tag == "Portal")
        {
            portalSummoned = false;
        }
    }
   
    void OnGUI()
	{
		GUI.Label(new Rect(85, 30, 100, 30), "Health: " + hp, myGUIStyle);
		GUI.Label(new Rect(275, 30, 40, 30), "x " + GameManager.GetComponent<GameManager>().livesLeft, myGUIStyle);
        GUI.Label(new Rect(420, 30, 40, 30), "x " + GameManager.GetComponent<GameManager>().shards, myGUIStyle);
    }
}
