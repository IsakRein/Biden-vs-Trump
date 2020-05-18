using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{
    public Vector3 startVector = new Vector3(1370f, 10f, 1f);

    private GameManager gameManager;
    public bool gameActive;
    private LevelManager levelManager;

    private Rigidbody2D rb2d;
    private Animator animator;
    
    [Header("Jump mechanics")]
    
    public float jumpHeight;
    public float jumpLength;
    public float velocityX;
    public float velocityY = 0;

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
    public GameObject explosion;
    public float waterSplashY;
    
    public List<GameObject> smoke_impacts = new List<GameObject>();
    public float smoke_impact_offset_X;
    public float smoke_impact_offset_Y;

    public bool is_jumping = false;
    public bool is_airbound = false;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        levelManager = GameObject.Find("Level").GetComponent<LevelManager>();

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        frames_collision = 100;
        seconds_jump = 100;

        //in:  height, length, speed, fallMultipler

        float time = jumpLength/velocityX;
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

            if (jumpCounter < 2)
            {
                if (Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    jump();
                }
            }

            if (frames_collision == 1) { collisionDetected = true; } 
            if (seconds_jump > 0.0 && trigger_jump) { jump(); trigger_jump = false; } 
            
            frames_collision++;
            seconds_jump += Time.deltaTime;

            if (rb2d.position.x == lastFrameTraveled) 
            {
                death_explosion();
            }
            
            lastFrameTraveled = rb2d.position.x;
        }
    }

    public void StartGame()
    {
        gameActive = true;
        jumpCounter = 2;
        collisionDetected = false;
        transform.position = startVector;
        jumpCounter = 0;
        rb2d.simulated = true;
        rb2d.velocity = new Vector2(0f,0f);
        animator.enabled = true;
        animator.SetBool("Death", false);
        waterSplash.SetActive(false);
        is_jumping = true;
        is_airbound = true;
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
        gameObject.SetActive(false);
    }

    public System.DateTime startTime;

    float startDistance; 
    float currentGravity;
    float lastFrameTraveled;
    
    void FixedUpdate()
    {
        if (velocityY > 0) {  currentGravity = gravity; }
        else { currentGravity = gravity * fallMultiplier; }

        if (is_airbound) { /*Debug.Log("Delta X: " + (rb2d.position.x - startDistance));*/ velocityY -= currentGravity * Time.fixedDeltaTime; }
        else { velocityY = 0; }

        animator.SetFloat("Speed", velocityY);
        transform.position = (rb2d.position + (new Vector2(velocityX, velocityY) * Time.fixedDeltaTime));
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);

        if (collisionEntered2D) 
        {
            collisionEntered2D = false;

            float real_travel_x = lastFrameTraveled + velocityX * Time.fixedDeltaTime;

            transform.position = ((new Vector3(real_travel_x, rb2d.position.y, 1f)));
            //Debug.Log("Delta X 3: "  + (real_travel_x - startDistance));

            is_airbound = false;

            if (collisionEntered2Dcol.gameObject.tag == "Level")
            {   
                trigger_smoke_impact();

                if (is_jumping) 
                {   
                    jump_end();
                }
            
                if (Input.GetKey("space") || Input.touchCount > 0)
                {
                    jump();
                }
            }
        }
    }

    void jump()
    {
        //-temp-
        startDistance = rb2d.position.x;
        //------

        velocityY = jumpingSpeed;
        is_jumping = true;
        is_airbound = true;
        animator.SetBool("Jumping", true);

        jumpCounter++;
    }

    void jump_end()
    {      

        is_jumping = false;
        animator.SetBool("Jumping", false);

        jumpCounter = 0;
    }

    void death_explosion() 
    {
        GameObject explosion_inst = Instantiate(explosion);
        explosion_inst.transform.position = transform.position;
        gameManager.Death();
        animator.SetBool("Death", true);
        is_jumping = false;
        animator.SetBool("Jumping", false);
    }

    private int smoke_impact_last_num = 0;

    void trigger_smoke_impact()
    {
        if (smoke_impact_last_num == smoke_impacts.Count - 1) { smoke_impact_last_num = 0; }
        else { smoke_impact_last_num++; }

        GameObject smoke_impact = Instantiate(smoke_impacts[smoke_impact_last_num]);
        smoke_impact.transform.position = new Vector2(transform.position.x + smoke_impact_offset_X, transform.position.y + smoke_impact_offset_Y);   
    }

    void setHasLandedTrue() 
    {
        animator.SetBool("Has Landed", true);
    }

    void setHasLandedFalse() 
    {
        animator.SetBool("Has Landed", false);
    }

    private bool collisionEntered2D;
    private Collision2D collisionEntered2Dcol;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Death")
        {
            death_explosion();
        }
        
        if (collision_count == 0)
        {
            collisionEntered2D = true;
            collisionEntered2Dcol = col;
        }
        collision_count += 1;
    }


    public int collision_count = 0;

    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        collision_count -= 1;

        if (collision_count == 0) 
        {
            is_airbound = true;
            setHasLandedFalse();
        }
    }
    
    private int prev_jump_counter;

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.tag == "add_jump_square") 
        {
            prev_jump_counter = jumpCounter;
            jumpCounter = 1;
        }
    }

    void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.tag == "add_jump_square") 
        {
            jumpCounter = prev_jump_counter;
        }
    }
}
