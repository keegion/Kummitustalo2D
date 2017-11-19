using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public float speed;
    public GameObject particle;
	Rigidbody2D rb;
	public bool isBoomerang;
	int direction = 1;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		if (transform.rotation.y == 0)
		{
			//speed = speed * -1;
			direction = -1;
		}

		if (isBoomerang)
		{
			rb.AddForce(new Vector2(direction * speed, 0.15f * speed), ForceMode2D.Impulse);
		}
		else 
		{
			rb.AddForce(new Vector2(direction * speed, 0), ForceMode2D.Impulse);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (isBoomerang)
		{
			if ( (rb.velocity.x > 0 && direction == 1) || (rb.velocity.x < 0 && direction == -1) )
			{
				rb.AddForce(new Vector2(-1 * direction * speed * Time.deltaTime, -0.3f * speed * Time.deltaTime), ForceMode2D.Impulse);
			}
			else if ( (rb.velocity.x < 0 && direction == 1) || (rb.velocity.x > 0 && direction == -1) )
			{
				rb.AddForce(new Vector2(-1 * direction * speed * Time.deltaTime, 0.3f * speed * Time.deltaTime), ForceMode2D.Impulse);
			}

			rb.transform.Rotate(0, 0, 7);
			Destroy(gameObject, 2f);
		}

		// pitäiskö palautuessaan jäädä heppamiehen käteen tai "päähän"? Lerpillä esim. loppumatka?

		// vanha tapa velocityllä:
        //GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag != "HorseBoy" && collision.tag != "spot1" && collision.tag != "spot2" && collision.tag != "Tikkaat" && collision.tag != "Muistisiru" && collision.tag !="RunningSkele" && collision.tag != "ShootingSkele")
		{
			Destroy(gameObject);
		}
		
       // GameObject temps = (GameObject)Instantiate(particle,collision.transform.position,collision.transform.rotation);
    }


}