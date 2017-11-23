using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI2 : MonoBehaviour
{

    Vector3 LastPOS, NextPOS;
    float velocity, currentSpeed;
    Vector3 previous;
    Animator anim;
    public float walkingSpeed = 3;
    public Transform frontRayRange, backRayRange;
    public bool shooting = false;
    bool readyToTP, teleportCD, spottedBackside, atWayPoint, spotted, changingDirection;
    public bool right, canTeleport = true;
    GameObject waypoint0, selectedWaypoint;
    GameObject[] waypoints, temp;
    Transform player;
    Enemy enemy;
    Animator ventanim;





    void Start()
    {

        waypoint0 = GameObject.FindGameObjectWithTag("TempLocation");
        temp = GameObject.FindGameObjectsWithTag("Waypoints");
        waypoints = new GameObject[temp.Length];
        waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
        Debug.Log(waypoints.Length);
        anim = GetComponent<Animator>();
        StartCoroutine(WaypointCountDown());
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<SpriteRenderer>().color = Color.red;
        enemy = GetComponent<Enemy>();
    }


    void Update()
    {
        if(!enemy.dead)
            CheckPos();
        currentSpeed = CurrentSpeed();
        if (anim.runtimeAnimatorController != null)
        {
            anim.SetFloat("speed", currentSpeed);
        }
        if (right && !enemy.dead)
            transform.position += Vector3.right * walkingSpeed * Time.deltaTime;
        if (!right && !enemy.dead)
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
        else if (LastPOS.x > NextPOS.x)
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
            shooting = PlayerHiddenByObstacles();

        }
        if (!spotted)
        {
            shooting = false;

        }
        if (spottedBackside && !atWayPoint && PlayerHiddenByObstacles()&&!changingDirection && !enemy.dead)
        {
            changingDirection = true;
            transform.forward = new Vector3(0f, 0f, transform.forward.z * -1);
            right = !right;
            shooting = true;
            StartCoroutine(ChangingDirectionCD());
        }



    }
    IEnumerator WaypointCountDown()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        readyToTP = true;
    }
    IEnumerator TeleportCountDown()
    {
        yield return new WaitForSeconds(2f);
        selectedWaypoint = waypoints[Random.Range(0, waypoints.Length)];
        ventanim = selectedWaypoint.GetComponent<Animator>();
        if(ventanim == null)
        {

            StartCoroutine(TeleportCountDown());
        }
        ventanim.SetBool("coming", true);
        yield return new WaitForSeconds(1f);
        transform.position = selectedWaypoint.transform.position;
        ventanim.SetBool("coming", false);
        StartCoroutine(WaypointCountDown());

    }
    IEnumerator ChangingDirectionCD()
    {
        yield return new WaitForSeconds(1f);
        changingDirection = false;
    }
          


    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "spot1")
        {
            right = true;

        }
        if (collision.tag == "spot2")
        {
            right = false;
        }
        if (collision.GetComponent<Collider2D>().gameObject.layer == 14)
        {
            if (readyToTP && canTeleport)
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
    //Check if enemy is behind obstacle
    bool PlayerHiddenByObstacles()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, player.position - transform.position, distanceToPlayer);

        foreach (RaycastHit2D hit in hits)
        {
            // ignore the enemy's own colliders (and other enemies)
            if (hit.transform.tag == "ShootingSkele" || hit.transform.tag == "HorseBoy" || hit.transform.tag == "spot2" || hit.transform.tag == "spot1" || hit.transform.tag == "RunningSkele" || hit.transform.tag == "Waypoints" || hit.transform.tag == "Muistisiru" || hit.transform.tag == "HPSiru")
                continue;

            if (hit.transform.tag != "Player")
            {
                return false;
            }
        }

        return true;

    }


}
