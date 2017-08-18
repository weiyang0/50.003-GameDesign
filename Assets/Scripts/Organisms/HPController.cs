using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController : MonoBehaviour {

	private float max_hp;
	private float hp;
	private float current_ratio;
    public GameObject healthbar;
	public GameObject bar;

	void Start () {
		if (this.gameObject != null) {
            max_hp = this.gameObject.GetComponent<OrganismClass>().max_hp;
            hp = this.gameObject.GetComponent<OrganismClass>().get_curr_hp();
        }
	}
	
	// Update is called once per frame
	void Update () {
        hp = this.gameObject.GetComponent<OrganismClass>().get_curr_hp();
		current_ratio = hp / max_hp;
		updateHPbar ();
	}

    public void updateHPbar() {
        if (gameObject.GetComponent<OrganismClass>().nickname.ToString().Equals("Gourd")) { 
            //Debug.Log("//Before:" + bar.localScale.x);
            bar.transform.localScale = new Vector3(current_ratio, 1, 1);
            //Debug.Log("After:" + bar.localScale.x);
        }
    }
}
