using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;

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

    private bool inputSpace = false;

    private float HorizontalMovement;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
        if (HorizontalMovement > 0)
            isHeadingRight = true;
        else if (HorizontalMovement < 0)
            isHeadingRight = false;
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

        if(inputSpace && isOnWallAir)
        {
            inputSpace = false;
            WallJump();
        }
    }

    private void Jump()
    {
        rb.velocity = Vector2.zero;
        if(isHeadingRight)
            rb.AddForce(rightJumpDir * jumpForce, ForceMode2D.Impulse);
        else
            rb.AddForce(leftJumpDir * jumpForce, ForceMode2D.Impulse);
    }

    private void WallJump()
    {
        rb.velocity = Vector2.zero;
        Debug.Log(isOnRightWall);
        if (isOnRightWall)
        {
            rb.AddForce(leftJumpDir * jumpForce * 1.5f, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(rightJumpDir * jumpForce * 1.5f, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;
        Debug.Log(collision.GetContact(0).normal);
        Vector2 c = collision.GetContact(0).normal;
        if(Mathf.Abs(c.y) > Mathf.Abs(c.x))
        {
            isGrounded = true;
        }
        else
        {
            isOnRightWall = c.x < 0;
            isOnWallAir = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isOnWallAir)
            isGrounded = false;
        isOnWallAir = false;
    }
}
