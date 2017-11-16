using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {
    EnemyAI enemyAI;
    public GameObject enemyBullet;
	public Transform bulletSpawn;
	public float bulletCDTime = 1;
    bool bulletOnCD;
	// Use this for initialization
	void Start () {
        enemyAI = GetComponent<EnemyAI>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(enemyAI.shooting && !bulletOnCD)
        {
            bulletOnCD = true;
			GameObject temps = (GameObject)Instantiate(enemyBullet, bulletSpawn.position, transform.rotation);
            Destroy(temps, 5f);
            StartCoroutine(BulletCD());
        }
	}
    IEnumerator BulletCD()
    {

		yield return new WaitForSeconds(bulletCDTime);
        bulletOnCD = false;
    }
    

}
