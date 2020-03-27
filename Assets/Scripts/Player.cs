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

    public int jumpCounter;
    private int frames_collision;
    private float seconds_jump;
    private bool trigger_jump;

    public bool collisionDetected;

    private Vector2 lastKnownVelocity;

    public GameObject waterSplash;
    
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        frames_collision = 100;
        seconds_jump = 100;
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

            if (jumpCounter < 2)
            {
                if (Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    rb2d.velocity = (Vector3.up * jumpingSpeed);
                    Debug.Log(jumpCounter);
                    jumpCounter++;
                }
            }

            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
            }

            //else if (rb2d.velocity.y > 0 && !Input.GetKey("space") && Input.touchCount == 0)
            //{
            //    rb2d.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            //}
            
            if (frames_collision == 1) { collisionDetected = true; } 
            if (seconds_jump > 0.0 && trigger_jump) { rb2d.velocity = (Vector3.up * jumpingSpeed); trigger_jump = false; } 
            
            frames_collision++;
            seconds_jump += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        gameActive = true;
        jumpCounter = 2;
        collisionDetected = false;
        transform.position = new Vector2(-gameManager.horizontalSize/2 + 35f, 10f);
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


    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Enter");


        if (col.gameObject.tag == "Death")
        {
            animator.SetBool("Death", true);
        }

        else if (Input.GetKey("space")) 
        {
            seconds_jump = 0;
            trigger_jump = true;
        }


    }

    
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.tag == "Level")
        {
            jumpCounter = 0;
        }

        if (col.gameObject.tag == "Death")
        {
            animator.SetBool("Death", true);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform.tag == "Level")
        {
            jumpCounter = 1;
        }
    }
}
