using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    Vector3 LastPOS, NextPOS;
    float velocity;
    float currentSpeed;
    Vector3 previous;
    Animator anim;
    Rigidbody2D rb;
    Transform temp;
    float walkingSpeed = 3;
    public Transform spot1, spot2;
    public Transform rayRange;
    public bool seeEnemy = false;
    bool spotted = false;

    void Start () {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        temp = spot1;
	}


    void Update()
    {

        CheckPos();
        currentSpeed = CurrentSpeed();
        anim.SetFloat("speed", currentSpeed);
        if (!seeEnemy)
            transform.position += (temp.transform.position - transform.position).normalized * walkingSpeed * Time.deltaTime;

        
        Raycasting();
        Behaviours();


    }

        //Checks characters walking direction
        void CheckPos()
    {


        NextPOS.x = transform.position.x;
        if (LastPOS.x < NextPOS.x)
        {
            transform.forward = new Vector3(0f, 0f, -1f);

        }
        else
        {
            transform.forward = new Vector3(0f, 0f, 1f);

        }
        LastPOS.x = NextPOS.x;

       
    }


    //Check current walking speed and return value as float.
    float CurrentSpeed()
    {
        velocity = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;
        return velocity;

    }
    //uses linecast to spot a targe, if target hits the line, spotted = true
    void Raycasting()
    {
        Debug.DrawLine(transform.position, rayRange.position, Color.red);
        spotted = Physics2D.Linecast(transform.position, rayRange.position, 1 << LayerMask.NameToLayer("Player"));

    }
    //What happens when enemy is found
    void Behaviours()
    {
        if (spotted)
        {

            Debug.Log("found");
            seeEnemy = true;

        }

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag=="spot1")
        {
            temp = spot2;
        }
        if (collision.tag == "spot2")
        {
            temp = spot1;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        


    }
    private void OnCollisionEnter(Collision collision)
    {
       
    }

}
