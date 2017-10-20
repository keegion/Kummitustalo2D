using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float hp;
	public GUIStyle myGUIStyle;
	public GameObject GameManagerPrefab;
	private GameObject GameManager;
    Rigidbody2D rb;
    public bool OnStairs = false;
    CharController charctr;
	// Use this for initialization
	void Start () {
        charctr = GetComponent<CharController>();
		myGUIStyle.fontSize = 24;
		myGUIStyle.normal.textColor = Color.white;
		// create GameManager if one doesn't exist in scene
		GameManager = GameObject.Find("GameManager(Clone)");
        rb = GetComponent<Rigidbody2D>();
		if (GameManager == null){
			GameManager = Instantiate(GameManagerPrefab);
		}
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

    void Die()
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="EnemyBullet")
        {
            hp -= 10;
        }
        if(collision.tag =="Muistisiru")
        {
            Destroy(collision.gameObject);
            GameManager.GetComponent<GameManager>().shards++;
        }
        if (collision.tag == "Tikkaat" && charctr.isGrounded)
        {
            rb.isKinematic = true;
            OnStairs = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Tikkaat")
        {
            rb.isKinematic = false;
            OnStairs = false;
        }
    }

    void OnGUI()
	{
		GUI.Label(new Rect(75, 30, 40, 30), "x " + GameManager.GetComponent<GameManager>().livesLeft, myGUIStyle);
		GUI.Label(new Rect(145, 30, 100, 30), "Health: " + hp, myGUIStyle);
        GUI.Label(new Rect(320, 30, 40, 30), "x " + GameManager.GetComponent<GameManager>().shards, myGUIStyle);
    }
}
