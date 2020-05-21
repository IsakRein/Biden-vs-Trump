using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{
    public Vector3 startVector = new Vector3(1370f, 10f, 1f);

    private GameManager gameManager;
    private bool gameActive;
    private LevelManager levelManager;

    private Rigidbody2D rb2d;
    private Animator animator;

    private int gravity_direction;
    
    [Header("Jetpack")]

    public bool jetpack_active;
    public float jetpack_speed;
    public float jetpack_max_speed;
    public GameObject jetpack;

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


    [Header("Unsorted trash")]

    public System.DateTime startTime;
    private float startDistance; 
    private float currentGravity;
    private float lastFrameTraveled;
    private int smoke_impact_last_num = 0;
    private bool collisionEntered2D;
    private Collision2D collisionEntered2Dcol;
    public int collision_count = 0;
    private int prev_jump_counter;

    private void Awake()
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

            if (jetpack_active) 
            {     
                if (Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    if (jumpCounter < 1) { jump(); Debug.Log("Jump"); }
                    // else { velocityY = 0; }
                }

                if (Input.GetKey("space") || (Input.touchCount > 0)) 
                {
                    jetpack_fly();
                }
                else {
                    jetpack_fall();
                }
            }

            else {
                if (jumpCounter < 2)
                {
                    if (Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                    {
                        jump();
                    }
                } 
            }
            

            if (frames_collision == 1) { collisionDetected = true; } 
            if (seconds_jump > 0.0 && trigger_jump) { jump(); trigger_jump = false; } 
            
            frames_collision++;
            seconds_jump += Time.deltaTime;
            lastFrameTraveled = rb2d.position.x;
        }
    }

    void FixedUpdate()
    {
        if (is_airbound)
        {
            if (jetpack_active) 
            {
                if (velocityY > 0) {  currentGravity = gravity; }
                else { currentGravity = gravity * fallMultiplier; }
            }
            else {
                if (velocityY * gravity_direction > 0) {  currentGravity = gravity * gravity_direction; }
                else { currentGravity = gravity * fallMultiplier * gravity_direction; }
                velocityY -= currentGravity * Time.fixedDeltaTime; 
            }
        }
        else { velocityY = 0; }

        if (jetpack_active) 
        { animator.SetFloat("Speed", 100); }
        else { animator.SetFloat("Speed", velocityY); }
        transform.position = (rb2d.position + (new Vector2(velocityX, velocityY) * Time.fixedDeltaTime));
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);

        if (collisionEntered2D) 
        {
            collisionEntered2D = false;

            float real_travel_x = lastFrameTraveled + velocityX * Time.fixedDeltaTime;   

            if (collisionEntered2Dcol.contacts[0].normal.y * gravity_direction == 1)  
            { 
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
            else {
                velocityY = 0;
            }
        }
    }

    public void StartGame()
    {
        gameActive = true;
        jumpCounter = 2;
        collisionDetected = false;
        collision_count = 0;
        transform.position = startVector;
        jumpCounter = 0;
        rb2d.simulated = true;
        rb2d.velocity = new Vector2(0f,0f);
        animator.enabled = true;
        animator.SetBool("Death", false);
        waterSplash.SetActive(false);
        is_jumping = true;
        is_airbound = true;
        jetpack_active = false;
        gravity_direction = 1;
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
    
    void jump()
    {
        startDistance = rb2d.position.x;

        velocityY = gravity_direction * jumpingSpeed;
        is_jumping = true;
        is_airbound = true;
        animator.SetBool("Jumping", true);

        jumpCounter++;
    }

    void jetpack_fly() 
    {
        if (velocityY < jetpack_max_speed)
        {
            velocityY += jetpack_speed * Time.deltaTime;
        }
        else 
        {
            velocityY = jetpack_max_speed + 3f;
        }
    }

    void jetpack_fall() 
    {
        if (velocityY > -jetpack_max_speed)
        {
            velocityY -= jetpack_speed * Time.deltaTime;
        }
        else 
        {
            velocityY = -jetpack_max_speed - 3f;
        }
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
 
    void trigger_smoke_impact()
    {
        if (smoke_impact_last_num == smoke_impacts.Count - 1) { smoke_impact_last_num = 0; }
        else { smoke_impact_last_num++; }

        GameObject smoke_impact = Instantiate(smoke_impacts[smoke_impact_last_num]);
        smoke_impact.transform.position = new Vector2(transform.position.x + smoke_impact_offset_X, transform.position.y + (smoke_impact_offset_Y * gravity_direction));   
        smoke_impact.transform.localScale = new Vector3(smoke_impact.transform.localScale.x, smoke_impact.transform.localScale.y * gravity_direction, smoke_impact.transform.localScale.z);
    }

    void setHasLandedTrue() 
    {
        animator.SetBool("Has Landed", true);
    }

    void setHasLandedFalse() 
    {
        animator.SetBool("Has Landed", false);
    }
   
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Death") { death_explosion(); }
        if (col.contacts.Length > 0) {
            foreach (var item in col.contacts)
            {
                if (item.normal == new Vector2(-1, 0)) { Debug.Log(item.normal); death_explosion(); }
                
                if (item.normal == new Vector2(0, -1) )
                {
                    velocityY = 0;
                }

                if (item.normal == new Vector2(0, -1) || item.normal == new Vector2(0, 1)) 
                {
                    if (collision_count > 0 ) 
                    {
                        death_explosion();
                    }
                }
            }
            
            if (collision_count == 0)
            {
                collisionEntered2D = true;
                collisionEntered2Dcol = col;
            }
            collision_count += 1;
        }
    }

    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        collision_count -= 1;

        if (collision_count == 0) 
        {
            is_airbound = true;
            setHasLandedFalse();
        }
    }
    
    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.tag == "add_jump_square") 
        {
            prev_jump_counter = jumpCounter;
            jumpCounter = 1;
        }

        if (trig.tag == "jetpack") { jetpack_active = true; jetpack.SetActive(true); }
        if (trig.tag == "jetpack_stop") { jetpack_active = false; jetpack.SetActive(false); }
        if (trig.tag == "gravity_switch") {
            gravity_direction = -1;
            transform.localScale = new Vector3(transform.localScale.x, -1 * transform.localScale.y, transform.localScale.z);
        }
    }

    void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.tag == "add_jump_square")  { jumpCounter = prev_jump_counter;}
        if (trig.tag == "gravity_switch") { 
            gravity_direction = 1;
            transform.localScale = new Vector3(transform.localScale.x, -1 * transform.localScale.y, transform.localScale.z);
        }    
    }
}
