using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomerang : MonoBehaviour {

    public float speed;
	Rigidbody2D rb;
	int direction = -1;
	public EnemyBoomeranging EnemyBoomeranging;
	public GameObject horseBoy;
	public Transform boomerangPoint;
	EnemyAI enemyAI;
    Enemy enemy;
	public float boomerangCDTime = 2;
	bool boomerangOnCD, deadhorsie;
	public float boomerangSmoothing = 10;
    Animator anim;

	bool usingAddForce;
	bool seekingBack;

	float startTime;
	float journeyLength;
	float distCovered;
	float fracJourney;



	void Start()
	{
		enemyAI = horseBoy.GetComponent<EnemyAI>();
		EnemyBoomeranging = horseBoy.GetComponent<EnemyBoomeranging>();
        anim = GetComponent<Animator>();
        enemy = horseBoy.GetComponent<Enemy>();
		//Debug.Log(horseBoy);
		//Debug.Log(boomerangPoint);
		//Debug.Log(EnemyBoomeranging);

		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

		//Debug.Log(usingAddForce);
		//Debug.Log(seekingBack);

		// Sattuu sydämeen tehdä nääkin joka freimillä
		if (horseBoy.transform.rotation.y == 0)
		{
			direction = -1;
		}
		else
		{
			direction = 1;
		}

		SetSpriteDirection();


		if (enemyAI.shooting && !boomerangOnCD && !enemy.dead)
		{
			boomerangOnCD = true;
			StartCoroutine(BoomerangCD());

			//Debug.Log("Viskoo bumerangia");

			// first make sure in start point
			transform.position = boomerangPoint.position;
			//transform.rotation = Quaternion.identity;
			rb.angularVelocity = 0;
			//rb.rotation = 0;

			usingAddForce = true;
			rb.AddForce(new Vector2(direction * speed, 0.01f * speed), ForceMode2D.Impulse);
		}

		if (!seekingBack && !enemy.dead)
		{
			if (boomerangOnCD && usingAddForce)
			{
				
				//Debug.Log("boomerangOnCD and using addforce");

				if ((rb.velocity.x > 0 && direction == 1) || (rb.velocity.x < 0 && direction == -1))
				{
					rb.AddForce(new Vector2(-0.6f * direction * speed * Time.deltaTime, -0.1f * speed * Time.deltaTime), ForceMode2D.Impulse);
				}
				else if ((rb.velocity.x < 0 && direction == 1) || (rb.velocity.x > 0 && direction == -1))
				{
					rb.AddForce(new Vector2(-0.6f * direction * speed * Time.deltaTime, 0.15f * speed * Time.deltaTime), ForceMode2D.Impulse);
				}

				// If you want to continuously rotate a rigidbody use MoveRotation instead, which takes interpolation into account.
				//rb.transform.Rotate(0, 0, 7);

			} 
			if (!boomerangOnCD) {

				//Debug.Log("else");

				//rb.angularVelocity = 0;
				//rb.rotation = 0;
				//transform.rotation = Quaternion.identity;

				// seuraa heppaa
				transform.position = Vector3.Lerp(transform.position, boomerangPoint.position, boomerangSmoothing * Time.deltaTime);

			}
		}

		if (seekingBack) // && !(fracJourney > 1f) // fracJourney voi mennä myös "Infinity":ksi...
		{
			//rb.velocity = Vector2.zero;

			distCovered = (Time.time - startTime) / 0.5f;
			fracJourney = distCovered / journeyLength;

			//Debug.Log("seekingBack");
			//Debug.Log(distCovered);
			//Debug.Log(fracJourney);
			//Debug.Log(Vector3.Distance(transform.position, boomerangPoint.position));

			transform.position = Vector3.Lerp(transform.position, boomerangPoint.position, fracJourney);
			transform.rotation = Quaternion.identity;

			// Tätä käytettäessä pitäisi ilmeisesti olla rigidbodyllä kinematic ja interpolate
			//rb.MovePosition(boomerangPoint.position);
		} 
		//else if (seekingBack){
		//	rb.velocity = Vector2.zero;
		//	seekingBack = false; // jos ei ollut seekingBack niin saattoi jäädä paikalleen leijumaan vaikka heppa jatkoi matkaa
		//}

        if(enemy.dead && !deadhorsie)
        {
            deadhorsie = true;
            anim.SetBool("dead", true);
            Debug.Log("deadhead");
            gameObject.layer = LayerMask.NameToLayer("dead");
         
        }

    }

	IEnumerator BoomerangCD()
	{
		//Debug.Log("IEnumerator");

		yield return new WaitForSeconds(boomerangCDTime/2);

		if (!seekingBack) {
			seekingBack = true;
			usingAddForce = false;
			rb.velocity = Vector2.zero;
			startTime = Time.time;
			journeyLength = Vector3.Distance(transform.position, boomerangPoint.position);
		}

		yield return new WaitForSeconds(boomerangCDTime/2);

		rb.angularVelocity = 0;
		//rb.rotation = 0;
		rb.velocity = Vector2.zero;
		fracJourney = 0;
		seekingBack = false;
		boomerangOnCD = false;
	}

	// go right back if hit something
	private void OnCollisionEnter2D(Collision2D collision)
	{
		startTime = Time.time;
		journeyLength = Vector3.Distance(transform.position, boomerangPoint.position);
		rb.angularVelocity = 0;
		//rb.rotation = 0;
		rb.velocity = Vector2.zero;
		seekingBack = true;
		usingAddForce = false;
	}

	void SetSpriteDirection () {
		Vector2 boomerangLocalScale = transform.localScale;
		transform.localScale = new Vector2(Mathf.Abs(boomerangLocalScale.x) * -direction, boomerangLocalScale.y);
	}

}