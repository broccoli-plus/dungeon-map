using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public Sprite openSprite;
    public Sprite doorSprite;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    //open the door when player enter
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = openSprite;      
        }
            
    }

    //close the door when player leave
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = doorSprite;
        }
            
    }
}
