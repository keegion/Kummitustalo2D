using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

	public float speed = 5f;
	//float someScale;
	bool facingRight;
	float input_x;
	float input_y;

	void Start()
	{
		//someScale = transform.localScale.x;
	}

	void Update()
	{

	}

	void FixedUpdate()
	{

		input_x = Input.GetAxis("Horizontal");
		input_y = Input.GetAxis("Vertical");

		transform.Translate(input_x * Time.deltaTime * speed, input_y * Time.deltaTime * speed, 0);

		//if (input_x > 0)
		//{
		//	transform.localScale = new Vector2(-someScale, transform.localScale.y);
		//}
		//if (input_x < 0)
		//{
		//	transform.localScale = new Vector2(someScale, transform.localScale.y);
		//}


		if (input_x < 0 && facingRight)
		{
			Flip();

		}
		else if (input_x > 0 && !facingRight)
		{
			Flip();

		}

	}

	void Flip()
	{

		facingRight = !facingRight;
		transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
	}

}
