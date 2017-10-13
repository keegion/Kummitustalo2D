using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI2 : MonoBehaviour {

    float speed = 3;
    public Transform pos0;
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public Transform pos5;
    public Transform pos6;
    private Transform temp;
    bool col = false;
    bool arrived = false;
    Rigidbody2D rb;
    Vector3 LastPOS, NextPOS;
    Animator anim;
    Vector3 previous;
    float velocity;
    float charspeed;
    Transform[] posArray;
    int random = 2;
    int max;
    int old;
    int position;
    int newValue;
    bool canNew = false;
    bool starting = true;
    // Use this for initialization
    void Start()
    {


        AddArray();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        max = posArray.Length;
        temp = pos0;

    }

    // Update is called once per frame
    void Update()
    {

        if (!arrived)
            transform.position += (temp.transform.position - transform.position).normalized * speed * Time.deltaTime;

        CheckPos();
        charspeed = CurrentSpeed();
        anim.SetFloat("speed", charspeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "pos0" && starting)
        {
            position = 0;
            arrived = true;
            NewRandom();
            starting = false;

        }
        if (collision.tag == "pos1")
        {
            position = 1;
            Debug.Log("pos1");
            arrived = true;
            NewRandom();

        }
        else if (collision.tag == "pos2")
        {
            position = 2;
            Debug.Log("pos2");
            arrived = true;
            NewRandom();

        }
        else if (collision.tag == "pos3")
        {
            position = 3;
            Debug.Log("pos3");
            arrived = true;
            NewRandom();

        }
        else if (collision.tag == "pos4")
        {
            position = 4;
            Debug.Log("pos4");
            arrived = true;
            NewRandom();
            if (random >= 5)
            {
                transform.position = pos5.position;
            }
        }
        else if (collision.tag == "pos5")
        {
            position = 5;
            Debug.Log("pos5");
            arrived = true;
            NewRandom();
            if (random < 5)
            {
                transform.position = pos4.position;
            }

        }
        else if (collision.tag == "pos6")
        {
            position = 6;
            Debug.Log("pos6");
            arrived = true;
            NewRandom();
        }
        if (collision.tag == "ladder")
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            rb.gravityScale = 0;
        }
     
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ladder")
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            rb.gravityScale = 1;


        }
    
    }
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
    float CurrentSpeed()
    {
        velocity = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;
        return velocity;

    }
    void AddArray()
    {
        posArray = new Transform[7];

        posArray[0] = pos0;
        posArray[1] = pos1;
        posArray[2] = pos2;
        posArray[3] = pos3;
        posArray[4] = pos4;
        posArray[5] = pos5;
        posArray[6] = pos6;
    }
    void NewRandom()
    {
        old = random;
        random = Random.Range(0, 10);
        Debug.Log("random: " + random);
        if (position == 0)
        {
            if (random < 5)
            {
                temp = posArray[1];
                arrived = false;
            }
            if (random >= 5)
            {
                temp = posArray[2];
                arrived = false;
            }

        }
        else if (position == 6)
        {
            temp = posArray[5];
            arrived = false;
        }
        else if (position == 1)
        {
            temp = posArray[2];
            arrived = false;
        }
        else
        {
            if (random < 5)
            {
                temp = posArray[position - 1];
                arrived = false;
            }
            if (random >= 5)
            {
                temp = posArray[position + 1];
                arrived = false;
            }


        }




    }
}
