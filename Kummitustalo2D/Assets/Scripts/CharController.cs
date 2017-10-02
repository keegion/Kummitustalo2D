using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

	public float speed = 5f;
	float someScale;

	void Start()
	{
		someScale = transform.localScale.x;
	}

	void Update()
	{

	}

	void FixedUpdate()
	{

		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		transform.Translate(moveHorizontal * Time.deltaTime * speed, moveVertical * Time.deltaTime * speed, 0);

		if (moveHorizontal > 0)
		{
			transform.localScale = new Vector2(-someScale, transform.localScale.y);
		}
		if (moveHorizontal < 0)
		{
			transform.localScale = new Vector2(someScale, transform.localScale.y);
		}

	}

}
