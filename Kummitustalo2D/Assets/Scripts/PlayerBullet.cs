using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    //public float bulletSpeed;
    //public Rigidbody2D rb;

    //public GameObject player;

    //void Start()
    //{
    //	rb = GetComponent<Rigidbody2D>();
    //}

    //void FixedUpdate()
    //{
    //	Debug.Log(player.GetComponent<CharController>().facingRight);
    //	rb.AddForce(new Vector2(1 * bulletSpeed, 0));
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "spot1" && collision.tag != "spot2" && collision.tag != "Tikkaat")
            Destroy(gameObject);
        // GameObject temps = (GameObject)Instantiate(particle,collision.transform.position,collision.transform.rotation);


    }
}
