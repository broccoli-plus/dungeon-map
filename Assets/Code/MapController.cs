using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    //all component
    public GameObject[] floorArray;
    public GameObject[] obstacleArray;
    public GameObject[] wallArray;
    public GameObject[] doorArray;
    public GameObject exit;
    public GameObject emptyColiider;
    public GameObject[] enimyArray;
    public GameObject[] foodArray;
    public GameObject player;
    public GameObject fire;

    //map size
    public int xSize = 40;
    public int ySize = 40;

    //special items' number
    public int enimyNumber;
    public int obstacleNumber;
    public int foodNumber;

    //map creater
    private RandomMap map;
    //use this to make hierarchy look more concise
    private Transform mapHolder;
    private enum items
    {
        soil,
        outWall,
        wall,
        visitedPredoor,
        predoor,
        door,
        floor,
        corridor,
    };

    // Use this for initialization
    void Start () {
        
        //instantiate two specail component that dont need destroying when a new scene is loaded
        player = GameObject.Instantiate(player, new Vector3(map.roomArray[0].x + 1, map.roomArray[0].y + 1, -2), Quaternion.identity) as GameObject;
        DontDestroyOnLoad(player);

        fire = GameObject.Instantiate(fire, player.transform.position + new Vector3(1f, 0.2f, -2), Quaternion.identity) as GameObject;
        DontDestroyOnLoad(fire);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void InitMap()
    {
               
        mapHolder = new GameObject("MapHolder").transform;
        map = new RandomMap(xSize, ySize);

        player.GetComponent<PlayerController>().enabled=true;

        SetMap();
        SetItems();
    }

    //the room, wall, and door
    private void SetMap()
    {
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                switch (map.dungeonMap[i, j])
                {
                    case (int)items.soil:
                        break;
                    case (int)items.outWall:
                        break;
                    case (int)items.wall:
                        SetItem(wallArray, i, j);
                        break;
                    case (int)items.door:
                        SetItem(doorArray, i, j);
                        break;
                    case (int)items.corridor:
                        SetItem(floorArray, i, j);
                        break;
                    case (int)items.floor:
                        SetItem(floorArray, i, j);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    //player fire exit enemy
    private void SetItems()
    {
        //set player and fire at born point
        player.transform.position = new Vector3(map.roomArray[0].x + 1, map.roomArray[0].y + 1, -2);
        fire.transform.position = player.transform.position + new Vector3(1f, 0.2f, -2);

        //set exit

        GameObject.Instantiate(exit, new Vector3(map.roomArray[map.roomArray.Count - 1].x + 1, map.roomArray[map.roomArray.Count - 1].y + 1, -1), Quaternion.identity);

        //set enimy

        List<Vector2> floorList = new List<Vector2>();

        for (int i = 0; i < xSize; ++i)
        {
            for (int j = 0; j < ySize; ++j)
            {
                if (map.dungeonMap[i, j] == (int)items.floor)
                {
                    floorList.Add(new Vector2(i, j));
                }
            }
        }

        for (int i = 0; i < enimyNumber; ++i)
        {

            int index = UnityEngine.Random.Range(0, floorList.Count);
            SetItem(enimyArray, (int)floorList[index].x, (int)floorList[index].y, -2);
            floorList.RemoveAt(index);
        }

        for (int i = 0; i < obstacleNumber; ++i)
        {
            int index = UnityEngine.Random.Range(0, floorList.Count);
            SetItem(obstacleArray, (int)floorList[index].x, (int)floorList[index].y, -1);
            floorList.RemoveAt(index);
        }

        for (int i = 0; i < foodNumber; ++i)
        {
            int index = UnityEngine.Random.Range(0, floorList.Count);
            SetItem(foodArray, (int)floorList[index].x, (int)floorList[index].y, -1);
            floorList.RemoveAt(index);
        }
 
    }

    private void SetItem(GameObject[] objectArray, int x, int y, int z = 0)
    {
        int index = UnityEngine.Random.Range(0, objectArray.Length);
        GameObject go = GameObject.Instantiate(objectArray[index], new Vector3(x, y, z), Quaternion.identity) as GameObject;
        go.transform.SetParent(mapHolder);
    }
    
}
