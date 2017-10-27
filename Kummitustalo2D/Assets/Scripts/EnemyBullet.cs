using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public float speed;
    // public GameObject player;

    public GameObject particle;


    void Start()
    {




        if (transform.rotation.y == 0)
            speed = speed * -1;



    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag !="Enemy" && collision.tag !="spot1" && collision.tag != "spot2" && collision.tag != "Tikkaat" && collision.tag != "Muistisiru")
       Destroy(gameObject);
       // GameObject temps = (GameObject)Instantiate(particle,collision.transform.position,collision.transform.rotation);
        

    }


}