using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public Vector3 debugStartVector = new Vector3(0f, 15f, 0f);
    public static Vector3 startVector = new Vector3(0f, 15f, 0f);
    private GameManager gameManager;
    private bool gameActive;
    private LevelManager levelManager;
    private Rigidbody2D rb2d;
    private Animator animator;
    public Environment environment;
    
    [Header("Add jump")]
    public Material outline_material;   
    public float outline_thickness;
    public bool outline_active;

    
    [Header("Jump mechanics")]   
    public float jumpHeight;
    public float jumpLength;
    public float velocityX;
    public float velocityY = 0;
    public float fall_multiplier = 2.5f;
    private float jumpingSpeed = 0;
    private float gravity = -10f;
    public int gravity_direction;
    public bool is_in_gravitybox;
    public int gravity_trig_count;
    public int gravity_regular_trig_count;
    public int no_double_jump_trig_count;

    [Header("Jetpack")]
    public bool jetpack_active;
    public float jetpack_speed;
    public float jetpack_max_speed;
    public float jetpack_fall_multiplier = 2.5f;
    public GameObject fire_effect;
    public ParticleSystem smoke_effect;

    [Header("Boost Up")]
    public float boost_up_multiplier;

    [Header("Effects")]
    public GameObject waterSplash;
    public GameObject waterSplash2;
    public GameObject lavaSplash;

    public GameObject explosion;
    public GameObject explosion_regular;
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

    [Header("UI")]
    public UI_TextToSpriteIndex progress_text;
    public Transform levelWon; 
    public float levelWonX; 

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
    public CameraScript cameraScript;
    public bool is_dead;

    public BoxCollider2D feet_collider; 

    public Transform previous_best_line;

    private Main main;
    private LocalSoundManager localSoundManager;


    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        levelManager = GameObject.Find("Level").GetComponent<LevelManager>();

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        frames_collision = 100;
        seconds_jump = 100;

        float time = jumpLength/velocityX;
        float t2 = time / (1 + Mathf.Sqrt(fall_multiplier));
        float t1 = time - t2;

        gravity = (2 * jumpHeight) / (fall_multiplier * t2*t2);
        jumpingSpeed = gravity * t1;

        main = FindObjectOfType<Main>().GetComponent<Main>();
        localSoundManager = FindObjectOfType<LocalSoundManager>();
        cameraScript = camera_animator.gameObject.GetComponent<CameraScript>();
        environment = GameObject.Find("Environment").GetComponent<Environment>();
    }

    void Start()
    {
        levelWon = GameObject.FindGameObjectWithTag("Level Won").transform;
        levelWonX = levelWon.position.x;
    }

    void Update()
    {
        if (gameActive)
        {
            cameraScript.FollowPlayer();
            if (transform.position.x > levelWonX) 
            {
                gameManager.percentage = 100;
                if (gameManager.gameActive) {
                    gameManager.GameWon();
                }
            }
            else 
            {
                gameManager.percentage = (int)(100*transform.position.x/levelWonX);
            }

            progress_text.updateFromDict(gameManager.percentage.ToString() + "%");

            if (jetpack_active)
            {
                if (Input.GetKey("space") || (Input.touchCount > 0))
                {
                    jetpack_fly();

                    // if (Input.GetKey("space") || !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    // {
                    //     jetpack_fly();
                    // }
                }
                else {
                    jetpack_fall();
                }
                
            }

            else {
                if (transform.position.y <= -150)
                {
                    gameManager.Death();
                }

                if (jumpCounter < 2)
                {
                    if (Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                    {
                        jump(jumpingSpeed);
                        // if (Input.GetKeyDown("space") || !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                        // {
                        //     jump(jumpingSpeed);
                        // }
                    
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

    void SwitchGravity(int direction)
    {
        gravity_direction = direction;
        transform.localScale = new Vector3(transform.localScale.x, direction * Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }

    void FixedUpdate()
    {
        if (gameActive) {
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
            animator.SetFloat("Speed", velocityY * gravity_direction);

            transform.position = (rb2d.position + (new Vector2(velocityX, velocityY) * Time.fixedDeltaTime));
            transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        }
    }
    
    public void StartGame()
    {
        gameManager.previous_highscore = PlayerPrefs.GetInt(gameManager.current_level_key);
        gameManager.previous_highscore_x = PlayerPrefs.GetFloat(gameManager.current_level_key + "_x");

        Debug.Log("gameManager.previous_highscore " + gameManager.previous_highscore);
        Debug.Log("gameManager.previous_highscore_x " + gameManager.previous_highscore_x);

        jumpCounter = 2;
        collisionDetected = false;
        collision_count = 0;
        transform.position = startVector;
        environment.StartGame();
        gameActive = true;

        startVector = debugStartVector;   
        jumpCounter = 0;
        rb2d.simulated = true;
        rb2d.velocity = new Vector2(0f,0f);
        animator.enabled = true; 
        is_jumping = true;
        is_airbound = true;
        jetpack_active = false;
        gravity_direction = 1;
        gravity_trig_count = 0;
        gravity_regular_trig_count = 0;
        no_double_jump_trig_count = 0;
        SwitchGravity(1);
        is_in_gravitybox = false;
        velocityY = 0;
        play_smoke_impact = true;
        is_dead = false;
        camera_animator.SetBool("zoomed_out", false);
        camera_animator.Play("camera_main", 0, 0.0f);
        animator.SetBool("Jumping", false);
        animator.SetBool("Has Landed", false);
        animator.SetBool("Jetpack", false);
        animator.SetBool("is_airbound", true);
        outline_material.SetFloat("_Thickness", 0);
        outline_active = false;

        fire_effect.SetActive(false);
        smoke_effect.Stop(); 

        if (0 < gameManager.previous_highscore && gameManager.previous_highscore < 100) 
        {
            previous_best_line.gameObject.SetActive(true);
            previous_best_line.position = new Vector2(gameManager.previous_highscore_x, 0);

        }
        else {
            previous_best_line.gameObject.SetActive(false);
        }

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
        collision_count = 0;
    }
    
    public void Death()
    {
        gameActive = false;
        
        #if UNITY_IPHONE || UNITY_ANDROID
        if (main.settings_vibration) {
            Handheld.Vibrate();
        }
        #endif
        gameObject.SetActive(false);
        localSoundManager.Stop("JetpackActive");

        saveScore();
    }

    public void GameWon() 
    {
        saveScore();
    }

    IEnumerator fakeMove() 
    {
        float speedX = 120;
        float time = 0;

        while (time < 10f)
        {
            time += Time.deltaTime;
            
            transform.position += new Vector3(speedX * Time.deltaTime, 0, 0);

            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false); 
        yield return null;
    }

    public void setSavePoint()
    {
        startVector = transform.position;
    }

    public void saveScore() 
    {
        if (PlayerPrefs.GetInt(gameManager.current_level_key) < gameManager.percentage) 
        {
            gameManager.previous_highscore = gameManager.percentage;
            gameManager.previous_highscore_x = transform.position.x;
            PlayerPrefs.SetInt(gameManager.current_level_key, gameManager.percentage);
            PlayerPrefs.SetFloat(gameManager.current_level_key + "_x", transform.position.x);
        }
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
        outline_material.SetFloat("_Thickness", 0);
        localSoundManager.Play("Jump");
    }

    void jetpack_fly() 
    {
        if (Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            localSoundManager.Play("JetpackActive");
            fire_effect.SetActive(true);
            smoke_effect.Play();
        }

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
        if (Input.GetKeyUp("space") || (Input.touchCount == 0))
        {
            localSoundManager.Stop("JetpackActive");
            fire_effect.SetActive(false);
            smoke_effect.Stop();
        }

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
        GameObject explosion_inst;
        if (jetpack_active) {explosion_inst = Instantiate(explosion);}
        else {explosion_inst = Instantiate(explosion_regular);}

        explosion_inst.transform.position = transform.position;
        gameManager.Death();
        animator.SetBool("Death", true);
        is_jumping = false;
        animator.SetBool("Jumping", false);
        is_dead = true;
        //localSoundManager.Play("Explosion2");
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
        localSoundManager.Play("Landing");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (gameActive) 
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
                            // foreach (var cont in col.contacts)
                            // { 
                            //     Debug.Log(cont.normal);
                            //     Debug.Log(cont.point);
                            //     Debug.Log(new Vector2(transform.position.x, transform.position.y) - cont.point);
                            //     Debug.Log("-");
                            // }
                            // Debug.Log(item.point);
                            // Debug.Log(new Vector2(transform.position.x, transform.position.y) - item.point);
                            // Debug.Log(transform.position.x - item.point.x);
                            // Debug.Log("-");

                            if ((transform.position.y - item.point.y) < 12) {
                                death_explosion();
                            }
                        }

                        // Collision tops
                        if (item.normal == new Vector2(0, -1 * gravity_direction))
                        {
                            velocityY = 0;
                        }

                        // Collision bottom
                        if (item.normal == new Vector2(0, 1 * gravity_direction)) {
                            is_airbound = false;
                            animator.SetBool("is_airbound", false);
                            

                            float feet_left = feet_collider.bounds.min.x;
                            float feet_right = feet_collider.bounds.max.x;
                            float feet_bottom = feet_collider.bounds.min.y;

                            Vector2 left_ray_start = new Vector2(feet_left+0.1f, feet_bottom + 1f);
                            Vector2 right_ray_start = new Vector2(feet_right-0.1f, feet_bottom + 1f);

                            RaycastHit2D ray_left = Physics2D.Raycast(left_ray_start, -Vector2.up, 2f);
                            RaycastHit2D ray_right = Physics2D.Raycast(right_ray_start, -Vector2.up, 2f);

                            if (ray_left.collider != null && ray_right.collider != null)
                            {
                                trigger_smoke_impact(transform.position.x + smoke_impact_offset_X); 
                            }
                            else if (ray_right.collider == null && ray_left.collider != null)
                            {   
                                Vector2 start_point = right_ray_start + (-Vector2.up * 2f);

                                RaycastHit2D ray_right_to_left = Physics2D.Raycast(start_point, Vector2.left, feet_right-feet_left);
                                trigger_smoke_impact(ray_right_to_left.point.x + max_offset_ground_x);
                                
                            }
                            else if (ray_left.collider == null && ray_right.collider != null)
                            {
                                Vector2 start_point = left_ray_start + (-Vector2.up * 2f);                   
                    
                                RaycastHit2D ray_left_to_right = Physics2D.Raycast(start_point, Vector2.right, feet_right-feet_left);
                                trigger_smoke_impact(ray_left_to_right.point.x - max_offset_ground_x);
                            }

                            // Smoke
                            // if (transform.position.x + max_offset_ground_x < col.collider.bounds.min.x)
                            // { trigger_smoke_impact(col.collider.bounds.min.x - max_offset_ground_x); }
                            // else if (transform.position.x > col.collider.bounds.max.x + max_offset_ground_x)
                            // { trigger_smoke_impact(col.collider.bounds.max.x + max_offset_ground_x); }
                            // else { trigger_smoke_impact(transform.position.x + smoke_impact_offset_X); }

                            // Stop jumping
                            if (is_jumping) { jump_end(); }
                            
                            // Jumping again
                            if ((Input.GetKey("space") || Input.touchCount > 0) && !jetpack_active && no_double_jump_trig_count == 0) { 
                                jump(jumpingSpeed); 
                                // if (Input.GetKey("space") || !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                                // {
                                //     jump(jumpingSpeed); 
                                // }
                            }
                        }

                        if (item.normal == new Vector2(0, -1) || item.normal == new Vector2(0, 1))
                        {
                            if (collision_count > 1)
                            {
                                //death_explosion();
                            }
                        }
                    }
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (gameActive)  {
            foreach (var item in col.contacts)
            {
                if (item.normal == new Vector2(0, -1*gravity_direction))
                {
                    velocityY = 0;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (gameActive)
        {
            is_airbound = true;
            animator.SetBool("is_airbound", true);
                
            // collision_count -= 1;

            // if (collision_count == 0) 
            // {
            //     is_airbound = true;
            //     animator.SetBool("is_airbound", true);
            // }
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (gameActive)
        {
            if (trig.tag == "add_jump_square") 
            {
                outline_material.SetFloat("_Thickness", outline_thickness);
                prev_jump_counter = jumpCounter;
                jumpCounter = 1;
                localSoundManager.Play("Glass");
                localSoundManager.Play("PowerUp2");
            }

            if (trig.tag == "jetpack") 
            {
                jetpack_active = true;
                camera_animator.SetBool("zoomed_out", true);
                animator.SetBool("Jetpack", true);
                localSoundManager.Play("JetpackStart");
            }

            if (trig.tag == "jetpack_stop") 
            {
                jetpack_active = false;
                fire_effect.SetActive(false);
                smoke_effect.Stop();
                localSoundManager.Stop("JetpackActive");
                camera_animator.SetBool("zoomed_out", false);
                animator.SetBool("Jetpack", false);
            }

            if (trig.tag == "boost_up")
            {
                jump(jumpingSpeed*boost_up_multiplier);
                jumpCounter = 1;
                localSoundManager.Play("BoostUp");
            }

            if (trig.tag == "gravity_switch")
            {
                gravity_trig_count += 1;
                if (gravity_trig_count==3  ){localSoundManager.Play("Gravity");}
 
                SwitchGravity(-1);
            }

            if (trig.tag == "gravity_switch_regular")
            {
                gravity_regular_trig_count += 1;
                SwitchGravity(1);
            }

            if (trig.tag == "no_double_jump")
            {
                no_double_jump_trig_count += 1;
            }

            if (trig.tag == "water") {
                GameObject water_inst = Instantiate(waterSplash);
                water_inst.transform.position = new Vector2(transform.position.x, trig.bounds.max.y + 9f);
                gameManager.Death();
                animator.SetBool("Death", true);
                is_jumping = false;
                animator.SetBool("Jumping", false);
                is_dead = true;
            }

            if (trig.tag == "water2") {
                GameObject water_inst2 = Instantiate(waterSplash2);
                water_inst2.transform.position = new Vector2(transform.position.x, trig.bounds.max.y + 9f);
                gameManager.Death();
                animator.SetBool("Death", true);
                is_jumping = false;
                animator.SetBool("Jumping", false);
                is_dead = true;
            }

            if (trig.tag == "lava") {
                GameObject lava_inst = Instantiate(lavaSplash);
                lava_inst.transform.position = new Vector2(transform.position.x, trig.bounds.max.y + 9f);
                gameManager.Death();
                animator.SetBool("Death", true);
                is_jumping = false;
                animator.SetBool("Jumping", false);
                is_dead = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.tag == "add_jump_square") { RemoveExtraJump(prev_jump_counter); }
        if (trig.tag == "gravity_switch")
        {
            gravity_trig_count -= 1;

            if (gravity_trig_count == 0)
            {
                SwitchGravity(1);
            }
        }

        if (trig.tag == "gravity_switch_regular")
        {
            gravity_regular_trig_count -= 1;
        }

        if (trig.tag == "no_double_jump")
        {
            no_double_jump_trig_count -= 1;
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
