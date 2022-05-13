using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;

    public float speed = 3;
    public float airSpeed = 5;
    public float jumpForce = 0.1f;
    public float maxVelocity = 10;

    private bool isGrounded = false;
    private bool isOnWallAir = false;

    private bool isOnRightWall = false;
    private bool isHeadingRight = true;

    private Vector2 rightJumpDir;
    private Vector2 leftJumpDir;

    public bool inputSpace = false;

    private float HorizontalMovement;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        rightJumpDir = new Vector2(1, 1.3f).normalized;
        leftJumpDir = new Vector2(-1, 1.3f).normalized;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || Input.GetKeyDown(KeyCode.Space) && isOnWallAir)
            inputSpace = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HorizontalMovement = Input.GetAxis("Horizontal");
        if (HorizontalMovement > float.Epsilon)
        {
            sprite.flipX = true;
            isHeadingRight = true;
            anim.SetFloat("speed", 2);
        }
        else if (HorizontalMovement < -float.Epsilon)
        {
            sprite.flipX = false;
            isHeadingRight = false;
            anim.SetFloat("speed", 2);
        }
        else
            anim.SetFloat("speed", 0);
        if (isGrounded)
        {
            rb.MovePosition((Vector2)transform.position + HorizontalMovement * speed * Time.fixedDeltaTime * Vector2.right);
        }
        else
        {
            rb.AddForce(HorizontalMovement * airSpeed * Vector2.right);
            if (rb.velocity.magnitude > maxVelocity)
                rb.velocity = rb.velocity.normalized * maxVelocity;
        }

        if (inputSpace && (isGrounded))
        {
            inputSpace = false;
            isGrounded = false;
            Jump();
        }
        else if(inputSpace && isOnWallAir)
        {
            inputSpace = false;
            isOnWallAir = false;
            WallJump();
        }
    }

    private void Jump()
    {
        rb.velocity = Vector2.zero;
        sprite.flipX = isHeadingRight;
        if (isHeadingRight)
        {
            rb.AddForce(rightJumpDir * jumpForce, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(leftJumpDir * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void WallJump()
    {
        rb.velocity = Vector2.zero;
        Debug.Log(isOnRightWall);
        sprite.flipX = !isOnRightWall;
        anim.SetTrigger("wallJumping");

        if (isOnRightWall)
        {
            rb.AddForce(3f * jumpForce * leftJumpDir, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(3f * jumpForce * rightJumpDir, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;
        Debug.Log(collision.GetContact(0).normal);
        Vector2 c = collision.GetContact(0).normal;
        if(Mathf.Abs(c.y) > Mathf.Abs(c.x))
        {
            Debug.Log("touching floor");
            isGrounded = true;
            if (isOnWallAir)
            {
                isOnWallAir = false;
                anim.SetTrigger("wallJumping");
            }
        }
        else
        {
            Debug.Log("touching wall");
            if (!isGrounded)
            {
                Debug.Log("Clawing wall");
                isOnRightWall = c.x < 0;
                sprite.flipX = isOnRightWall;
                anim.SetTrigger("wallJumping");
                isOnWallAir = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isOnWallAir)
            isGrounded = false;
        else
            anim.SetTrigger("wallJumping");
        isOnWallAir = false;
    }
}
