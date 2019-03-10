using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // the distance between palyer and camera
    private Vector3 distance;
    //target position
    private Transform target;
    //use this to initailize the game
    public GameObject gameManager;

    // Use this for initialization
    void Start()
    {
        distance = new Vector3(2, 2, -4);
        // the initializer are only instantiate once
        if(GameController.Controller == null)
        {
            //Debug.Log("create");
            GameObject.Instantiate(gameManager);          
        }
        GameController.Controller.InitGame();
    }

    // Update is called once per frame to fix the position
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        this.transform.position = target.position + distance;
    }

}
