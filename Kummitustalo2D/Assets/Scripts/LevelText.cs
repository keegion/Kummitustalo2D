using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelText : MonoBehaviour {

	public GameObject LevelName;

	// Use this for initialization
	void Start () {
		StartCoroutine(LevelNameCD());
	}
	
	IEnumerator LevelNameCD()
	{
		yield return new WaitForSeconds(2f);
		LevelName.SetActive(false);
	}
}
