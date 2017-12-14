using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomeranging : MonoBehaviour {
	public GameObject boomerangPrefab;
	public Transform boomerangPoint;
	internal GameObject boomerang;
	EnemyBoomerang boomerangController;

	void Start () {
		boomerang = Instantiate(boomerangPrefab, boomerangPoint.position, transform.rotation);

		boomerangController = boomerang.GetComponent<EnemyBoomerang>();
		boomerangController.horseBoy = gameObject;
		boomerangController.boomerangPoint = boomerangPoint;
		         
		EnemyBoomerang controller = boomerang.GetComponent<EnemyBoomerang>();
	}

}
