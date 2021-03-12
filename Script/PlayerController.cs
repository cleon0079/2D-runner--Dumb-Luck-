﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float accelerateTime;
    [SerializeField] float decelerateTime;
    [SerializeField] Vector2 inputOffset;
    bool canMove = true;

    [Header("Jump")]
    [SerializeField] float jumpingSpeed;
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;
    bool canJump = true;
    bool isJumping;

    [Header("DoubleJump")]
    [SerializeField] float doubleJumpSpeed;
    bool canDoubleJump;
    bool isDoubleJumping;

    [Header("WallJump")]
    [SerializeField] float wallJumpSpeedY;
    [SerializeField] float wallJumpSpeedX;
    [SerializeField] bool canWallJump;
    bool isWallJumped;
    bool isJumpingButtonRelease = true;

    [Header("GroundCheck")]
    [SerializeField] Vector2 pointOffSet;
    [SerializeField] Vector2 size;
    [SerializeField] LayerMask groundLayerMask;
    bool gravityModifier = true;
    bool isOnGround;

    [Header("WallCheck")]
    [SerializeField] Vector2 leftPointOffSet;
    [SerializeField] Vector2 rightPointOffSet;
    [SerializeField] Vector2 onWallSize;
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

        #region Movement
        if (canMove)
        {
            //Move left and right
            if (Input.GetAxisRaw("Horizontal") > inputOffset.x)
            {
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, walkSpeed * Time.fixedDeltaTime * 60, ref velocityX, accelerateTime), rigidBody.velocity.y);
                spriteRenderer.flipX = false;
            }
            else if (Input.GetAxisRaw("Horizontal") < inputOffset.x * -1)
            {
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, walkSpeed * Time.fixedDeltaTime * -60, ref velocityX, accelerateTime), rigidBody.velocity.y);
                spriteRenderer.flipX = true;
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
            }
            if(isJumping && canDoubleJump && !isDoubleJumping && isJumpingButtonRelease)
            {
                if (Input.GetAxis("Jump") == 1)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, doubleJumpSpeed);
                    canDoubleJump = false;
                    isDoubleJumping = true;
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
            animator.SetBool("Jump", false);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            transform.parent = null;
        }
    }
    #endregion
}