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
    bool isJumping;

    [Header("DoubleJump")]
    [SerializeField] float doubleJumpSpeed = 6f;
    [SerializeField]bool canDoubleJump;
    bool isDoubleJumping;

    [Header("WallJump")]
    [SerializeField] float wallJumpSpeedY = 15f;
    [SerializeField] float wallJumpSpeedX = 20f;
    [SerializeField] bool canWallJump = false;
    bool isWallJumped;
    bool isJumpingButtonRelease = true;

    [Header("GroundCheck")]
    [SerializeField] Vector2 pointOffSet = new Vector2(0, -0.96f);
    [SerializeField] Vector2 size = new Vector2(0.33f, 0.27f);
    [SerializeField] LayerMask groundLayerMask;
    bool gravityModifier = true;
    bool isOnGround;

    [Header("WallCheck")]
    [SerializeField] Vector2 leftPointOffSet = new Vector2(-0.31f, 0);
    [SerializeField] Vector2 rightPointOffSet = new Vector2(0.33f, 0);
    [SerializeField] Vector2 onWallSize = new Vector2(0.18f, 0.37f);
    bool isOnWall = true;
    bool isOnLeftWall = true;
    bool isOnRightWall = true;

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

    void FixedUpdate()
    {
        //Ground Check
        isOnGround = OnGround();
        isOnLeftWall = OnLeftWall();
        isOnRightWall = OnRightWall();
        isOnWall = isOnLeftWall ^ isOnRightWall;

        if(rigidBody.velocity.x == 0 && rigidBody.velocity.y == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Fall", false);
            animator.SetBool("Jump", false);
            animator.SetBool("Run", false);
            animator.SetBool("DoubleJump", false);
        }
        

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
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, 0, ref velocityX, decelerateTime), rigidBody.velocity.y);
            }
        }
        #endregion

        #region Jumping and Falling
        if (canJump)
        {
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

        #region Jumping Button Reset
        if (!isJumpingButtonRelease && Input.GetAxis("Jump") == 0)
        {
            isJumpingButtonRelease = true;
        }
        #endregion

        #region WallJump
        if (canWallJump)
        {
            if (Input.GetAxis("Jump") == 1 && isOnWall && !isOnGround && !isWallJumped && isJumpingButtonRelease)
            {
                if (isOnLeftWall)
                {
                    rigidBody.velocity = new Vector2(wallJumpSpeedX, wallJumpSpeedY);
                }
                else
                {
                    rigidBody.velocity = new Vector2(wallJumpSpeedX * -1, wallJumpSpeedY);
                }
                isWallJumped = true;
                isJumpingButtonRelease = false;
                canDoubleJump = true;
                isDoubleJumping = false;
            }
        }

        if(isOnGround)
        {
            isJumping = false;
            canDoubleJump = false;
            isDoubleJumping = false;
            isWallJumped = false;
        }
        #endregion
    }
        #region GroundCheck
    bool OnGround()
    {
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

    bool OnLeftWall()
    {
        Collider2D coll = Physics2D.OverlapBox((Vector2)transform.position + leftPointOffSet, onWallSize, 0, groundLayerMask);
        if (coll != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool OnRightWall()
    {
        Collider2D coll = Physics2D.OverlapBox((Vector2)transform.position + rightPointOffSet, onWallSize, 0, groundLayerMask);
        if (coll != null)
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + pointOffSet, size);
        Gizmos.DrawWireCube((Vector2)transform.position + leftPointOffSet, onWallSize);
        Gizmos.DrawWireCube((Vector2)transform.position + rightPointOffSet, onWallSize);
    }

    #endregion
}