using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

	public float speed = 5f;

	public bool facingRight;
	float input_x;
	float input_y;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed;
	public float bulletLifeTime;

	Rigidbody2D CharacterRB;
	Animator animator;
	public float jumpForce;

	void Start()
	{
		CharacterRB = GetComponent<Rigidbody2D>();
		//animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Fire();
		}

		if (Input.GetButtonDown("Jump"))
		{
			//animator.SetTrigger("Jump");
			CharacterRB.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
		}
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
	}

	void Fire()
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
		Vector3 bulletTransformHorizontal = bullet.transform.right;

		// Add velocity to the bullet
		if (facingRight) {
			bulletRB.velocity = bulletTransformHorizontal * bulletSpeed;
		} else {
			bulletRB.velocity = -bulletTransformHorizontal * bulletSpeed;
		}

		// Destroy the bullet after 2 seconds
		Destroy(bullet, bulletLifeTime);
	}

	void Flip()
	{
		facingRight = !facingRight;
		transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
	}

}
