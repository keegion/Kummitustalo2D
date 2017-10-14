using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]

public class CharController : MonoBehaviour {

	public float speed = 5f, input_x, input_y, jumpForce, bulletSpeed, bulletLifeTime;
	public bool facingRight, isGrounded;
	public GameObject bulletPrefab;
	public Transform bulletSpawn; //, tagGround;
	Rigidbody2D rb;
	//Animator animator;

	//public LayerMask playerMask;

	public Transform GroundCheck1; // Put the prefab of the ground here
	public LayerMask groundLayer; // Insert the layer here.

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		//animator = GetComponent<Animator>();
	}

	void Update()
	{
		////isGrounded = Physics2D.Linecast(transform.position, tagGround.position, playerMask);
		//if (Physics2D.Linecast(transform.position, tagGround.position, playerMask) || rb.velocity.y == 0)
		//{
		//    isGrounded = true;
		//}
		//else
		//{
		//    isGrounded = false;
		//}
		////Debug.Log(Physics2D.Linecast(transform.position, tagGround.position, playerMask).transform);

		isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer); // checks if you are within 0.15 position in the Y of the ground

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			Jump();
		}

		// test dying

		// starting game from the beginning (when no more lives left)
		if (transform.position.y < -4f)
		{
			Debug.Log("Die motherfucker die");
			SceneManager.LoadScene("Test_start_scene", LoadSceneMode.Single);
		}

		// reloading level (when still some lives left)
		if (Input.GetButtonDown("Fire2"))
		{
			gameObject.GetComponent<Player>().hp -= 10f;
			Debug.Log(gameObject.GetComponent<Player>().hp);
			if (gameObject.GetComponent<Player>().hp <= 0)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	void FixedUpdate()
	{
		input_x = Input.GetAxis("Horizontal");
		input_y = 0; //Input.GetAxis("Vertical");

		transform.Translate(input_x * Time.deltaTime * speed, input_y * Time.deltaTime * speed, 0);

		if (input_x < 0 && facingRight)
		{
			Flip();
		}
		else if (input_x > 0 && !facingRight)
		{
			Flip();
		}

		if (Input.GetButtonDown("Fire1"))
		{
			Fire();
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
	}

	// Laukeaa myös hypätessä, joka johtaa siihen että voi hypätä kerran myös ilmassa...
	//void OnCollisionStay2D(Collision2D collision)
	//{
	//	Debug.Log(collision.collider.gameObject.name);
	//	isGrounded = true;
	//}

	// Tapahtuu myös osuessa esineisiin sivuilta, mikä ei hyvä juttu
	//void OnCollisionEnter2D(Collision2D collision)
	//{
	//	isGrounded = true;
	//}§

	void Jump () 
	{
		//animator.SetTrigger("Jump");
		rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
		isGrounded = false;
	}

	void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
		Vector3 bulletTFRight = bullet.transform.right;
		Vector2 bulletTFLocalScale = bullet.transform.localScale;

		bullet.transform.localScale = new Vector2(bulletTFLocalScale.x * (facingRight ? -1 : 1), bulletTFLocalScale.y);

		bulletRB.AddForce(bulletTFRight * bulletSpeed * (facingRight ? 1 : -1));
		//bulletRB.velocity = bulletTFRight * bulletSpeed * (facingRight? 1 : -1 );

		Destroy(bullet, bulletLifeTime);
	}

}
