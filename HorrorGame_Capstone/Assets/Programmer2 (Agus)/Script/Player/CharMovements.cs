using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharMovements : MonoBehaviour
{
   
    public float Speed;
    public float JumpForce;
    private Animator anim;
    private Rigidbody2D body;
    private bool Grounded;
    private int jumpRemaining;
    public int maxJumpCount;

    [Header("Dashing")]
    [SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = 0.5f;
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
        Vector2 movementInput = playerInput.Player.Movement.ReadValue<Vector2>();
        bool dashInput = playerInput.Player.Dash.triggered;

        body.velocity = new Vector2(movementInput.x * Speed, body.velocity.y);

        if (movementInput.x > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (movementInput.x < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (playerInput.Player.Jump.triggered && Grounded && maxJumpCount > 0)
        {
            Jump();
        }

        if (dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            dashingDir = movementInput;

            if (dashingDir == Vector2.zero)
            {
                dashingDir = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(StopDashing());
        }

        anim.SetBool("Dashing", isDashing);

        if (isDashing)
        {
            body.velocity = dashingDir.normalized * dashingVelocity;
            return;
        }

        if (Grounded)
        {
            canDash = true;
        }

        anim.SetBool("Walk", movementInput != Vector2.zero);
        anim.SetBool("Grounded", Grounded);
    }

    public void Jump()
    {
        if (Grounded && maxJumpCount > 0)
        {
            body.velocity = new Vector2(body.velocity.x, JumpForce);
            anim.SetTrigger("Jump");
            jumpRemaining--;
            Grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
            jumpRemaining = maxJumpCount;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = false;
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
    }
}