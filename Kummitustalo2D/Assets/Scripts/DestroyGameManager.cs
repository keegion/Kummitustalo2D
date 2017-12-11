using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameManager : MonoBehaviour {

	private GameObject GameManager;

	void Start()
	{
		GameManager = GameObject.Find("GameManager(Clone)");
		// reset all player stuff
		Destroy(GameManager);
	}
}
