using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int level = 1;
    public int food = 1000;
    public int totalLevel = 3;

    private static GameController controller;
    private GameObject player;
    private MapController mapController;
    private Text foodText;
    private Text failText;

    private bool win = false;

    public static GameController Controller
    {
        get
        {
            return controller;
        }
    }

    // Use this for initialization
    void Start()
    {
        controller = this;
    }

    void Awake()
    {
        controller = this;
        DontDestroyOnLoad(this);
        
    }
	
	// Update is called once per frame
	void Update () {

        // if win, press right mouse button to restart the game, reset win tag
        if (win)
        {
            failText.enabled = true;
            failText.text = "YOU WIN !";
            //player.GetComponent<PlayerController>().enabled = false;
            if (Input.GetKeyDown(KeyCode.R))
            {
                level = 1;
                food = 500;
                win = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        //if lose, press right mouse button to restart the game
        else if (food <= 0)
        {
            food = 0;
            failText.enabled = true;
            failText.text = "YOU ARE DEAD !";
            if (Input.GetKeyDown(KeyCode.R))
            {
                level = 1;
                food = 500;
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
	}

    public void InitGame()
    {
        //get map controller
        mapController = GetComponent<MapController>();
        //get two text
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        failText = GameObject.Find("FailText").GetComponent<Text>();
        failText.enabled = false;
        //initailize the map
        mapController.InitMap();    
    }

    public void AddFood(int foodChange)
    {
        food += foodChange;
        if (food < 0)
            food = 0;
        foodText.text = "THE NO."+level+"  Food : " + food;
    }

    public void NextLevel()
    {
        ++level;
        //passed every level
        if (level == totalLevel+1)
        {
            win = true;
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
      
}
