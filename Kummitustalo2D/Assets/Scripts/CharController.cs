using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

    public float speed = 5f;
    float someScale;

	//public Rigidbody2D playerRB;

	void Start()
	{
		someScale = transform.localScale.x;
		//playerRB = GetComponent<Rigidbody2D>();
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



		//float moveHorizontal = Input.GetAxis("Horizontal");
		//float moveVertical = Input.GetAxis("Vertical");

		//Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		//playerRB.AddForce(movement * speed);

		//if (playerRB.velocity.x >= 0) {
		//    transform.localScale = new Vector2(someScale, transform.localScale.y);
		//} else {
		//    transform.localScale = new Vector2(-someScale, transform.localScale.y);
		//}

	}

}
