using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : MonoBehaviour {
    Animator anim;
	public AudioClip attack;
	AudioSource source;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "Player")
		{
			anim.SetBool("attack", true);
			source.PlayOneShot(attack, 0.3f);
		}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            anim.SetBool("attack", false);
    }
}
