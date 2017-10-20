using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public int lives;
    internal int livesLeft;

    void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () {
        livesLeft = lives;
    }
	
	// Update is called once per frame
	void Update () {


	}
}
