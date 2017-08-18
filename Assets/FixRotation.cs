using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour {

	Quaternion rotation;
	// Use this for initialization

	void Awake(){
		rotation = transform.rotation;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate()
	{
		transform.rotation = rotation;

	}
}
