using UnityEngine;

//public enum Phase
//{
//    Baby, Teenager, Adult, Elderly
//}

public class GourdManager : MonoBehaviour
{
    private int hp;
    private float percentageAge;
    public string currPhase;
    

    public float currProgress;
    private bool gameOver;

    public float maxDist = 8.0f;
    public float pulseSpeed = 30.0f;

    void Start()
    {
        gameOver = false;
        currPhase = "Baby";
    }

    // Update is called once per frame
    void Update()
    {

        //this.phase = Phases [currLevel];
        this.hp = gameObject.GetComponent<OrganismClass>().get_curr_hp();  //get the updated hp
        if (this.hp <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver(); // end game
        }
        float progressRatio = GameObject.Find("ProgressBar").GetComponent<ProgressBar>().getCurrPercentage();

    }


    public float getPercentageAge()
    {   //to be used by levelSelectManager
        return this.percentageAge; //currLevel
    }

    public void setPercentageAge(float age)
    {
        this.percentageAge = age;
        //Debug.Log(this.percentageAge);
    }

    public void OnEndGame()
    {
        this.gameOver = true;
    }
}
