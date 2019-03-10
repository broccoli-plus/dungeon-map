using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private bool faceRight;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private CircleCollider2D collider;

    public GameObject bullet;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
        faceRight = true;
        speed = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //dead
        if(GameController.Controller.food <= 0)
        {
            this.enabled = false;
            return;
        }

        //move
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        rigidBody.velocity = new Vector2(moveHorizontal, moveVertical) * speed;

        if ((moveHorizontal > 0 && faceRight == false) || (moveHorizontal < 0 && faceRight == true))
        {
            faceRight = !faceRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        //when press left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(this.transform.position.x + moveHorizontal, this.transform.position.y + moveVertical));
            if (hit.transform != null)
            {
                if (hit.collider.tag == "Obstacle" || hit.transform != null && hit.collider.tag == "Enemy")
                { 
                    hit.collider.SendMessage("TakeDamage");
                }
                
            }
            collider.enabled = true;
        } 
        else if (Input.GetMouseButtonDown(1) && GameController.Controller.level>1)
        {
            GameObject.Instantiate(bullet, this.transform.GetChild(0).position, Quaternion.identity);
        }


    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Damage");
        GameController.Controller.AddFood(damage * -1);
    }
}
