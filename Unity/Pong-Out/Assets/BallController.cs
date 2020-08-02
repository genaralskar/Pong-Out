using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 100f;

    public Vector2 velocity = new Vector2(1f, .5f);

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //rb.velocity = velocity * moveSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        return;
        //check which direction it's hit from, top, bottom, left, right
        Debug.Log($"tag= {collision.transform.tag}, {collision.gameObject}");
        if(collision.transform.CompareTag("Bounds Caps"))
        {
            velocity.y *= -1;
        }
        else
        {
            velocity.x *= -1;
        }

        rb.velocity = velocity * moveSpeed;


        //change velocity based on hit direction
    }
}
