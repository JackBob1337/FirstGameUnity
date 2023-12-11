using System.ComponentModel;
using System.Runtime;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;

    
    [Header("Move Info")]
    private float dirX = 0f;
    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpForce = 14f;
    private int airJumpCount;
    private int airJumpCountMax;
    
    [Header("Collision Info")]
    [SerializeField] private LayerMask jumpableGround;
   
    

    private enum Movementstate {idle, running, jumping, falling, doubleJump}
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        airJumpCountMax = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        //move
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        
        //jump
        if (IsGrounded())
        {
            airJumpCount = 0;
        }
        //Noramal Jump
        if (Input.GetButton("Jump"))
        {
            if (IsGrounded())
            {
                Jump();
            } 
            else 
            {
                //Double Jump
                if (Input.GetButtonDown("Jump"))
                {
                    if (airJumpCount < airJumpCountMax)
                    {
                        Jump();
                        airJumpCount++;
                    }
                }                    
            }
        }
        UpdateAnimationUpdate();
        
        
    }

    private void UpdateAnimationUpdate() 
    {
        Movementstate state;
        
        //animation
        if (dirX > 0f)
        {
            state = Movementstate.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = Movementstate.running;
            sprite.flipX = true;
        }
        else
        {
            state = Movementstate.idle; 
        }

        
        if (rb.velocity.y > .1f)
        {
            if (airJumpCount == 0 && rb.velocity.y != 0)
            {
                state = Movementstate.jumping;
            }
            else if (airJumpCount > 0)
            {
                state = Movementstate.doubleJump;
            }
        }
        else if (rb.velocity.y < -.01f)
        {
            state = Movementstate.falling;
        }
        
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

  
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
