using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
    EnemyAI enemyAI;
	// Use this for initialization
	void Start () {
        enemyAI = GetComponent<EnemyAI>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(enemyAI.seeEnemy)
        {
            Debug.Log("Shooting");
        }
	}
    

}
