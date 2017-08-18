using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Attack, Attack2, Defense, Special
}
public enum Species
{
    Fruit, Pest
}
public enum Ability
{
    a, b, c, d, e, f, g
}

public enum Name {
	Gourd,  Blueberry, Banana, Durian, Grape, Watermelon, Strawberry, Orange, Ladybug, Caterpillar, Rat
}



public class OrganismClass : MonoBehaviour {
	// all of them have rigidbodies
	//COMMON ATTRIBUTES BETWEEN INSECTS AND PLANTS- make it public so can change in inspector directly for testing purposes

	//TODO: Update any missing attributes
	public Type type; //attack, defense?- TO FILL IN FROM INSPECTOR
	public Species species; //fruit OR pest
	public Ability ability; //ability will determine effect
	public Name nickname;

	//TODO: change to private after everything works
	public int max_hp = 1000;  //begin at full HP -- display this on the UI
    private int hp;
	public int damage; //damage to enemy (MORE POWERFUL, HIGHER VALUE)

	public int level;   //set according to type of insect
	public bool unlocked; //FOR ZHENYANG DRAGGABLE TO ACCESS
    private bool gameOver;

    int age;
	//TODO: ACCESS GOURD'S AGE
	public int current_gourd_age;
	public int current_level;
	public int min_gourd_age; //when this value is reached, it gets unlocked
	//fruit/pest is unlocked / spawned when the game reaches min gourd age
	public float TimeOfBirth;
	public float maturing_period =10f;
	public string state= "Baby";

    private AudioSource collision_sound;
    private AudioSource healing_sound;
    private AudioSource evolve_sound;

    Animator anim;

    private GameManager gameManager;
    private bool paused;

	void Awake(){
        AudioSource[] audios = GetComponents<AudioSource>();
        collision_sound = audios[0];
        healing_sound = audios[1];
        evolve_sound = audios[2];
    }
    // Use this for initialization
    void Start () {
		//tag is a combination of the properties??
		this.unlocked= false; //all fruits are unlocked at the start
		this.current_gourd_age=0;
		//Get the time when object was spawned
		this.TimeOfBirth= Time.time; //system time
        anim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        this.gameOver = false;

        this.hp = max_hp;
    }

    // Update is called once per frame
    void Update () {
        paused = gameManager.isPaused();

        //when gourd age reaches this object's min_gourd_age: it will get unlocked
        //HOW TO GET GLOBAL VARIABLE? (GOURD AGE)
        //REPLACE with public values of GOURD_AGE AND CURRENT LEVEL 
        //TODO: obtain current age from GourdManager

        if (current_gourd_age>= min_gourd_age && this.level >= current_level) {
			unlocked = true;
		}

		//Transition to adult state when maturing period is reached- and change the Sprite from baby to adult
		if (Time.time >= TimeOfBirth + maturing_period)
		{
            evolve_sound.Play();
            this.state="Adult";
            anim.SetBool("isGrownUp", true);
        }//if true


		if (nickname.ToString()=="Gourd") {
			if (gameManager.getWon()) {
                Debug.Log("babyIsGrownUp");
                this.gameObject.GetComponent<GourdManager>().currPhase = "Teenager";
                anim.SetBool("babyIsGrownUp", true);
                //Debug.Log("babyIsGrownUp");
			
			}
			else if (gameManager.getWon()) {
                this.gameObject.GetComponent<GourdManager>().currPhase = "Adult";
                anim.SetBool("teenIsGrownUp", true);
			}else if (gameManager.getWon()) {
                this.gameObject.GetComponent<GourdManager>().currPhase = "Elderly";
                anim.SetBool("adultIsGrownUp", true);
			}//else if (Time.time >= TimeOfBirth + maturing_period * 8) {
				//anim.SetBool("isGrownUp", true);
			//}
		}
		//Destroy object when (1) HP reaches 0 or below, (2) game is over, or (3) it is not a Gourd
		if ((this.hp <= 0 || this.gameOver) && !this.nickname.ToString().Equals("Gourd")) {
            Debug.Log("Destroyeddd " + this.nickname.ToString());
            gameManager.AddScore(543210);
            Destroy(this.gameObject);
		}
	}
	

	void FixedUpdate() {

	}

