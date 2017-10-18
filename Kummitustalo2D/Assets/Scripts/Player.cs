using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float hp;
	// Use this for initialization
	void Start () {
		
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
        Debug.Log("You have died");
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="EnemyBullet")
        {
            hp -= 10;
        }
    }
}
