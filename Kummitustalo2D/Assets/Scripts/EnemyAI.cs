using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    Vector3 LastPOS, NextPOS;
    float velocity, currentSpeed;
    Vector3 previous;
    Animator anim;
    Rigidbody2D rb;
    public  float walkingSpeed = 3;
    public Transform frontRayRange, backRayRange;
    public bool seeEnemy = false;
    bool readyToTP, teleportCD, spotted, spottedBackside = false;
    bool shooting;
    bool atWayPoint;
    bool right = true;
    public GameObject waypoint0, waypoint1, waypoint2;
    GameObject[] waypoints;
    public int wayPointCount;


    void Start () {
        waypoints = new GameObject[wayPointCount];
        waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaypointCountDown());
	}


    void Update()
    {
        if (!shooting)
        CheckPos();
        currentSpeed = CurrentSpeed();
        anim.SetFloat("speed", currentSpeed);
        if (!seeEnemy && right)
            transform.position += Vector3.right * walkingSpeed * Time.deltaTime;
        if (!seeEnemy && !right)
            transform.position += Vector3.left * walkingSpeed * Time.deltaTime;


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
            transform.forward = new Vector3(0f, 0f, transform.forward.z * -1);
            right = !right;
            seeEnemy = true;
            shooting = true;



        }
    


    }
    IEnumerator WaypointCountDown()
    {
        yield return new WaitForSeconds(Random.Range(15f,40f));
        readyToTP = true;
    }
    IEnumerator TeleportCountDown()
    {
        yield return new WaitForSeconds(2f);
        transform.position = waypoints[Random.Range(0,waypoints.Length)].transform.position;
        StartCoroutine(WaypointCountDown());

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag=="spot1")
        {
            right = true;
            
        }
        if (collision.tag == "spot2")
        {
            right = false;
        }
        if (collision.GetComponent<Collider2D>().gameObject.layer == 14)
        {
            if(readyToTP)
            {
                readyToTP = false;
                transform.position = waypoint0.transform.position;
                StartCoroutine(TeleportCountDown());

            }
        }

    }
  
    void AddTransforms()
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        atWayPoint = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
       
    }
  

}