	//FOR ALL DYNAMIC & KINEMATIC  RIGIDBODIES- MAKE SUREIS TRIGGER IS FALSE
	void OnCollisionEnter2D(Collision2D coll) {

		GetComponent<AudioSource> ().Play ();

		//this.minus_hp (50);
        if (!paused)
        {

            OrganismClass theirScript = coll.gameObject.GetComponent<OrganismClass>();
            if (coll.gameObject.GetComponent<OrganismClass>().species.ToString() == this.species.ToString())
            {
                Physics2D.IgnoreCollision(coll.gameObject.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>(), true);
            }
            Action(theirScript); //perform the relavant actions

            //this script will only work on THIS OBJECT- so there is no overlap
            //minus_hp (theirScript.damage); //receive the damage from them
			if (coll.gameObject.tag == "StrawberrySeed") {
				Destroy (coll.gameObject);
				this.minus_hp (50);
			}
        }


    }
	//As long as insect knocked into the fruit, update the hp accordingly

	//VAL ADDED FOR STRAWBERRY SEEDS
	void OnTriggerEnter2D(Collider2D kena){

        if (!paused)
        {
			if (kena.gameObject.tag == "StrawberrySeed") {
				Destroy (kena.gameObject);
				this.minus_hp (50);
			} else {
				OrganismClass theirScript = kena.gameObject.GetComponent<OrganismClass>();
				Action (theirScript);
			}
        }
	}
	//https://forum.unity3d.com/threads/accessing-variables-from-another-object-solved.51143/
	//Probably the problem is GetComponent returns a Component object, not an AttackTemplate object. You need to say to the compiler to treat that component as an AttackTemplate script.

	//(GetComponent("AttackTemplate") as AttackTemplate).power

	//Also to get a component you can use GetComponent(typeof(AttackTemplate)) or GetComponent<AttackTemplate>() in C#. Both are more faster than passing a string to the method

	//TODO: BASED ON ABILITY(STRING)- THEY WILL ATTACK OR DEFEND IN DIFFERENT WAYS
	//to be called in OnCollide (other)
	//can ACCESS HP FROM HERE?
	void Action(OrganismClass other){ //TODO: pass in the enemy object from OnCollision method
                                      //only got 3 types- defend, attack, attack_2 (with bullets/forces applied to it)
                                      //TODO: NEED INTERACTION: ESP BLUEBERRY- WILL HEAL EVERYTHING AROUND IT

		if (other != null) {
            if (!other.species.ToString().Equals(this.species.ToString())) { // if it is an enemy
                if (this.type.ToString () == "Defend") { //applies to FRUIT- BLUEBERRY HEALER ONLY
					this.minus_hp (other.damage); //GET HURT ONLY
				} else if (this.type.ToString ().Equals("Attack")) { //HURT AND GET HURT
					this.minus_hp (other.damage); 
					other.minus_hp (damage); //attack!
					if (this.nickname.ToString() == "Watermelon") {
						this.gameObject.GetComponent<PointEffector2D> ().forceMagnitude = 5;
					}

                } else if (this.type.ToString () == "Attack2") { //TODO: MOVE THIS OUT TO THE BULLET.
                                                                 //OnCollide with the bullet! ONLY APPLIES FOR STRAWBERRY

                } else if (this.type.ToString () == "Special") {

                } 
			} else {
				//Do nothing since its an ally
				other.hp = 999; //testing
			}
		}
			
	}

	//TODO: IMPLEMENT THE ATTACK OR DEFEND METHODS ACCORDING TO STRUCTURE OF FRUIT OR INSECT
	//Some attackes using itself, some attacks with bullet



	//TODO: MAKE ALL THE EFFECTS/HP FUNCTIONS- MODULARITY
	
    public int get_curr_hp()
    {
        return this.hp;
    }
    public void set_curr_hp(int new_hp)
    {
        this.hp = new_hp;
    }

    public void add_hp(int increase){
        healing_sound.Play();
        this.hp += increase;
	}

	public void minus_hp(int decrease){

        collision_sound.Play();
        playHurtAnim();

        this.hp -= decrease;

        playIdleAnim();
    }

