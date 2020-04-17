using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{
    private GameManager gameManager;
    public bool gameActive;
    private LevelManager levelManager;

    private Rigidbody2D rb2d;
    private Animator animator;
    
    [Header("Jump mechanics")]
    
    public float jumpHeight;
    public float jumpLength;
    public float speed;
    public float fallMultiplier = 2.5f;

    private float jumpingSpeed = 0;
    private float gravity = -10f;

    [Header("Other")]

    public int jumpCounter;
    private int frames_collision;
    private float seconds_jump;
    private bool trigger_jump;

    public bool collisionDetected;

    private Vector2 lastKnownVelocity;

    [Header("Effects")]
    public GameObject waterSplash;
    public float waterSplashY;
    public GameObject landingSmoke;
    public float landingSmokeX;
    public float landingSmokeY;

    private bool is_jumping = true;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        levelManager = GameObject.Find("Level").GetComponent<LevelManager>();

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        frames_collision = 100;
        seconds_jump = 100;

        //in:  height, length, speed, fallMultipler

        float time = jumpLength/speed;
        float t2 = time / (1 + Mathf.Sqrt(fallMultiplier));
        float t1 = time - t2;

        gravity = (2 * jumpHeight) / (fallMultiplier * t2*t2);
        jumpingSpeed = gravity * t1;

       // Debug.Log(t1 + " " + t2 + " " + gravity + " " + jumpingSpeed);

        //out: gravity, jumpForce
    }

    void Update()
    {
        if (gameActive)
        {
            if (transform.position.y <= -50)
            {
                waterSplash.SetActive(true);
                waterSplash.transform.position = new Vector2(transform.position.x, waterSplashY);
                gameManager.Death();
            }

            Physics2D.gravity = new Vector2(0, -gravity);

            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);


            var vel = rb2d.velocity;

            animator.SetFloat("Speed", vel.y);

            if (jumpCounter < 2)
            {
                if (Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    jump();
                }
            }

            if (rb2d.velocity.y < 0)
            {
                rb2d.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;                
            }
            
            if (frames_collision == 1) { collisionDetected = true; } 
            if (seconds_jump > 0.0 && trigger_jump) { jump(); trigger_jump = false; } 
            
            frames_collision++;
            seconds_jump += Time.deltaTime;
        }

        else
        {
            rb2d.velocity = new Vector2(0, 0);
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
        animator.SetBool("Death", false);
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

    public System.DateTime startTime;
    public int timeBeforeTop = 5;
    public int timeAfterTop;

    void jump()
    {
        is_jumping = true;
        animator.SetBool("Jumping", true);

        rb2d.velocity = (Vector3.up * jumpingSpeed);
        jumpCounter++;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Debug.Log("distance: " + (startDistance - levelManager.gameObject.transform.position.x));

        if (col.gameObject.tag == "Death")
        {
            animator.SetBool("Death", true);
        }

        else
        { 
            if (is_jumping)
            {
                GameObject landingSmokeInst = Instantiate(landingSmoke);

                landingSmokeInst.transform.position = new Vector2(transform.position.x + landingSmokeX, transform.position.y + landingSmokeY);
            }

            if (Input.GetKey("space") || Input.touchCount > 0)
            {
                seconds_jump = 0;
                trigger_jump = true;
            }
        }

        is_jumping = false;
        animator.SetBool("Jumping", false);
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
            is_jumping = false;
            animator.SetBool("Jumping", false);

        }
    }
 
    float startDistance; 

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform.tag == "Level")
        {       
            startDistance = levelManager.gameObject.transform.position.x;
            jumpCounter = 1;
        }
    }
}
