using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomeranging : MonoBehaviour {
    EnemyAI enemyAI;
	public GameObject boomerangPrefab;
	public Transform boomerangPoint;
	public float boomerangCDTime = 1;
    bool boomerangOnCD;
	internal GameObject boomerang;
	//Rigidbody2D boomerangRB;
	public float boomerangSmoothing = 5f;
	EnemyBoomerang boomerangController;

	void Start () {
        enemyAI = GetComponent<EnemyAI>();
		boomerang = Instantiate(boomerangPrefab, boomerangPoint.position, transform.rotation);

		boomerangController = boomerang.GetComponent<EnemyBoomerang>();
		boomerangController.horseBoy = gameObject;
		boomerangController.boomerangPoint = boomerangPoint;
		         
		EnemyBoomerang controller = boomerang.GetComponent<EnemyBoomerang>();

		//boomerangRB = boomerang.GetComponent<Rigidbody2D>();
	}

	//void Update () {
		
 //       if(enemyAI.spotted && !boomerangOnCD)
 //       {
	//		boomerangOnCD = true;
 //           StartCoroutine(BoomerangCD());

	//	}

	//	boomerang.transform.position = Vector3.Lerp(boomerang.transform.position, boomerangPoint.position, boomerangSmoothing * Time.deltaTime);
	//}

  //  IEnumerator BoomerangCD()
  //  {
		//yield return new WaitForSeconds(boomerangCDTime);

		//if (boomerang.transform.position.x < transform.position.x){
		//	boomerang.transform.position = boomerangPoint.position;
		//}

		//boomerangOnCD = false;
    //}

}
