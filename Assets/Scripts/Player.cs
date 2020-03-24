using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManager gameManager;
    private bool gameActive;

    private Rigidbody2D rb2d;
    private Animator animator;
    public float jumpingSpeed = 0;
    public float gravity = -10f;
    [Range(0.0f, 20.0f)]
    public float fallMultiplier = 2.5f;
    [Range(0.0f, 20.0f)]
    public float lowJumpMultiplier = 2f;

    public bool jumpAvaliable;
    private int waitFrames;
    public bool collisionDetected;

    private Vector2 lastKnownVelocity;

    public GameObject waterSplash;


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (gameActive)
        {
            if (transform.position.y <= -50)
            {
                waterSplash.SetActive(true);
                waterSplash.transform.position = new Vector2(transform.position.x, -42);
                gameManager.Death();
            }


            Physics2D.gravity = new Vector2(0, gravity);

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

            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
            }

            else if (rb2d.velocity.y > 0 && !Input.GetKey("space") && Input.touchCount == 0)
            {
                rb2d.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            if (waitFrames > 2)
            {
                collisionDetected = true;
            }
            waitFrames++;
        }
    }

    public void StartGame()
    {
        gameActive = true;
        jumpAvaliable = false;
        collisionDetected = false;
        transform.position = new Vector2(-gameManager.horizontalSize/2 + 30f, 10f);
        rb2d.simulated = true;
        rb2d.velocity = new Vector2(0f,0f);
        animator.enabled = true;
        waterSplash.SetActive(false);

    }

    public void PauseGame()
    {
        gameActive = false;
        rb2d.simulated = false;
        animator.enabled = false;
        lastKnownVelocity = rb2d.velocity;
    }
    
    public void ResumeGame() 
    {
        gameActive = true;
        rb2d.simulated = true;
        animator.enabled = true;
        rb2d.velocity = lastKnownVelocity;
    }

    public void Death()
    {
        gameActive = false;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (collisionDetected)
        {
            if (col.transform.tag == "Level")
            {
                jumpAvaliable = false;
            }

            waitFrames = 0;
            collisionDetected = false;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.tag == "Level")
        {
            jumpAvaliable = true;
        }

        if (col.gameObject.tag == "Death")
        {
            animator.SetBool("Death", true);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (collisionDetected)
        {
            if (col.transform.tag == "Level")
            {
                jumpAvaliable = true;
            }

            if (col.gameObject.tag == "Death")
            {
                animator.SetBool("Death", true);
            }

            waitFrames = 0;
            collisionDetected = false;
        }
    }
}
