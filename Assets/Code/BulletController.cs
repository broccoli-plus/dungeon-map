using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public int speed;

	// Use this for initialization
	void Start () {

        

        Vector3 vecA = GetComponent<Transform>().position;//Input.mousePosition;
        Vector3 vecB = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vecB.z = vecA.z;
        Vector3 direction = vecB - vecA;                                    
        //float angle = Vector3.Angle(direction, Vector3.up);              
        

        float angle = Vector3.Angle(direction, Vector3.up);             
        direction = Vector3.Normalize(direction);                          
        float dot = Vector3.Dot(direction, Vector3.right);                  
        if (dot > 0)
            angle = 360 - angle;
        //Debug.LogWarning("vecA：" + vecA.ToString() + ", vecB：" + vecB.ToString() + ", angle: " + angle.ToString());

        this.GetComponent<Transform>().Rotate(0, 0, angle);
        

        GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x,direction.y)*speed;

        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Fire").GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.SendMessage("TakeDamage", 1);
        }
        if (collision.transform.tag == "Obstacle")
        {
            collision.transform.SendMessage("TakeDamage", 1);
        }
        
         Destroy(this.gameObject);
        
    }
}
