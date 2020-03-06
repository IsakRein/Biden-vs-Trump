using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;
    public float jumpingSpeed = 500;
    public float jumpingHoldingSpeed = 500;
    public float gravity = 500;

    public bool jumpAvaliable;
    public bool liftAvaliable;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpAvaliable = true;
        liftAvaliable = true;
    }

    void Update()
    {
        var vel = rb2d.velocity;
        var speed = vel.magnitude;
        Physics2D.gravity = new Vector2(0, gravity);

        animator.SetFloat("Speed", vel.y);

        if (jumpAvaliable)
        {
            if (Input.GetKeyDown("space"))
            {
                rb2d.AddForce(Vector3.up * jumpingSpeed);
                jumpAvaliable = false;
            }         
        }

        if (!jumpAvaliable && !Input.GetKey("space"))
        {
            liftAvaliable = false;
        }

        if (liftAvaliable)
        {
            if (Input.GetKey("space"))
            {
                rb2d.AddForce(Vector3.up * Time.deltaTime * jumpingHoldingSpeed);
            }
        }
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.transform.tag);
        if (col.transform.tag == "Level")
        {
            jumpAvaliable = true;
            liftAvaliable = true;
        }
    }
}
