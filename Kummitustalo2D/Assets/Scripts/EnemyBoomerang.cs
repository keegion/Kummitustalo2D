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
	public float boomerangCDTime = 2;
	bool boomerangOnCD;
	public float boomerangSmoothing = 10;

	void Start()
	{
		enemyAI = horseBoy.GetComponent<EnemyAI>();
		EnemyBoomeranging = horseBoy.GetComponent<EnemyBoomeranging>();

		//Debug.Log(horseBoy);
		//Debug.Log(boomerangPoint);
		//Debug.Log(EnemyBoomeranging);

		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

		if (enemyAI.shooting && !boomerangOnCD)
		{
			boomerangOnCD = true;
			StartCoroutine(BoomerangCD());

			//Debug.Log("Viskoo bumerangia");

			if (horseBoy.transform.rotation.y == 0)
			{
				direction = -1;
			} else {
				direction = 1;
			}

			transform.rotation = Quaternion.Euler(0, 0, 0);
			rb.AddForce(new Vector2(direction * speed, 0.01f * speed), ForceMode2D.Impulse);
		}

		if (boomerangOnCD) {

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

			//rb.rotation = 0; // was ist das?
			transform.rotation = Quaternion.identity; // tää pitäis tehä oikeesti vain kerran eikä joka freimillä
			transform.position = Vector3.Lerp(transform.position, boomerangPoint.position, boomerangSmoothing * Time.deltaTime);

		}

		//// pitäiskö palautuessaan jäädä heppamiehen käteen tai "päähän"? Lerpillä esim. loppumatka?
    }

	IEnumerator BoomerangCD()
	{
		//Debug.Log("IEnumerator");

		yield return new WaitForSeconds(boomerangCDTime);

		// tarvitsee jotenkin handlauksen että palaa kotiin jos jää esim. pelaajan selän taakse, ja pitäis muutenkin palata boomerangPointin kohdalle...
		//if (transform.position.x < horseBoy.transform.position.x)
		//{

		//Debug.Log("ET GO HOME");
		transform.position = boomerangPoint.position;
		transform.rotation = Quaternion.identity;
		rb.velocity = Vector2.zero;

		//}

		boomerangOnCD = false;
	}

}