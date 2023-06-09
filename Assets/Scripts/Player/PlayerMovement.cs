using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls controls;

    float direction = 0;
    public float speed = 200;
    public bool ifFacingRight = true;

    public float jumpForce = 6;
    bool isGrounded;
    int numberOfJumps = 0;
    public Transform GroundCheck;
    public LayerMask GroundLayer;

    public Rigidbody2D playerRB;
    public Animator animator;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();

        controls.Land.Move.performed += ctx => 
        {
            direction = ctx.ReadValue<float>();
        };

        controls.Land.Jump.performed += ctx => Jump();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, GroundLayer );
        animator.SetBool("isGrounded", isGrounded);
        
        playerRB.velocity = new Vector2(direction * speed * Time.fixedDeltaTime, playerRB.velocity.y );
        animator.SetFloat("Speed", Mathf.Abs(direction));

        if(ifFacingRight && direction <0 || !ifFacingRight && direction >0 )
        Flip();
    }


    void Flip()
    {
        ifFacingRight = !ifFacingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void Jump()
    {
        if(isGrounded)
        {
            numberOfJumps = 0;
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            numberOfJumps++;
            AudioManager.instance.Play("FirstJump");
        }
        else
        {
            if(numberOfJumps == 1)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                numberOfJumps++;
                AudioManager.instance.Play("SecondJump");
            }
        }
    }
}
