using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 8f;
    [SerializeField] float accelerateTime = 0.7f;
    [SerializeField] float decelerateTime = 1f;
    [SerializeField] Vector2 inputOffset = new Vector2(0.1f, 0.1f);
    bool canMove = true;

    [Header("Jump")]
    [SerializeField] float jumpingSpeed = 8f;
    [SerializeField] float fallMultiplier = 3f;
    [SerializeField] float lowJumpMultiplier = 3f;
    [SerializeField]bool canJump = true;
    bool isJumpingButtonRelease = true;
    bool isJumping;

    [Header("DoubleJump")]
    [SerializeField] float doubleJumpSpeed = 6f;
    [SerializeField]bool canDoubleJump;
    bool isDoubleJumping;

    [Header("GroundCheck")]
    [SerializeField] Vector2 pointOffSet = new Vector2(0, -0.96f);
    [SerializeField] Vector2 size = new Vector2(0.33f, 0.27f);
    [SerializeField] LayerMask groundLayerMask;
    bool gravityModifier = true;
    bool isOnGround;

    public bool playerIsDead = false;
    Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    Animator animator;

    float velocityX;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Ground Check
        isOnGround = OnGround();

        // Animations set to default
        if(rigidBody.velocity.x == 0 && rigidBody.velocity.y == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Fall", false);
            animator.SetBool("Jump", false);
            animator.SetBool("Run", false);
            animator.SetBool("DoubleJump", false);
        }

        #region Jumping Button Reset
        // check the jump button is release
        if (!isJumpingButtonRelease && Input.GetAxis("Jump") == 0)
        {
            isJumpingButtonRelease = true;
        }

        // check if is on ground then set jumping back to default
        if(isOnGround)
        {
            isJumping = false;
            canDoubleJump = false;
            isDoubleJumping = false;
        }
        #endregion
    }

    void FixedUpdate()
    {
        #region Movement
        if (canMove)
        {
            //Move left and right
            if (Input.GetAxisRaw("Horizontal") > inputOffset.x)
            {
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, walkSpeed * Time.fixedDeltaTime * 60, ref velocityX, accelerateTime), rigidBody.velocity.y);
                spriteRenderer.flipX = false;
                if(rigidBody.velocity.y == 0)
                {
                    animator.SetBool("Run", true);
                    animator.SetBool("Idle", false);
                    animator.SetBool("Fall", false);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < inputOffset.x * -1)
            {
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, walkSpeed * Time.fixedDeltaTime * -60, ref velocityX, accelerateTime), rigidBody.velocity.y);
                spriteRenderer.flipX = true;
                if (rigidBody.velocity.y == 0)
                {
                    animator.SetBool("Run", true);
                    animator.SetBool("Idle", false);
                    animator.SetBool("Fall", false);
                }
            }
            else
            {
                // This function allows the player to slide after running 
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, 0, ref velocityX, decelerateTime), rigidBody.velocity.y);
            }
        }
        #endregion

        #region Jumping and Falling
        if (canJump)
        {
            // Jumping function
            if (Input.GetAxis("Jump") == 1 && !isJumping && !canDoubleJump && !isDoubleJumping)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingSpeed);
                canDoubleJump = true;
                isJumping = true;
                isJumpingButtonRelease = false;
                animator.SetBool("Jump", true);
                animator.SetBool("Run", false);
                animator.SetBool("Idle", false);
            }
            if(isJumping && canDoubleJump && !isDoubleJumping && isJumpingButtonRelease)
            {
                // DoubleJumping function
                if (Input.GetAxis("Jump") == 1)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, doubleJumpSpeed);
                    canDoubleJump = false;
                    isDoubleJumping = true;
                    animator.SetBool("DoubleJump", true);
                    animator.SetBool("Jump", false);
                    animator.SetBool("Fall", false);
                }
            }
        }
        if (gravityModifier)
        {
            //When player is falling
            if (rigidBody.velocity.y < 0)
            {
                //Speed up falling
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
                animator.SetBool("Fall", true);
                animator.SetBool("Jump", false);
                animator.SetBool("DoubleJump", false);
            }
            //When player is jumping and release the jump button
            else if (rigidBody.velocity.y > 0 && Input.GetAxis("Jump") != 1)
            {
                //Slow down jumping
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
        #endregion

    }
        #region GroundCheck
    bool OnGround()
    {
        // Ground Check
        Collider2D collidedWithGround = Physics2D.OverlapBox((Vector2)transform.position + pointOffSet, size, 0, groundLayerMask);
        if (collidedWithGround != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw out the box of the GroundCheck function
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + pointOffSet, size);
    }
    #endregion
}