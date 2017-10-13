using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public float speed;
    // public GameObject player;
    public EnemyAI enemy;
    public GameObject particle;


    void Start()
    {


        enemy = FindObjectOfType<EnemyAI>();

        if (!enemy.facingRight)
        {
            speed = -speed;
        }

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag !="Enemy")
       Destroy(gameObject);
       // GameObject temps = (GameObject)Instantiate(particle,collision.transform.position,collision.transform.rotation);
        

    }


}