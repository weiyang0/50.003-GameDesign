using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSpawner : MonoBehaviour {

	//TODO: create a field of radius in which when the insect comes in, strawberry will spawn a seed thaat MOVES TOWARDS
	//the insect to decrease its HP
	public GameObject strawberry_seed;
	private GameObject seed_to_fire; //to instantiate when a pest is spotted
	public Collider2D current_enemy_pos;

	public Vector2 spawnValues; //range of X Y values in which enemies will be spawned- outside the field area
	public int seedCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public float degree= 20f;
	public bool attacking= false;
	//TODO: UPDATE ALL THE UI TEXT AND SCORE. HP STUFF IS SUPPOSED TO BE ON "LIVING THINGS" ALONE. WHEN A INSECT TOUCHES A FRUIT (AND CLASSIFY BY THE TYPE)


	//SET THE RANGE OF THE FIELD AREA TO DETERMINE SPAWNING POSITION


	void Start ()
	{
		//StartCoroutine (SpawnWaves ());
	}


	void Update () {
		//TODO: UPDATE THE PEST'S POSITION!
		if (attacking && seed_to_fire!=null) {
			seed_to_fire.GetComponent<SeedShootPest> ().TargetPosition = current_enemy_pos.transform.position;
		}
		//for(float i = -180f; i < 180f; i += degree) {
			//Quaternion rotation = Quaternion.AngleAxis(i, transform.forward);
			//Vector3 shotPosition = transform.position + rotation* (new Vector3(1,0,0)) ;
			//Instantiate(strawberry_seed, shotPosition, rotation * transform.rotation);
		//}
	}

	//TODO: AUTOSHOOT BULLETS BY ROTATING THE STRAWBERRY
	void OnTriggerEnter2D(Collider2D incoming) {
		//ACCESS THE ORGANISM CLASS SCRIPT TO DECREASE HP
		if (incoming.gameObject!=null){
			if (incoming.gameObject.GetComponent<OrganismClass> ().species.ToString() == "Pest") { //only act on pests, not moving fruit parts
				//Instantiate(strawberry_seed, this.gameObject.transform); //GENERATE THE SEED at its location!
				//PASS IN THE TRANSFORM TO THE STRAWBERRY SEED...
				//strawberry_seed.GetComponent<SeedShootPest>().pesty= incoming.gameObject.transform;
				//pass in the insect's position
				ShootSeed(incoming.gameObject);
				current_enemy_pos = incoming.gameObject.GetComponent<Collider2D>();
			}
		}
	}

	public void ShootSeed(GameObject target){
		Debug.Log("Shoot Seed");
		attacking = true;
		seed_to_fire = (GameObject)Instantiate (strawberry_seed, transform.position, strawberry_seed.transform.rotation);
		//PASS IN THE INSECT'S POSITION TO THE SCRIPT ATTACHED TO THE SEED
		seed_to_fire.GetComponent<SeedShootPest>().TargetPosition = target.transform.position;
		//float speed = fireSpeed * Time.deltaTime;
	}
	//TODO: SPAWNWAVES OF BULLET? OR ONLY TO INSECT

	void OnCollisionEnter2D(Collision2D pest){ //SELF-DESTRUCT AND RESET
		//seed_to_fire
		if (seed_to_fire != null) { //IF THE SEED HAS BEEN FIRED, THEN CAN DESTROY IT UPON COLLISION
			Destroy (seed_to_fire); //destroy the seed or SET IT TO NULL?
			seed_to_fire = null;
		}
		//attacking = false;
		//TODO: FIND OUT HOW TO SET THE REFERENCE TO NULL
	}

}