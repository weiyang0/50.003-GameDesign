using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WEIYANGType
{
    Attack, Attack2, Defense, Special
}
public enum WEIYANGSpecies
{
    Fruit, Pest
}
public enum WEIYANGAbility
{
    a, b, c, d, e, f, g
}

public class WEIYANG : MonoBehaviour
{
    // all of them have rigidbodies
    //COMMON ATTRIBUTES BETWEEN INSECTS AND PLANTS- make it public so can change in inspector directly for testing purposes

    //TODO: name and ability overlaps
    public string tagname; //USE CAPS
                           //TODO: Update any missing attributes
    public WEIYANGType type; //attack, defense?- TO FILL IN FROM INSPECTOR
    public WEIYANGSpecies species; //fruit OR pest
    public WEIYANGAbility ability; //ability will determine effect


    //TODO: change to private after everything works
    public int hp = 1000;  //begin at full HP -- display this on the UI
    public int damage; //damage to enemy (MORE POWERFUL, HIGHER VALUE)

    public int level;   //set according to type of insect
    public bool unlocked;

    //TODO: ACCESS GOURD'S AGE
    public int current_gourd_age;
    public int current_level;
    public int min_gourd_age; //when this value is reached, it gets unlocked
                              //fruit/pest is unlocked / spawned when the game reaches min gourd age
    public float TimeOfBirth;
    public float maturing_period = 100;
    public string state = "Baby";

    //TODO: a default halo component that gets activated with red/green colour when hp decrease/increase
    public Light halo;

    //TODO: ACCESS THE SPRITE RENDERER FROM HERE..



    //TODO: store the "bullet component" if applicablt

    //public GameObject bullet; //TODO: CREATE A CLASS FOR THE BULLET (LIKE A SUBSET OF ORGANISM)

    Animator anim;

    // Use this for initialization
    void Start()
    {
        //tag is a combination of the properties??
        //this.tag = tagname; // assign name to tag here so can detect for collision
        this.unlocked = false; //all fruits are unlocked at the start
        this.current_gourd_age = 0;
        //Get the time when object was spawned
        this.TimeOfBirth = Time.time; //system time
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //when gourd age reaches this object's min_gourd_age: it will get unlocked
        //HOW TO GET GLOBAL VARIABLE? (GOURD AGE)
        //REPLACE with public values of GOURD_AGE AND CURRENT LEVEL 
        //TODO: obtain current age from GourdManager

        if (current_gourd_age >= min_gourd_age && this.level >= current_level)
        {
            unlocked = true;
        }

        //Transition to adult state when maturing period is reached- and change the Sprite from baby to adult
        if (Time.time >= TimeOfBirth + maturing_period)
        {
            this.state = "Adult";
            anim.SetBool("isGrownUp", true);
        }//if true

        //Destroy object when HP reaches 0 or below
        if (this.hp <= 0)
        { //TODO: EDIT THIS
            Debug.Log("Destroyeddd");
            Destroy(this.gameObject); //SELF DESSTRUCT
        }
    }


    void FixedUpdate()
    {

    }

    //FOR ALL DYNAMIC & KINEMATIC  RIGIDBODIES- MAKE SUREIS TRIGGER IS FALSE
    void OnCollisionEnter2D(Collision2D coll)
    {
        //this.minus_hp (50);
        Debug.Log("HELLO");
        WEIYANG theirScript = coll.gameObject.GetComponent<WEIYANG>();
        Action(theirScript); //perform the relavant actions

        //this script will only work on THIS OBJECT- so there is no overlap
        //minus_hp (theirScript.damage); //receive the damage from them 
    }
    //As long as insect knocked into the fruit, update the hp accordingly

    //VAL ADDED FOR STRAWBERRY SEEDS
    void OnTriggerEnter2D(Collider2D kena)
    {
        if (kena.gameObject.tag == "StrawberrySeed")
        {
            Destroy(kena.gameObject);
            this.minus_hp(50);
        }
    }
    //https://forum.unity3d.com/threads/accessing-variables-from-another-object-solved.51143/
    //Probably the problem is GetComponent returns a Component object, not an AttackTemplate object. You need to say to the compiler to treat that component as an AttackTemplate script.

