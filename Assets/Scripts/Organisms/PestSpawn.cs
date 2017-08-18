using System.Collections;
using UnityEngine;

public class PestSpawn: MonoBehaviour {


	public GameObject[] hazards; 
	public Vector2 startPosField;
    public Vector2 endPosField;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

    private Transform pestHolder;

	private Transform fruitsHolder;
	public GameObject[] fruits; //attach the completed fruits here

	private bool gameOver;
	private bool restart;
    private bool paused;
    private int currLevel;

    private float speed;

	public void StartSpawn (int level)
	{
		gameOver = false;
        paused = false;
        currLevel = level;
        pestHolder = GameObject.Find("GameElements").transform;
		StartCoroutine (SpawnWaves ());
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
        if (gameOver)
            yield break;

        while (!gameOver)
		{
            if (!paused)
            {
                GameObject hazard = hazards[currLevel - 1];

                int rand = Random.Range(0, 4);

                Vector2 spawnPosition;
                switch (rand)
                {
                    case 0: // Spawn from Top
                        spawnPosition = new Vector2(Random.Range(startPosField.x, endPosField.x), endPosField.y);
                        break;

                    case 1: // Spawn from Bottom
                        spawnPosition = new Vector2(Random.Range(startPosField.x, endPosField.x), startPosField.y);
                        break;

                    case 2: // Spawn from Left
                        spawnPosition = new Vector2(startPosField.x, Random.Range(startPosField.y, endPosField.y));
                        break;

                    case 3: // Spawn from Right
                        spawnPosition = new Vector2(endPosField.x, Random.Range(startPosField.y, endPosField.y));
                        break;

                    default:
                        spawnPosition = new Vector2(0, 0);
                        break;
                }

                //MAKE A COORD FROM TOP LEFT BOTTOM OR RIGHT
                //Vector2 spawnPosition = new Vector2(Random.Range(startPosField.x, endPosField.x), Random.Range(startPosField.y, endPosField.y));

                //DIFFERENT SIDES ARE ACTIVATED DEPENDING ON LEVEL
                //Vector2 spawnPositionTOP = new Vector2 (Random.Range (startPosField.x, endPosField.x), endPosField.y);
                //Vector2 spawnPositionBOTTOM = new Vector2 (Random.Range (startPosField.x, endPosField.x), startPosField.y);
                //Vector2 spawnPositionLEFT = new Vector2 (startPosField.x, Random.Range(startPosField.x, endPosField.x));
                //Vector2 spawnPositionRIGHT = new Vector2(endPosField.x, Random.Range(startPosField.x, endPosField.x));

                //CALCULATE MANUALLY THE AREA TO SPAWN THE ENEMIES
                Quaternion spawnRotation = Quaternion.identity;
                GameObject instance = Instantiate(hazard, spawnPosition, spawnRotation);
                instance.transform.SetParent(pestHolder);
            }
            yield return new WaitForSeconds (spawnWait);
		}
	}

    public void OnEndGame()
    {
        this.gameOver = true;
    }

    public void TogglePause()
    {
        
        paused = !paused;
    }

}
