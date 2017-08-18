
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour {

    public static ChangeLevel instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    private int currLevel;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int goToWhichLevel){
        
        currLevel = goToWhichLevel;
        if (goToWhichLevel == 2)
            SceneManager.LoadScene("InGame2");
        else
            SceneManager.LoadScene("InGame");
    }

    public int GetLevel()
    {
        return currLevel;
    }

}
