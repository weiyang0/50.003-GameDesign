using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumPests : MonoBehaviour {

	public bool level_up= false;
	// Use this for initialization
	public int amount_sucked= 0;



	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//level_up= GameObject.Find<"GameManager">.;
	}


	void OnTriggerEnter2D(Collider2D other) {
		
	
		if (level_up && other!= null && other.gameObject.GetComponent<OrganismClass>().species.ToString() == "Pest") {
			this.amount_sucked++; //add 1 insect that kena sucked
			Destroy (other.gameObject);
		}
	}
}
