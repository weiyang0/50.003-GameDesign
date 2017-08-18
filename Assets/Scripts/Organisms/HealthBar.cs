﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class HealthBar : MonoBehaviour {

	public Slider slider;
	private int counter;

	public int MaxHealth = 500;
	public Image Fill;  // assign in the editor the "Fill"
	public Color MaxHealthColor = Color.green;
	public Color MinHealthColor = Color.red;

	private void Awake() {
		slider = gameObject.GetComponent<Slider>();
		counter = MaxHealth;            // just for testing purposes
	}

	private void Start() {
		slider.wholeNumbers = true;        // I dont want 3.543 Health but 3 or 4
		slider.minValue = 0f;
		slider.maxValue = MaxHealth;
		slider.value = MaxHealth;        // start with full health
	}

	private void Update() {
		UpdateHealthBar(counter);        // just for testing purposes
		counter--;                        // just for testing purposes
	}

	public void UpdateHealthBar(int val) {
		slider.value = val;
		Fill.color = Color.Lerp(MinHealthColor, MaxHealthColor, (float)val / MaxHealth);
	}
}