    public void playHurtAnim()
    {
        if (this.species.ToString() == "Fruit")
        {
            if (this.nickname.ToString() == "Gourd")
            {
                if (this.gameObject.GetComponent<GourdManager>().currPhase == "Baby")
                {
                    anim.Play("BabyGourdHurt");
                }
                else if (this.gameObject.GetComponent<GourdManager>().currPhase == "Teenager")
                {
                    anim.Play("TeenagerGourdHurt");
                }
                else if (this.gameObject.GetComponent<GourdManager>().currPhase == "Adult")
                {
                    anim.Play("AdultGourdHurt");
                }
                else if (this.gameObject.GetComponent<GourdManager>().currPhase == "Elderly")
                {
                    anim.Play("ElderlyGourdHurt");
                }
            }
            else if (this.nickname.ToString() == "Grape")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyGrapeHurt");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultGrapeHurt");
                }
            }
            else if (this.nickname.ToString() == "Banana")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyBananaHurt");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultBananaHurt");
                }
            }
            else if (this.nickname.ToString() == "Strawberry")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyStrawberryHurt");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultStrawberryHurt");
                }
            }
            else if (this.nickname.ToString() == "Orange")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyOrangeHurt");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultOrangeHurt");
                }
            }
            else if (this.nickname.ToString() == "Blueberry")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyBlueberryHurt");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultBlueberryHurt");
                }
            }
            else if (this.nickname.ToString() == "Durian")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyDurianHurt");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultDurianHurt");
                }
            }
            else if (this.nickname.ToString() == "Watermelon")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyWatermelonHurt");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultWatermelonHurt");
                }
            }
        }
        else if (this.species.ToString() == "Pest")
        {
            if (this.nickname.ToString() == "Ladybug")
            {
                anim.Play("LadybugHurt");
            }
            else if (this.nickname.ToString() == "Caterpillar")
            {
                anim.Play("CaterpillarHurt");
            }
            else if (this.nickname.ToString() == "Rat")
            {
                anim.Play("RatHurt");
            }
        }
    }

    public void playIdleAnim()
    {
        if (this.species.ToString() == "Fruit")
        {
            if (this.nickname.ToString() == "Gourd")
            {
                if (this.gameObject.GetComponent<GourdManager>().currPhase == "Baby")
                {
                    anim.Play("BabyGourdIdle");
                }
                else if (this.gameObject.GetComponent<GourdManager>().currPhase == "Teenager")
                {
                    anim.Play("TeenagerGourdIdle");
                }
                else if (this.gameObject.GetComponent<GourdManager>().currPhase == "Adult")
                {
                    anim.Play("AdultGourdIdle");
                }
                else if (this.gameObject.GetComponent<GourdManager>().currPhase == "Elderly")
                {
                    anim.Play("ElderlyGourdIdle");
                }
            }
            else if (this.nickname.ToString() == "Grape")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyGrapeIdle");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultGrapeIdle");
                }
            }
            else if (this.nickname.ToString() == "Banana")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyBananaIdle");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultBananaIdle");
                }
            }
            else if (this.nickname.ToString() == "Strawberry")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyStrawberryIdle");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultStrawberryIdle");
                }
            }
            else if (this.nickname.ToString() == "Orange")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyOrangeIdle");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultOrangeIdle");
                }
            }
            else if (this.nickname.ToString() == "Blueberry")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyBlueberryIdle");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultBlueberryIdle");
                }
            }
            else if (this.nickname.ToString() == "Durian")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyDurianIdle");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultDurianIdle");
                }
            }
            else if (this.nickname.ToString() == "Watermelon")
            {
                if (this.state.ToString() == "Baby")
                {
                    anim.Play("BabyWatermelonIdle");
                }
                else if (this.state.ToString() == "Adult")
                {
                    anim.Play("AdultWatermelonIdle");
                }
            }
        }
        else if (this.species.ToString() == "Pest")
        {
            if (this.nickname.ToString() == "Ladybug")
            {
                anim.Play("LadybugMove");
            }
            else if (this.nickname.ToString() == "Caterpillar")
            {
                anim.Play("CaterpillarMove");
            }
            else if (this.nickname.ToString() == "Rat")
            {
                anim.Play("RatMove");
            }
        }
    }

    public void OnEndGame()
    {
        this.gameOver = true;
    }

	//TODO: ON COLLIDE RIGIDBODY :
	// call 1 function at  atime to test if it works, or else v hard to debug. comment out the rest.
	//void OnCollisionEnter2D(Collision2D coll){
		//this.hp = 4000;
	//}



	//TODO: FINALIZE STRUCTURE OF GAMEOBJECT ONLY AFTER ALL CODE IS WORKING FOR THE TESTING PEST AND GRAPE 
	//AND BLUEBERRY
}
