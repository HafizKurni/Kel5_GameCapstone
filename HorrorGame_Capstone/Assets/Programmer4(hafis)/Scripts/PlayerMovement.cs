using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float hirizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps =2;
    int jumpRemaining;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(hirizontalMovement * moveSpeed, rb.velocity.y);
        GorundCheck();

        animator.SetFloat("Walk", rb.velocity.x);
    }

    public void Move(InputAction.CallbackContext context)
    {
        hirizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(jumpRemaining > 0)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x,  jumpPower);
                jumpRemaining--;
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpRemaining--;
            }
            
        }

    }

   

    private void GorundCheck()
    {
        if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpRemaining = maxJumps;
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
