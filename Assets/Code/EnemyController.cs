using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int hp;
    public int hitDamage;
    public float speed;

    private Animator animator;
    private Rigidbody2D rigidBody;
    private CircleCollider2D collider;
    private GameObject player;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

        if (hp <= 0)
            Destroy(this.gameObject);

        //use a ray to detect the player, if found, chase after
        collider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y));
        if (hit.transform != null && hit.collider.tag == "Player")
        {
            rigidBody.velocity = new Vector2(player.transform.position.x - this.transform.position.x, player.transform.position.y - this.transform.position.y).normalized * speed;
        }
        else
        {
            rigidBody.velocity = new Vector2(0, 0);
        }
        collider.enabled = true;
    }

    public void TakeDamage()
    {
        hp--;

    }

    //when meet player, attack
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            animator.SetTrigger("Attack");
            collision.transform.SendMessage("TakeDamage", hitDamage);
        }
    }

}