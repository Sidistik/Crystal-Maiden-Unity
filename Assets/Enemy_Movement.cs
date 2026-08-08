using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Transform player;
    private bool isChasing;

    private Animator animator;

    private Vector2 lastDirection; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isChasing && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;

            animator.SetBool("isWalking", true);
            animator.SetFloat("inputX", direction.x);
            animator.SetFloat("inputY", direction.y);

            
            if (direction.magnitude > 0.01f)
                lastDirection = direction;
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);

           
            animator.SetFloat("LastInputX", lastDirection.x);
            animator.SetFloat("LastInputY", lastDirection.y);
        }
    }

    

    public void StartChase(Transform playerTransform)
    {
        player = playerTransform;
        isChasing = true;
    }

    public void StopChase()
    {
        isChasing = false;
        rb.velocity = Vector2.zero;
    }

}
