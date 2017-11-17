using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public float hp;
	public float maxHp;
	public GUIStyle myGUIStyle;
	public Image healthMeter;
	public Transform memories, playerSpawnPoint;
	GameObject[] shardArray = new GameObject[15];
	public GameObject GameManagerPrefab;
	private GameObject GameManager;
	Rigidbody2D rb;
	public bool OnStairs, portalSummoned,dmgOnCD;
	CharController charctr;
    public GameObject dmgText;

	Animator animator;

	PortalSummon summonPortal;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		charctr = GetComponent<CharController>();
		summonPortal = GetComponent<PortalSummon>();
		myGUIStyle.fontSize = 24;
		myGUIStyle.normal.textColor = Color.white;

		// create GameManager if one doesn't exist in scene
		//GameManager = GameObject.Find("GameManager(Clone)");
		//if (GameManager == null)
		if (!FindObjectOfType(typeof(GameManager)))
		{
			GameManager = Instantiate(GameManagerPrefab);
		}

		animator = healthMeter.GetComponent<Animator>();
		ResetHealthBarAnimation();
		animator.speed = 0;
		shardArray = GameObject.FindGameObjectsWithTag("shardInUI");
		// Sortataan array nimien mukaan, koska FindGameObjectsWithTag palauttaa gameobjectit "random" järjestyksessä
		Array.Sort(shardArray, CompareObNames);
		UpdateShardsOnStart();
	}

	int CompareObNames(GameObject a, GameObject b)
	{
		return string.Compare(a.name, b.name, StringComparison.CurrentCulture);
	}

	// Update is called once per frame
	void Update()
	{
		CheckHP();
	}

	void CheckHP()
	{
		if (hp <= 0)
			Die();
	}

	void ResetHealthBarAnimation ()
	{
		animator.Play("HealthBar", 0, 0.99f);
	}

	public void Die()
	{
		if (GameManager.GetComponent<GameManager>().livesLeft < 1)
		{
			Debug.Log("Game over man");
			SceneManager.LoadScene("Test_start_scene", LoadSceneMode.Single);
		}
		else
		{
			GameManager.GetComponent<GameManager>().livesLeft--;
			Debug.Log("You have died");
			if (playerSpawnPoint)
			{
				transform.position = playerSpawnPoint.position;
			} 
			else 
			{
				// fixed fallback if spawn point not added to scene
				transform.position = new Vector3(1f, 1.5f, 0);
			}
			hp = maxHp;

			ResetHealthBarAnimation();
			//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	private void UpdateShardsOnStart(){
		int shardCount = GameManager.GetComponent<GameManager>().shards;
		for (int i = 0; i < shardCount && i < shardArray.Length; i++)
		{
			shardArray[i].SetActive(false);
		}
	}

	private void AddShard()
	{
		int shardCount = GameManager.GetComponent<GameManager>().shards;
		if (shardCount < shardArray.Length)
		{
			shardArray[shardCount].SetActive(false);
		}
		GameManager.GetComponent<GameManager>().shards++;
	}
    IEnumerator RunningSkeleCD()
    {
        yield return new WaitForSeconds(1f);
        dmgOnCD = false;
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag == "EnemyBullet")
		{
			hp -= 1;
			float roundedHealth = hp / maxHp;
			animator.Play("HealthBar", -1, roundedHealth);
			animator.speed = 0;
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="EnemyBullet")
        {
            //Debug.Log("Health" + hp/maxHp);
            hp -= 1;
            addDmgText();
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!dmgOnCD && collision.gameObject.name == "RunningSkele")
        {
            dmgOnCD = true;
            hp -= 1;
            StartCoroutine(RunningSkeleCD());
            addDmgText();
        }
    }
    void addDmgText()
    {
        GameObject temp = (GameObject)Instantiate(dmgText, transform.position, transform.rotation);
        Destroy(temp, 0.5f);
    }
    void OnGUI()
	{
		GUI.Label(new Rect(215, 30, 100, 30), "Health: " + hp, myGUIStyle);
		GUI.Label(new Rect(375, 30, 100, 30), "Lives: " + GameManager.GetComponent<GameManager>().livesLeft, myGUIStyle);
		GUI.Label(new Rect(520, 30, 100, 30), "Shards: " + GameManager.GetComponent<GameManager>().shards, myGUIStyle);
    }
}