    //(GetComponent("AttackTemplate") as AttackTemplate).power

    //Also to get a component you can use GetComponent(typeof(AttackTemplate)) or GetComponent<AttackTemplate>() in C#. Both are more faster than passing a string to the method

    //TODO: BASED ON ABILITY(STRING)- THEY WILL ATTACK OR DEFEND IN DIFFERENT WAYS
    //to be called in OnCollide (other)
    //can ACCESS HP FROM HERE?
    void Action(WEIYANG other)
    { //TODO: pass in the enemy object from OnCollision method
      //only got 3 types- defend, attack, attack_2 (with bullets/forces applied to it)
      //TODO: NEED INTERACTION: ESP BLUEBERRY- WILL HEAL EVERYTHING AROUND IT
        if (other != null)
        {
            if (other.species.ToString() != this.species.ToString())
            { // if it is an enemy

                if (this.type.ToString() == "Defend")
                { //applies to FRUIT- BLUEBERRY HEALER ONLY
                    this.minus_hp(other.damage); //GET HURT ONLY

                }
                else if (this.type.ToString() == "Attack")
                { //HURT AND GET HURT
                    this.minus_hp(other.damage);
                    other.minus_hp(damage); //attack!

                }
                else if (this.type.ToString() == "Attack2")
                { //TODO: MOVE THIS OUT TO THE BULLET.
                  //OnCollide with the bullet! ONLY APPLIES FOR STRAWBERRY

                }
                else if (this.type.ToString() == "Special")
                {

                }
            }
            else
            {
                //Do nothing since its an ally
                other.hp = 999; //testing
            }
        }

    }

    //TODO: IMPLEMENT THE ATTACK OR DEFEND METHODS ACCORDING TO STRUCTURE OF FRUIT OR INSECT
    //Some attackes using itself, some attacks with bullet



    //TODO: MAKE ALL THE EFFECTS/HP FUNCTIONS- MODULARITY
    /// 
    /// other objects can call this
    public void add_hp(int increase)
    {
        this.hp += increase;
        this.activate_halo("green", 10, 5);//TODO: TEST IF THIS WORKS, THEN DO FOR MINUSHP ALSO
    }

    public void minus_hp(int decrease)
    {
        if(this.species.ToString() == "Fruit")
        {
            if (this.state.ToString() == "Baby")
            {
                Debug.Log("BabyHurt");
                anim.Play("BabyGrapeHurt");
                //anim.SetBool("babyIsHurt", true);
            }
            else if (this.state.ToString() == "Adult")
            {
                Debug.Log("AdultHurt");
                anim.Play("AdultGrapeHurt");
                //anim.SetBool("adultIsHurt", true);
            }
        }
        else if(this.species.ToString() == "Pest")
        {
            anim.Play("LadybugHurt");
        }
        
        this.hp -= decrease;
        //this.activate_halo("red", 10, 5);
        if (this.species.ToString() == "Fruit")
        {
            if (this.state.ToString() == "Baby")
            {
                Debug.Log("BabyIdle");
                anim.Play("BabyGrapeIdle");
                //anim.SetBool("babyIsHurt", false);
            }
            else if (this.state.ToString() == "Adult")
            {
                Debug.Log("AdultIdle");
                anim.Play("AdultGrapeIdle");
                //anim.SetBool("adultIsHurt", false);
            }
        }
        else if (this.species.ToString() == "Pest")
        {
            anim.Play("LadybugMove");
        }
    }

    public void activate_halo(string colour, int radius, float duration)
    {
        //this.halo.color = colour;
        //turn on halo temporarily
        //this.halo.enabled = true;
        //WaitForSeconds (duration);  //TODO: use a float?
        this.halo.enabled = false;

    }

    //TODO: ON COLLIDE RIGIDBODY :
    // call 1 function at  atime to test if it works, or else v hard to debug. comment out the rest.
    //void OnCollisionEnter2D(Collision2D coll){
    //this.hp = 4000;
    //}



    //TODO: FINALIZE STRUCTURE OF GAMEOBJECT ONLY AFTER ALL CODE IS WORKING FOR THE TESTING PEST AND GRAPE 
    //AND BLUEBERRY
}
