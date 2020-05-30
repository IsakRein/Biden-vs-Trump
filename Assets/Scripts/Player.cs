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
    
    [Header("Jump mechanics")]   
    public float jumpHeight;
    public float jumpLength;
    public float velocityX;
    public float velocityY = 0;
    public float fall_multiplier = 2.5f;
    private float jumpingSpeed = 0;
    private float gravity = -10f;
    
    [Header("Jetpack")]
    public bool jetpack_active;
    public float jetpack_speed;
    public float jetpack_max_speed;
    public float jetpack_fall_multiplier = 2.5f;

    [Header("Boost Up")]
    public float boost_up_multiplier;

    [Header("Effects")]
    public GameObject waterSplash;
    public GameObject explosion;
    public float waterSplashY;
    public List<GameObject> smoke_impacts = new List<GameObject>();
    public float smoke_impact_offset_X;
    public float smoke_impact_offset_Y;
    public bool is_jumping = false;
    public bool is_airbound = false;
    public float max_offset_ground_x;

    [Header("Other")]
    public int jumpCounter;
    private int frames_collision;
    private float seconds_jump;
    private bool trigger_jump;
    public bool collisionDetected;
    private Vector2 lastKnownVelocity;


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
    private bool play_smoke_impact;
    public Animator camera_animator;
    public bool is_dead;

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
        float t2 = time / (1 + Mathf.Sqrt(fall_multiplier));
        float t1 = time - t2;

        gravity = (2 * jumpHeight) / (fall_multiplier * t2*t2);
        jumpingSpeed = gravity * t1;

        // Debug.Log(t1 + " " + t2 + " " + gravity + " " + jumpingSpeed);
        //out: gravity, jumpForce
    }

    void Update()
    {
        if (gameActive)
        {
            if (jetpack_active)
            {

                if (Input.GetKey("space") || (Input.touchCount > 0))
                {
                    jetpack_fly();
                }
                else {
                    jetpack_fall();
                }
            }

            else {
                if (transform.position.y <= -50)
                {
                    // waterSplash.SetActive(true);
                    // waterSplash.transform.position = new Vector2(transform.position.x, waterSplashY);
                    gameManager.Death();
                }

                if (jumpCounter < 2)
                {
                    if (Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                    {
                        jump(jumpingSpeed);
                    }
                }
            }


            if (frames_collision == 1) { collisionDetected = true; }
            if (seconds_jump > 0.0 && trigger_jump) { jump(jumpingSpeed); trigger_jump = false; }

            frames_collision++;
            seconds_jump += Time.deltaTime;
            lastFrameTraveled = rb2d.position.x;
        }
    }

    void FixedUpdate()
    {
        if (is_airbound)
        {
            if (velocityY * gravity_direction > 0) { currentGravity = gravity * gravity_direction; }
            else if (jetpack_active) { currentGravity = gravity * jetpack_fall_multiplier * gravity_direction; }
            else { currentGravity = gravity * fall_multiplier * gravity_direction; }

            if (!jetpack_active) 
            {
                velocityY -= currentGravity * Time.fixedDeltaTime; 
            }
        }

        else { velocityY = 0; }
        animator.SetFloat("Speed", velocityY);

        transform.position = (rb2d.position + (new Vector2(velocityX, velocityY) * Time.fixedDeltaTime));
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
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
        play_smoke_impact = true;
        is_dead = false;
        camera_animator.SetBool("zoomed_out", false);
        camera_animator.Play("camera_main", 0, 0.0f);

        animator.SetBool("Jumping", false);
        animator.SetBool("Has Landed", false);
        animator.SetBool("Jetpack", false);
        animator.SetBool("is_airbound", true);
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
    
    void jump(float _jumpingSpeed)
    {
        startDistance = rb2d.position.x;
        velocityY = gravity_direction * _jumpingSpeed;
        is_jumping = true;
        is_airbound = true;
        animator.SetBool("Jumping", true);
        animator.SetBool("is_airbound", true);
        jumpCounter++;
    }

    void jetpack_fly() 
    {
        is_jumping = true;
        is_airbound = true;
        animator.SetBool("Jumping", true);
        animator.SetBool("is_airbound", true);
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
        //if (velocityY > -jetpack_max_speed)
        //{
        //    velocityY -= jetpack_speed * Time.deltaTime;
        //}
        //else 
        //{
        //    velocityY = -jetpack_max_speed - 3f;
        //}

        velocityY -= currentGravity * Time.deltaTime;

    }

    void jump_end()
    {      
        is_jumping = false;
        animator.SetBool("Jumping", false);

        // Debug.Log("Error: " + ((rb2d.position.x - startDistance) - jumpLength));

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
        is_dead = true;
    }
 
    void trigger_smoke_impact(float _smoke_impact_offset_X)
    {
        if (play_smoke_impact)
        {
            if (smoke_impact_last_num == smoke_impacts.Count - 1) { smoke_impact_last_num = 0; }
            else { smoke_impact_last_num++; }

            GameObject smoke_impact = Instantiate(smoke_impacts[smoke_impact_last_num]);
            smoke_impact.transform.position = new Vector2(_smoke_impact_offset_X, transform.position.y + (smoke_impact_offset_Y * gravity_direction));
            smoke_impact.transform.localScale = new Vector3(smoke_impact.transform.localScale.x, smoke_impact.transform.localScale.y * gravity_direction, smoke_impact.transform.localScale.z);
            play_smoke_impact = false;
            StartCoroutine(ResetSmokeImpact());
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        collision_count++;

        if (!is_dead) 
        {
            if (col.gameObject.tag == "Death") 
            { 
                death_explosion(); 
            }
            
            else
            {
                foreach (var item in col.contacts)
                {
                    // Collision right
                    if (item.normal == new Vector2(-1, 0))
                    {
                        death_explosion();
                    }

                    // Collision top
                    if (item.normal == new Vector2(0, -1))
                    {
                        velocityY = 0;
                    }

                    // Collision bottom
                    if (item.normal == new Vector2(0, 1)) {
                        is_airbound = false;
                        animator.SetBool("is_airbound", false);

                        // Smoke
                        if (transform.position.x + max_offset_ground_x < col.collider.bounds.min.x)
                        { trigger_smoke_impact(col.collider.bounds.min.x - max_offset_ground_x); }
                        else if (transform.position.x > col.collider.bounds.max.x + max_offset_ground_x)
                        { trigger_smoke_impact(col.collider.bounds.max.x + max_offset_ground_x); }
                        else { trigger_smoke_impact(transform.position.x + smoke_impact_offset_X); }

                        // Stop jumping
                        if (is_jumping) { jump_end(); }
                        
                        // Jumping again
                        if ((Input.GetKey("space") || Input.touchCount > 0) && !jetpack_active) { jump(jumpingSpeed); }
                    }

                    if (item.normal == new Vector2(0, -1) || item.normal == new Vector2(0, 1))
                    {
                        if (collision_count > 1)
                        {
                            death_explosion();
                        }
                    }
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        foreach (var item in col.contacts)
        {
            if (item.normal == new Vector2(0, -1))
            {
                velocityY = 0;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        collision_count -= 1;

        if (collision_count == 0) 
        {
            is_airbound = true;
            animator.SetBool("is_airbound", true);
        }
    }
    
    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.tag == "add_jump_square") 
        {
            prev_jump_counter = jumpCounter;
            jumpCounter = 1;
        }

        if (trig.tag == "jetpack") 
        {
            jetpack_active = true;
            camera_animator.SetBool("zoomed_out", true);
            animator.SetBool("Jetpack", true);
        }

        if (trig.tag == "jetpack_stop") 
        {
            jetpack_active = false;
            camera_animator.SetBool("zoomed_out", false);
            animator.SetBool("Jetpack", false);
        }

        if (trig.tag == "gravity_switch") 
        {
            gravity_direction = -1;
            transform.localScale = new Vector3(transform.localScale.x, -1 * transform.localScale.y, transform.localScale.z);
        }

        if (trig.tag == "boost_up") {
            jump(jumpingSpeed*boost_up_multiplier);
            jumpCounter = 1;
        }
    }

    void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.tag == "add_jump_square")  { RemoveExtraJump(prev_jump_counter); }
        if (trig.tag == "gravity_switch") { 
            gravity_direction = 1;
            transform.localScale = new Vector3(transform.localScale.x, -1 * transform.localScale.y, transform.localScale.z);
        }    
    }

    IEnumerator ResetSmokeImpact()
    {
        yield return new WaitForSeconds(.1f);
        
        play_smoke_impact = true;
    }

    IEnumerator RemoveExtraJump(int _prev_jump_counter)
    {
        yield return new WaitForSeconds(.1f);
        
        jumpCounter = _prev_jump_counter;
    }
    
}
