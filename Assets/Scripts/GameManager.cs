using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{

    /*^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * ---------------------------------------------
     * 
     * -----  INITIALIZES GAME FOR EACH LEVEL  -----
     * 
     * ---------------------------------------------
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^*/

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private int level = 1;                                  //Current level number
    public float speedOfPests = 13.0f;
    public string mainSceneName;
    public GameObject progressbar;
    public GameObject gourd;

    public int durationOfGame = 10;


    //Awake is always called before any Start functions
    void Awake()
    {
        GameObject LevelSelectManager = GameObject.Find("LevelSelectManager");
        if (LevelSelectManager != null)
        {
            ChangeLevel lvlScript = LevelSelectManager.GetComponent<ChangeLevel>();
            level = lvlScript.GetLevel();
            Debug.Log("Currently selected: level " + level);
            Destroy(LevelSelectManager);
        }

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 
        SetupScene(level);

    }

    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene(int level)
    {
        //Sets up the UI
        UISetup();
    }


    /*^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * ---------------------------------------------
     * 
     * ---------   SET-UP USER INTERFACE   ---------
     * 
     * ---------------------------------------------
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^*/

    private GameObject gameOverContainer;
    private Text txt_score;
    private Text txt_gameOver_header;
    private Text txt_gameOver_subheader;
    private Text txt_gameOver_score;
    private Image bg_gameOver;
    private int score;
    private bool gameOver;
    private bool restart;
    private bool paused;
    private bool won;

    private GameObject menu;
    private GameObject btn_pause, btn_close;
    private GameObject plantBox;

    void UISetup()
    {
        txt_score = GameObject.Find("txt_score").GetComponent<Text>();

        txt_gameOver_header = GameObject.Find("txt_gameOver_header").GetComponent<Text>();
        txt_gameOver_subheader = GameObject.Find("txt_gameOver_subheader").GetComponent<Text>();
        txt_gameOver_score = GameObject.Find("txt_gameOver_score").GetComponent<Text>();
        bg_gameOver = GameObject.Find("GameOver").GetComponent<Image>();
        gameOverContainer = GameObject.Find("GameOver");
        gameOverContainer.SetActive(false);

        menu = GameObject.Find("Menu");
        menu.SetActive(false);

        btn_pause = GameObject.Find("btn_pause");
        btn_close = GameObject.Find("btn_close");
        btn_close.SetActive(false);

    }

    public void ToggleMenu()
    {
        progressbar.GetComponent<ProgressBar>().TogglePauseGame();
        TogglePauseGame();
        btn_pause.SetActive(!btn_pause.activeInHierarchy);
        btn_close.SetActive(!btn_close.activeInHierarchy);
        menu.SetActive(!menu.activeInHierarchy);
    }

    public void TogglePauseGame()
    {
        paused = !paused;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
        {
            go.SendMessage("TogglePause", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
    }

    public void UpdateScore()
    {
        txt_score.text = "Score: " + score;
    }

    public bool isPaused()
    {
        return this.paused;
    }

    public bool getWon()
    {
        return this.won;
    }

    public void GameOver()
    {
        gameOverContainer.SetActive(true);
        if (gourd.GetComponent<OrganismClass>().get_curr_hp() >= 1)
        {
            won = true;
            txt_gameOver_header.text = "You have won!";
        }
        else
        {
            won = false;
            txt_gameOver_header.text = "You have lost..";
        }
        txt_gameOver_score.text = this.score.ToString();


        this.gameOver = true;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
        {
            go.SendMessage("OnEndGame", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    // Initialization
    void Start()
    {
        paused = false;
        gameOver = false;
        restart = false;
        score = 0;
        UpdateScore();
        progressbar.GetComponent<ProgressBar>().setDuration(durationOfGame);
        gameObject.GetComponent<PestSpawn>().StartSpawn(level);
    }

    // Update is called once per frame
    void Update()
    {
        float currPercentage = progressbar.GetComponent<ProgressBar>().getCurrPercentage();
        if (!gameOver && !paused)
            this.score += 888;

        if (currPercentage == 1.0f)
        {
            GameOver();
        }
        if (!gameOver)
            gourd.GetComponent<GourdManager>().setPercentageAge(currPercentage);

        UpdateScore();
    }

}


// Using Serializable allows us to embed a class with sub properties in the inspector.
[System.Serializable]
public class Count
{
    public int minimum;             //Minimum value for our Count class.
    public int maximum;             //Maximum value for our Count class.


    //Assignment constructor.
    public Count(int min, int max)
    {
        minimum = min;
        maximum = max;
    }
}