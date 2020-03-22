using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;
    [Range(0.0f, 20.0f)]
    public float jumpingSpeed = 0;
    [Range(-0.0f, -20.0f)]
    public float gravity = -10f;
    [Range(0.0f, 20.0f)]
    public float fallMultiplier = 2.5f;
    [Range(0.0f, 20.0f)]
    public float lowJumpMultiplier = 2f;

    public LevelGenerator levelGenerator;

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
        Physics2D.gravity = new Vector2(0,gravity);

        var vel = rb2d.velocity;
        var speed = vel.magnitude;

        animator.SetFloat("Speed", vel.y);

        if (jumpAvaliable)
        {
            if (Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                rb2d.velocity = (Vector3.up * jumpingSpeed);
                jumpAvaliable = false;
            }    
        }

        if (!jumpAvaliable && !Input.GetKey("space") && Input.touchCount == 0)
        {
            liftAvaliable = false;
        }

        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rb2d.velocity.y > 0 && !Input.GetKey("space") && Input.touchCount == 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Level")
        {
            jumpAvaliable = true;
            liftAvaliable = true;
        }
        if (col.gameObject.tag == "Death")
        {
            levelGenerator.levelScrollingSpeed = 0;
            animator.SetBool("Death", true);
        }
    }
}
