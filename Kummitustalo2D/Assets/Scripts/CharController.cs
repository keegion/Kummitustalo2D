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
	//Animator animator;
	public float jumpForce;

	void Start()
	{
		CharacterRB = GetComponent<Rigidbody2D>();
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

		if (Input.GetButtonDown("Jump"))
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

	void Jump () 
	{
		//animator.SetTrigger("Jump");
		CharacterRB.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
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