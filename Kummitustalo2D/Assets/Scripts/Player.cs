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
	GameObject[] shardArray;
	public GameObject GameManagerPrefab;
	private GameObject GameManager;
	Rigidbody2D rb;
	public bool OnStairs, portalSummoned,dmgOnCD;
	CharController charctr;
    public GameObject dmgText,shardPic2,shardPic3,shardPic4;
	Animator animator;
	Animator healthMeterAnimator;
	PortalSummon summonPortal;
    int shardcount;
    public AudioClip gettingHit;
    public AudioClip pickUp;
    AudioSource source;
    public float time;




    // Use this for initialization
    void Start()
	{
        Debug.Log("Startup, current best time: " + PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name));
        summonPortal = GetComponent<PortalSummon>();
        shardcount = summonPortal.MaxShards;
        shardArray = new GameObject[shardcount-1];
        DeactivateShards();
        rb = GetComponent<Rigidbody2D>();
		charctr = GetComponent<CharController>();
		myGUIStyle.fontSize = 24;
		myGUIStyle.normal.textColor = Color.white;
        source = GetComponent<AudioSource>();

        // create GameManager if one doesn't exist in scene
        GameManager = GameObject.Find("GameManager(Clone)");
        if (GameManager == null)
		//if (!FindObjectOfType(typeof(GameManager)))
		{
			GameManager = Instantiate(GameManagerPrefab);
		}
        GameManager.GetComponent<GameManager>().shards = 0;
        animator = animator = GetComponent<Animator>();
		healthMeterAnimator = healthMeter.GetComponent<Animator>();
		ResetHealthBarAnimation();
		healthMeterAnimator.speed = 0;
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
        time += Time.deltaTime;
    }

	void CheckHP()
	{
		if (hp <= 0)
			Die();
	}

	void ResetHealthBarAnimation ()
	{
		healthMeterAnimator.Play("HealthBar", 0, 0.99f);
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
            source.PlayOneShot(gettingHit, 0.3f);
            hp -= 1;
            addDmgText();
            float roundedHealth = hp / maxHp;
			healthMeterAnimator.Play("HealthBar", -1, roundedHealth);
			healthMeterAnimator.speed = 0;

			animator.SetBool("Damage", true);
			StartCoroutine(CharDamageCD());
		}
	}

	IEnumerator CharDamageCD()
	{
		yield return new WaitForSeconds(0.1f);
		animator.SetBool("Damage", false);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="EnemyBullet")
        {
            source.PlayOneShot(gettingHit, 0.3f);
            //Debug.Log("Health" + hp/maxHp);
            hp -= 1;
            addDmgText();
            float roundedHealth = hp / maxHp;
            //Debug.Log("rounded: " + roundedHealth);
			healthMeterAnimator.Play("HealthBar", -1, roundedHealth);
			healthMeterAnimator.speed = 0;
        }
        if(collision.tag =="Muistisiru")
        {
			AddShard();
            source.PlayOneShot(pickUp, 0.3f);
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
        if(collision.tag =="HPSiru")
        {
            if(hp<7)
            {
                source.PlayOneShot(pickUp, 1f);
                hp++;
                float roundedHealth = hp / maxHp;
				healthMeterAnimator.Play("HealthBar", -1, roundedHealth);
				healthMeterAnimator.speed = 0;
                Destroy(collision.gameObject);
                if(hp ==7)
                {
                    healthMeterAnimator.Play("HealthBar",-1, 0.99f);
                }
            }
            
       
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
        if (!dmgOnCD && collision.gameObject.name == "RunningSkele" && !collision.gameObject.GetComponent<Enemy>().dead)
        {
            source.PlayOneShot(gettingHit, 0.3f);
            dmgOnCD = true;
            hp -= 1;
            StartCoroutine(RunningSkeleCD());
            addDmgText();
            float roundedHealth = hp / maxHp;
			healthMeterAnimator.Play("HealthBar", -1, roundedHealth);
			healthMeterAnimator.speed = 0;
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
        GUI.Label(new Rect(700, 30, 100, 30), "Time: " + Mathf.Round(time * 1f)/1f + " s", myGUIStyle);
    }
    void DeactivateShards()
    {
        if(shardcount<=4)
        {
            shardPic2.SetActive(false);
            shardPic3.SetActive(false);
            shardPic4.SetActive(false);
        }
         if (shardcount <9)
        {
            shardPic3.SetActive(false);
            shardPic4.SetActive(false);
        }
        if(shardcount <13)
        {
            shardPic4.SetActive(false);
        }
    }
}

