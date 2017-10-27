using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public int hp;
	public GUIStyle myGUIStyle;
    public Image healthMeter;
	public GameObject GameManagerPrefab;
	private GameObject GameManager;
    Rigidbody2D rb;
    public bool OnStairs = false;
    CharController charctr;

    public Texture healthImage;
    public Material healthMaterial;


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

        healthMaterial = GetComponent<Renderer>().material;

        //Debug.Log(healthMeter.GetComponent<Image>().sprite);

        //healthMeter.GetComponent<Image>().sprite = healthMeter.GetComponent<HealthImage>().pieces[2];

        //Debug.Log(healthMeter.GetComponent<HealthImage>().pieces[10]);

        //Debug.Log(GameObject.Find("Health").GetComponent<SpriteRenderer>().sprite);

        

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
            Debug.Log("Health" + hp);
            hp -= 1;
            healthMeter.GetComponent<Image>().sprite = healthMeter.GetComponent<HealthImage>().pieces[hp];
            Debug.Log(healthMeter.GetComponent<HealthImage>().pieces[hp]);

           // healthImage = textureFromSprite(healthMeter.GetComponent<HealthImage>().pieces[hp]);

          //  healthMaterial.SetTexture("_MainTex", healthImage);


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

    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

}
