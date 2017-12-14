using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]

public class CharController : MonoBehaviour {

	public float speed, input_x, input_y, jumpForce, bulletSpeed, bulletLifeTime, bulletCDTime = 1f, bulletCDTimestamp = 0, tempSpeed;
	public bool facingRight, isGrounded, miniSized; //, bulletCD;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	Rigidbody2D rb;
    Player player;
	Animator animator;
    public AudioClip shootingSound;
    public AudioClip jumpSound;
    AudioSource source;
    Vector3 sp, dir;
    public Transform GroundCheck;
	public LayerMask groundLayer;

	bool isRunning;
	bool isJumping;

	void Start()
	{
        player = GetComponent<Player>();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

	void Update()
	{
		isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.15f, groundLayer);


        if (isGrounded && Input.GetButtonDown("Jump") && !player.dead)
		{
            source.PlayOneShot(jumpSound, 0.2f);
			Jump();
			isJumping = true;
			animator.SetBool("Jump", true);
		}

		if (isGrounded && isJumping){
			isJumping = false;
			animator.SetBool("Jump", false);
		}

		input_x = Input.GetAxis("Horizontal");
		input_y = Input.GetAxis("Vertical");
        if(!player.dead)
		transform.Translate(input_x * Time.deltaTime * speed, 0, 0);

		if (input_x < 0 && facingRight && !player.dead)
		{
			Flip();
		}
		else if (input_x > 0 && !facingRight && !player.dead)
		{
			Flip();
		}

		if (input_x != 0 && !isRunning && !isJumping && isGrounded){
			isRunning = true;
			animator.SetBool("Run", true);
		} else if (input_x == 0 && isRunning) {
			isRunning = false;
			animator.SetBool("Run", false);
		}

		if (Input.GetButtonDown("Fire1") && !player.dead)
		{
			Fire();
		}
        if(player.OnStairs)
        {
            transform.Translate(input_x * Time.deltaTime * speed, input_y * Time.deltaTime * speed, 0);
        }
		if ( (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) ) && !miniSized && !player.dead)
        {
            StartCoroutine(MiniSize());
        }

     

	}

	void Flip()
	{
		facingRight = !facingRight;
		transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
	}

	void Jump () 
	{
		rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
		isGrounded = false;
	}

    void Fire()
    {
      

        if (bulletCDTimestamp <= Time.time)
        //if (!bulletCD)
        {
            source.PlayOneShot(shootingSound, 0.3f);
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

    IEnumerator MiniSize()
    {
        miniSized = true;
        tempSpeed = speed;
        speed = 2;
        transform.localScale = new Vector2(transform.localScale.x * 0.4f, 0.1f);
        yield return new WaitForSeconds(2f);
        transform.localScale = new Vector2(transform.localScale.x * 2.5f, 0.25f);
        miniSized = false;
        speed = tempSpeed;
    }

    

	//IEnumerator BulletCDRoutine(float waitTime)
	//{
	//	yield return new WaitForSeconds(waitTime);
	//	bulletCD = false;
	//}

}
