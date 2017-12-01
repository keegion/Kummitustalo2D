using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float hp;
    public GameObject hpsiru;
	EnemyBoomeranging boomerangingController;
    Animator anim;
    public bool dead;
    CapsuleCollider2D coll;
    BoxCollider2D boxcoll;
    public GameObject particle;
    
    

    // Use this for initialization
    void Start () {
		boomerangingController = GetComponent<EnemyBoomeranging>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        boxcoll = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (hp <= 0 && !dead)
            Die();
	}
    
    void Die()
    {
        dead = true;
        int rnd = Random.Range(0, 30);
        if(rnd <= 10)
        {
            Instantiate(hpsiru, transform.position, transform.rotation);
        }
        
        if (gameObject.tag == "RunningSkele" || gameObject.tag == "ShootingSkele" )
        {
            anim.SetBool("dead", true);
            coll.offset = new Vector2(0f, -1.6f);
            coll.size = new Vector2(0.1f, 0.1f);
            particle.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer("dead");


            // transform.position = new Vector3(transform.position.x, transform.position.y + 3f, +0);
        }
        if (gameObject.tag == "HorseBoy")
        {
            anim.SetBool("dead", true);
            boxcoll.offset = new Vector2(0f, -4.7f);
            boxcoll.size = new Vector2(0.1f, 0.1f);
            gameObject.layer = LayerMask.NameToLayer("dead");
        }
		//if (boomerangingController){
		//	Destroy(boomerangingController.boomerang);
		//}
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            hp -= 10;
        }
    }
}
