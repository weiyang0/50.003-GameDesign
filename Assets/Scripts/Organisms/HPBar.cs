using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour {

	public float healthBarLength;
	public Slider healthSlider;

	// Use this for initialization
	void Start () {
		healthBarLength = 50f;
	}
	
	// Update is called once per frame
	void Update () {
		healthSlider.value -= 1f;
	}

	void OnGUI(){
		GUI.Box (new Rect (10, 10, healthBarLength, 20), "Hello");

	}
}


