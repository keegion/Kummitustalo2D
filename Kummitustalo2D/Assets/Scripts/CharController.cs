using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class CharController : MonoBehaviour {

	public float speed = 5f;

	public bool facingRight;
	float input_x;
	float input_y;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed;
	public float bulletLifeTime;

	Rigidbody2D rb;
	//Animator animator;
	public float jumpForce;
	public bool isGrounded;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		//animator = GetComponent<Animator>();
	}

	void Update()
	{
	}

	void FixedUpdate()
	{
		input_x = Input.GetAxis("Horizontal");
		input_y = Input.GetAxis("Vertical");

		transform.Translate(input_x * Time.deltaTime * speed, input_y * Time.deltaTime * speed, 0);

		if (input_x < 0 && facingRight)
		{
			Flip();
		}
		else if (input_x > 0 && !facingRight)
		{
			Flip();
		}

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			Jump();
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

	void OnCollisionEnter2D(Collision2D collision)
	{
		// Todo: varmaan joku tsekkaus mihin osui
		isGrounded = true;
	}

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
		Vector3 bulletTransformHorizontal = bullet.transform.right;

		if (facingRight) {
			//bulletRB.velocity = bulletTransformHorizontal * bulletSpeed;
			bulletRB.AddForce(bulletTransformHorizontal * bulletSpeed);
		} else {
			//bulletRB.velocity = -bulletTransformHorizontal * bulletSpeed;
			bulletRB.AddForce(-bulletTransformHorizontal * bulletSpeed);
		}

		Destroy(bullet, bulletLifeTime);
	}

}