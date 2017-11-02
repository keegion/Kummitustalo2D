using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;
	public float smoothing = 5f;

	void Start()
	{
		offset = transform.position - player.transform.position;
	}

	void LateUpdate()
	{
		Vector3 targetCamPos = player.transform.position + offset;
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
