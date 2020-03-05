using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;
    public float jumpingSpeed = 500;
    public float jumpingHoldingSpeed = 500;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        var vel = rb2d.velocity;
        var speed = vel.magnitude;

        animator.SetFloat("Speed", vel.y);

        if (Input.GetKeyDown("space"))
        {
            rb2d.AddForce(Vector3.up* jumpingSpeed);
        }
        if (Input.GetKey("space"))
        {
            rb2d.AddForce(Vector3.up * Time.deltaTime * jumpingHoldingSpeed);
        }
    }
}
