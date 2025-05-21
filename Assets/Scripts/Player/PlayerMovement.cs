using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;
    [SerializeField] private float wallJumpCooldown;
    private float lastWallJumpTime;

    [Header("Wall To Wall Jump")]
    [SerializeField] private float wallToWallJumpForce;
    [SerializeField] private float wallToWallJumpCooldown;
    private float lastWallToWallJumpTime;
    private bool wasOnWallLastFrame;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private bool wasGroundedLastFrame;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        HandleSpriteFlip();
        UpdateAnimations();
        HandleJumpInput();
        HandleCoyoteTime();
        HandleWallToWallJump();
    }

    private void FixedUpdate()
    {
        if (Time.time > lastWallJumpTime + wallJumpCooldown)
        {
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
        }
    }

    private void HandleWallToWallJump()
    {
        bool isOnWall = OnWall();

        // Прыжок от стены к стене, когда игрок отталкивается от стены
        if (!isOnWall && wasOnWallLastFrame && !IsGrounded() &&
            Time.time > lastWallToWallJumpTime + wallToWallJumpCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PerformWallToWallJump();
            }
        }

        wasOnWallLastFrame = isOnWall;
    }

    private void PerformWallToWallJump()
    {
        body.linearVelocity = new Vector2(0, body.linearVelocity.y); // Сохраняем вертикальную скорость
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallToWallJumpForce, wallToWallJumpForce),
                     ForceMode2D.Impulse);

        lastWallToWallJumpTime = Time.time;
        SoundManager.instance?.PlaySound(jumpSound);
        anim.SetTrigger("jump");
    }

    private void HandleSpriteFlip()
    {
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void UpdateAnimations()
    {
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", IsGrounded());
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocity.y > 0)
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * 0.5f);
    }

    private void HandleCoyoteTime()
    {
        bool currentlyGrounded = IsGrounded();

        if (currentlyGrounded && !wasGroundedLastFrame)
        {
            coyoteCounter = coyoteTime;
            jumpCounter = extraJumps;
        }
        else if (!currentlyGrounded)
        {
            coyoteCounter -= Time.deltaTime;
        }

        wasGroundedLastFrame = currentlyGrounded;
    }

    private void Jump()
    {
        if (OnWall() && Time.time > lastWallJumpTime + wallJumpCooldown)
        {
            WallJump();
            return;
        }

        if (IsGrounded() || coyoteCounter > 0)
        {
            PerformJump();
            coyoteCounter = 0;
        }
        else if (jumpCounter > 0)
        {
            PerformJump();
            jumpCounter--;
        }
    }

    private void PerformJump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
        SoundManager.instance?.PlaySound(jumpSound);
        anim.SetTrigger("jump");
    }

    private void WallJump()
    {
        body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY);
        lastWallJumpTime = Time.time;
        SoundManager.instance?.PlaySound(jumpSound);
        anim.SetTrigger("jump");
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            groundLayer
        );
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            wallLayer
        );
        return raycastHit.collider != null;
    }

    public bool CanAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }
}