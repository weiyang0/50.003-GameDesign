using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget: MonoBehaviour {

    private Transform target;
    private bool paused;
    private bool gameOver;
    private float speed;

    private GameManager gameManager;

    void Start () {
	    target = GameObject.FindGameObjectWithTag("Gourd").transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        speed = gameManager.speedOfPests;
        paused = false;
        gameOver = false;
    }

    void Update () {

        if (!paused && !gameOver)
        {
            //transform.Rotate (speedRot * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            Vector3 dir = Vector3.RotateTowards(transform.position, transform.position - target.position, speed * Time.deltaTime, 0.0F);
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(dir, Vector3.forward),
                    Time.deltaTime * speed
                );
            }
        }
    }

    public void TogglePause()
    {
        paused = !paused;
    }

    public void OnEndGame()
    {
        gameOver = true;
    }


    void OnCollisionEnter2D(Collision2D coll) {

	    //this part is handled by the VacuumPests script attached to the Gourd	
    }

}