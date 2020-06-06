using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.SetBool("fly_away", false);
        transform.localPosition = new Vector3(0,0,0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("fly_away", true);

            float direction = transform.localScale.x / Mathf.Abs(transform.localScale.x);
            float accX = Random.Range(0.3f, direction * -0.7f);
            float accY = Random.Range(0.5f, 1.3f);

            StartCoroutine(Fly_Away(accX, accY));
        }
    }


    private IEnumerator Fly_Away(float accX, float accY)
    {
        float speedX = 0;
        float speedY = 0;


        while (transform.localPosition.y < 250f)
        {
            speedX += accX;
            speedY += accY;

            transform.position = transform.position + new Vector3(speedX * Time.deltaTime, speedY * Time.deltaTime, 0);
            yield return null;
        }

        transform.parent.gameObject.SetActive(false);

        yield return null;
    }
 }
