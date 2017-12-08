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
	public Transform livesMeter, playerSpawnPoint;
    Transform livesMeterMiddleBG, livesMeterRightBG;
    GameObject livesMeterPlayerIcon1, livesMeterPlayerIcon2, livesMeterPlayerIcon3;
    RectTransform middleRectTransform;
    GameObject[] shardArray;
	public GameObject GameManagerPrefab;
	private GameObject GameManager;
	Rigidbody2D rb;
	public bool OnStairs, portalSummoned,dmgOnCD, dead;
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
    bool isHudInitialized;
    CapsuleCollider2D coll;

    // Use this for initialization
    void Start()
	{
        coll = GetComponent<CapsuleCollider2D>();
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

        livesMeterMiddleBG = livesMeter.Find("middleBG");
        livesMeterRightBG = livesMeter.Find("rightBG");
        livesMeterPlayerIcon1 = livesMeter.Find("playerIcon1").gameObject;
        livesMeterPlayerIcon2 = livesMeter.Find("playerIcon2").gameObject;
        livesMeterPlayerIcon3 = livesMeter.Find("playerIcon3").gameObject;
        middleRectTransform = livesMeterMiddleBG as RectTransform;

    }

    int CompareObNames(GameObject a, GameObject b)
	{
		return string.Compare(a.name, b.name, StringComparison.CurrentCulture);
	}

	// Update is called once per frame
	void Update()
	{
        if (!isHudInitialized)
        {
			UpdateLivesMeter();
            isHudInitialized = true;
        }

        CheckHP();
        time += Time.deltaTime;
    }

	void CheckHP()
	{
        if (hp <= 0 && !dead)
            StartCoroutine(Die());
		
	}

	void ResetHealthBarAnimation ()
	{
		healthMeterAnimator.Play("HealthBar", 0, 0.99f);
	}

	void UpdateLivesMeter ()
	{
		float livesMeterStepWidth = 46;
		float livesMiddleWidth = livesMeterStepWidth * GameManager.GetComponent<GameManager>().livesLeft;

		middleRectTransform.sizeDelta = new Vector2((livesMeterStepWidth * GameManager.GetComponent<GameManager>().livesLeft) - 30f, middleRectTransform.sizeDelta.y);

		livesMeterRightBG.position = new Vector3(141.92f + (0.32f * GameManager.GetComponent<GameManager>().livesLeft), 5f, 1f);
		//livesMeterRightBG.position = new Vector3(34.88f + livesMiddleWidth, 5f, 100f); // mittayksiköt/skaala näyttää olevan jotain paljon isompaa kuin pikseleitä
		//Debug.Log(livesMeterRightBG.position);

		// vaihtoehtoinen tapa liikuttaa kikkaretta
		//RectTransform rightRectTransform = livesMeterRightBG as RectTransform;
		//rightRectTransform.SetPositionAndRotation(new Vector3(32 + livesMiddleWidth, 5, 100), Quaternion.identity);

		// The most quick and dirty way ever to do this
		if (GameManager.GetComponent<GameManager>().livesLeft < 3)
		{
			livesMeterPlayerIcon3.SetActive(false);
		}
		if (GameManager.GetComponent<GameManager>().livesLeft < 2)
		{
			livesMeterPlayerIcon2.SetActive(false);
		}
	}

	 IEnumerator Die()
	{
        dead = true;
        gameObject.tag = "Enemy";
        gameObject.layer = 8;
        animator.SetBool("dead", true);
        coll.offset = new Vector2(-0.2f, -1.6f);
        yield return new WaitForSeconds(5f);
        coll.offset = new Vector2(-0.2f, 0f);
        gameObject.tag = "Player";
        gameObject.layer = 9;
        animator.SetBool("dead", false);
        if (GameManager.GetComponent<GameManager>().livesLeft == 0)
		{
			Debug.Log("Game over man");
			SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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
			UpdateLivesMeter();
            dead = false;
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "mimic" && !dmgOnCD)
        {
            dmgOnCD = true;
            source.PlayOneShot(gettingHit, 0.3f);
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

