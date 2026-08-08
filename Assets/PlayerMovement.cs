using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    public Vector2 LastMoveDir { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = moveInput * moveSpeed;

        if (moveInput != Vector2.zero)
        {
            LastMoveDir = moveInput;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        
        animator.SetFloat("inputX", moveInput.x);
        animator.SetFloat("inputY", moveInput.y);

        if (context.canceled && moveInput == Vector2.zero)
        {
            animator.SetFloat("LastInputX", LastMoveDir.x);
            animator.SetFloat("LastInputY", LastMoveDir.y);
        }
    }
}
