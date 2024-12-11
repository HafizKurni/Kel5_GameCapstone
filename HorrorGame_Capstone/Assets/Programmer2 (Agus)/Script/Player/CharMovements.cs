using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharMovements : MonoBehaviour
{
    [Header("Movement")]
    public float Speed;

    [Header("Dashing")]
    [SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = 0.5f;

    [Header("Jump")]
    public float JumpForce;
    public int maxJumpCount;
    private bool Grounded;
    private int jumpRemaining;

    private Animator anim;
    private Rigidbody2D body;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpRemaining = maxJumpCount;
        playerInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleDash();
        UpdateAnimation();
    }

    private void HandleMovement()
    {
        Vector2 movementInput = playerInput.Player.Movement.ReadValue<Vector2>();
        body.velocity = new Vector2(movementInput.x * Speed, body.velocity.y);

        // Flip character based on movement direction
        if (movementInput.x > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (movementInput.x < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleJump()
    {
        if (playerInput.Player.Jump.triggered && Grounded && jumpRemaining > 0)
        {
            Jump();
        }
    }

    private void HandleDash()
    {
        bool dashInput = playerInput.Player.Dash.triggered;

        if (dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            dashingDir = playerInput.Player.Movement.ReadValue<Vector2>();

            // If no movement input, dash in the direction the character is facing
            if (dashingDir == Vector2.zero)
            {
                dashingDir = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(StopDashing());
        }

        if (isDashing)
        {
            body.velocity = dashingDir.normalized * dashingVelocity;
            return;
        }

        if (Grounded)
        {
            canDash = true;
        }
    }

    public void Jump()
    {
        if (jumpRemaining > 0)
        {
            body.velocity = new Vector2(body.velocity.x, JumpForce);
            anim.SetTrigger("Jump");
            jumpRemaining--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
            jumpRemaining = maxJumpCount; // Reset jump count when landing
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = false; // Set Grounded to false when leaving the ground
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
    }

    private void UpdateAnimation()
    {
        anim.SetBool("Dashing", isDashing);
        anim.SetBool("Walk", body.velocity.x != 0);
        anim.SetBool("Grounded", Grounded);
    }
}