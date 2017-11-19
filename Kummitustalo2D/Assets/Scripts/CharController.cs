using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]

public class CharController : MonoBehaviour {

	public float speed = 5f, input_x, input_y, jumpForce, bulletSpeed, bulletLifeTime, bulletCDTime = 1f, bulletCDTimestamp = 0;
	public bool facingRight, isGrounded; //, bulletCD;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	Rigidbody2D rb;
    Player player;
	Animator animator;

	public Transform GroundCheck;
	public LayerMask groundLayer;

	bool isRunning;
	bool isJumping;

	void Start()
	{
        player = GetComponent<Player>();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.15f, groundLayer);

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			Jump();
		}

		if (isGrounded && isJumping){
			animator.SetBool("Jump", false);
			isJumping = false;
		}

		input_x = Input.GetAxis("Horizontal");
		input_y = Input.GetAxis("Vertical");

		transform.Translate(input_x * Time.deltaTime * speed, 0, 0);

		if (input_x < 0 && facingRight)
		{
			Flip();
		}
		else if (input_x > 0 && !facingRight)
		{
			Flip();
		}

		if (input_x != 0 && !isRunning){
			animator.SetBool("Run", true);
			isRunning = true;
		} else if (input_x == 0 && isRunning) {
			animator.SetBool("Run", false);
			isRunning = false;
		}

		if (Input.GetButtonDown("Fire1"))
		{
			Fire();
		}
        if(player.OnStairs)
        {
            transform.Translate(input_x * Time.deltaTime * speed, input_y * Time.deltaTime * speed, 0);
        }

        // tarkistaa onko pelaaja kosketuksessa portaaliin ja jos painaa nuolinäppäintä ylös tai w näppäintä aloittaa uuden tason.
        if(player.portalSummoned)
        {

            if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                //tähän seuraava taso(väliaikaisesti kuolee eli aloittaa tason uudelleen)
                player.Die();
            }

        }

	}

	void Flip()
	{
		facingRight = !facingRight;
		transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
	}

	void Jump () 
	{
        animator.SetBool("Jump", true);
		rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
		isGrounded = false;
		isJumping = true;
	}

	void Fire()
	{
		if (bulletCDTimestamp <= Time.time)
		//if (!bulletCD)
		{
			GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

			Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
			Vector3 bulletTFRight = bullet.transform.right;
			Vector2 bulletTFLocalScale = bullet.transform.localScale;

			bullet.transform.localScale = new Vector2(bulletTFLocalScale.x * (facingRight ? -1 : 1), bulletTFLocalScale.y);

			bulletRB.AddForce(bulletTFRight * bulletSpeed * (facingRight ? 1 : -1));
			//bulletRB.velocity = bulletTFRight * bulletSpeed * (facingRight? 1 : -1 );

			Destroy(bullet, bulletLifeTime);

			bulletCDTimestamp = Time.time + bulletCDTime;

		//	bulletCD = true;
		//	StartCoroutine(BulletCDRoutine(bulletCDTime));

		}
	}

    

	//IEnumerator BulletCDRoutine(float waitTime)
	//{
	//	yield return new WaitForSeconds(waitTime);
	//	bulletCD = false;
	//}

}
