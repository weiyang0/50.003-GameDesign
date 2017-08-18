using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedShootPest : MonoBehaviour {

	private float speed = 10f;
	//private Vector3 speedRot = Vector3.right * 50f;
	public Vector3 TargetPosition;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, TargetPosition, speed * Time.deltaTime);

	}

}
