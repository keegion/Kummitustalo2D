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
			transform.rotation = Quaternion.identity;

			rb.AddForce(new Vector2(direction * speed, 0.01f * speed), ForceMode2D.Impulse);
		}

		if (!seekingBack && !enemy.dead)
		{
			if (boomerangOnCD)
			{
				
				//Debug.Log("boomerangOnCD");

				if ((rb.velocity.x > 0 && direction == 1) || (rb.velocity.x < 0 && direction == -1))
				{
					rb.AddForce(new Vector2(-1 * direction * speed * Time.deltaTime, -0.1f * speed * Time.deltaTime), ForceMode2D.Impulse);
				}
				else if ((rb.velocity.x < 0 && direction == 1) || (rb.velocity.x > 0 && direction == -1))
				{
					rb.AddForce(new Vector2(-1 * direction * speed * Time.deltaTime, 0.15f * speed * Time.deltaTime), ForceMode2D.Impulse);
				}

				//rb.transform.Rotate(0, 0, 7);

			} else {

				//Debug.Log("else");

				// Pitäiskö nollaus tehdä rigidbodyllä koska alussa siihen kohdistetaan voimaa..?

				//rb.rotation = 0; // was ist das?
				transform.rotation = Quaternion.identity; // tää pitäis tehä oikeesti vain kerran eikä joka freimillä
				// seuraa heppaa
				transform.position = Vector3.Lerp(transform.position, boomerangPoint.position, boomerangSmoothing * Time.deltaTime);

			}
		}

		if (seekingBack)
		{
			distCovered = (Time.time - startTime) / 2f;
			fracJourney = distCovered / journeyLength;

			//Debug.Log(distCovered); // käyttäytyy oudosti..?
			//Debug.Log(fracJourney); // käyttäytyy oudosti..?
			// Toimi todella oudosti ennen velocity zeroa (joka ei sekään pysäytä bumenragia), joka toisella toimi melkein oikein...
			transform.position = Vector3.Lerp(transform.position, boomerangPoint.position, fracJourney);

			transform.rotation = Quaternion.identity;

			// Tätä käytettäessä pitäisi ilmeisesti olla rigidbodyllä kinematic ja interpolate
			//rb.MovePosition(boomerangPoint.position);
		}

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

		// tarvitsee jotenkin handlauksen että palaa kotiin jos jää esim. pelaajan selän taakse...
		//if (transform.position.x < horseBoy.transform.position.x)
		//{

		seekingBack = true;
		//Debug.Log("seeking back");

		startTime = Time.time;
		journeyLength = Vector3.Distance(transform.position, boomerangPoint.position);
		//Debug.Log(journeyLength); // Tuntuis näyttävän suht oikein

		yield return new WaitForSeconds(boomerangCDTime/2);

		seekingBack = false;

		rb.angularVelocity = 0;
		rb.velocity = Vector2.zero;

		//}

		boomerangOnCD = false;
	}

	void SetSpriteDirection () {
		Vector2 boomerangLocalScale = transform.localScale;
		transform.localScale = new Vector2(Mathf.Abs(boomerangLocalScale.x) * -direction, boomerangLocalScale.y);
	}

}