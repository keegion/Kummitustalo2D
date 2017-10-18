using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    Vector3 LastPOS, NextPOS;
    float velocity, currentSpeed;
    Vector3 previous;
    Animator anim;
    Rigidbody2D rb;
    Transform temp;
    public  float walkingSpeed = 3;
    public Transform spot1, spot2, frontRayRange, backRayRange;
    public bool seeEnemy = false;
    bool spotted, spottedBackside = false;
    bool shooting;
    bool atWayPoint;

    void Start () {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        temp = spot1;
	}


    void Update()
    {
        if (!shooting)
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
        else if(LastPOS.x > NextPOS.x)
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
        Debug.DrawLine(transform.position, frontRayRange.position, Color.red);
        spotted = Physics2D.Linecast(transform.position, frontRayRange.position, 1 << LayerMask.NameToLayer("Player"));

        Debug.DrawLine(transform.position, backRayRange.position, Color.red);
        spottedBackside = Physics2D.Linecast(transform.position, backRayRange.position, 1 << LayerMask.NameToLayer("Player"));

    }
    //What happens when enemy is found
    void Behaviours()
    {
        if (spotted)
        {

            seeEnemy = true;
            shooting = true;

        }
        if (!spotted)
        {
            seeEnemy = false;
            shooting = false;
        }
        if(spottedBackside && !atWayPoint)
        {
            
           
            if (temp == spot2)
            {
                temp = spot1;
                
            }
                
            if (temp == spot1)
            {
                temp = spot2;
            }

        }
    


    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag=="spot1")
        {

            temp = spot2;
            atWayPoint = true;
        }
        if (collision.tag == "spot2")
        {
            temp = spot1;
            atWayPoint = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        atWayPoint = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
       
    }
  

}
