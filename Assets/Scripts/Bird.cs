using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Animator animator;
    private GameManager gameManager;
    private IEnumerator fly_away = null;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        animator.enabled = true;
        animator.SetBool("fly_away", false);
        transform.localPosition = new Vector3(0,0,0);
        if (fly_away != null) {
            StopCoroutine(fly_away);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("fly_away", true);

            float direction = transform.localScale.x / Mathf.Abs(transform.localScale.x);
            float accX = Random.Range(direction * -0.2f, direction * -0.3f);
            float accY = Random.Range(0.5f, 1.5f);

            fly_away = Fly_Away(accX, accY, direction);

            StartCoroutine(fly_away);
        }
    }


    private IEnumerator Fly_Away(float accX, float accY, float direction)
    {
        // Debug.Log("gameManager.gameActive: " + gameManager.gameActive);

        float speedX = direction * -10;
        float speedY = 15;

        while (transform.localPosition.y < 250f)
        {
            if (gameManager.gameActive) 
            {
                animator.enabled = true;
                speedX += accX;
                speedY += accY;
                transform.position = transform.position + new Vector3(speedX * Time.deltaTime, speedY * Time.deltaTime, 0);
            }
            else {
                animator.enabled = false;
            }
            yield return null;
        }

        transform.parent.gameObject.SetActive(false);

        yield return null;
    }
}
