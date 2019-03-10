using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{

    public Sprite damageSprite;
    private int hp = 2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame 
    void Update()
    {
        
    }

    //change sprite
    private void TakeDamage()
    {
        --hp;
        if (hp == 1)
        {
            GetComponent<SpriteRenderer>().sprite = damageSprite;
        }
        if (hp == 0)
        {
            Destroy(this.gameObject);
        }
    }

}
