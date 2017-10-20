using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float hp;
    public GameObject muistisiru;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (hp <= 0)
            Die();
	}
    
    void Die()
    {
        Instantiate(muistisiru,transform.position,transform.rotation);
        Destroy(gameObject);

      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            hp -= 10;
        }
    }
}
