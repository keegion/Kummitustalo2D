using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public float speed;
    public GameObject particle;
	Rigidbody2D rb;
	public bool isBoomerang;

    void Start()
    {
        if (transform.rotation.y == 0)
            speed = speed * -1;

		rb = GetComponent<Rigidbody2D>();
		rb.AddForce(new Vector2(1f * speed, 0), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
		if (isBoomerang)
		{
			rb.AddForce(new Vector2(-1f * speed * Time.deltaTime, 0), ForceMode2D.Impulse);
			rb.transform.Rotate(0, 0, 7);
			Destroy(gameObject, 1.85f);
		}
		// pitäiskö tuhoutua edes kun osuu pelaajaan?
		// pitäiskö palautuessaan jäädä heppamiehen käteen tai "päähän"?

		// vanha tapa velocityllä:
        //GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag !="Enemy" && collision.tag !="spot1" && collision.tag != "spot2" && collision.tag != "Tikkaat" && collision.tag != "Muistisiru")
       Destroy(gameObject);
       // GameObject temps = (GameObject)Instantiate(particle,collision.transform.position,collision.transform.rotation);
        

    }


}