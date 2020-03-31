using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics_Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float speed;
    public float height;
    public float time;

    public float jumpRotation;

    private bool rotating;
    private float rot = 0;
    private float jumpingSpeed;
    private float gravity;

    void Start()
    {
        jumpingSpeed = (4*height)/time;
        gravity = (8*height)/(time*time);
        Application.targetFrameRate = 300;
    }

    void Update()
    {
        Physics2D.gravity = new Vector2(0, -gravity);

        if (Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) 
        {
            rb2d.velocity += new Vector2(0, jumpingSpeed);
            rotating = true;
        }

        if (rotating) 
        {
            rot += ((-jumpRotation/time)*Time.deltaTime);
            if (rot < -jumpRotation) 
            {
                rot = 0;
                rotating = false;
            }
            transform.eulerAngles = new Vector3(0, 0, rot);   
        }

        rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        Debug.Log(rb2d.velocity);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Debug.Log(collisionInfo.contacts[0].normal);

        if (collisionInfo.contacts[0].normal.y < 0.8) 
        {
            gameObject.SetActive(false);
        }

        if (Input.GetKey("space") || Input.touchCount > 0) 
        {
            rb2d.velocity += new Vector2(0, jumpingSpeed);
            
            rotating = true;
            rot = 0;
        }
    }
}
