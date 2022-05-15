using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
    RespawnManager respawnManager;

    public ParticleSystem groundParticles;
    public ParticleSystem wallParticles;
    public ParticleSystem dashParticles;
    public ParticleSystem deathParticles;

    public float speed = 3;
    public float airSpeed = 5;
    public float jumpForce = 0.1f;
    public float maxVelocityX = 10;
    public float respawnTime = 5;

    public bool isGrounded = false;
    public bool isOnWallAir = false;
    public bool isJumping = false;
    public bool hasDashed = false;

    public bool isOnRightWall = true;
    public bool isHeadingRight = true;

    private Vector2 rightJumpDir;
    private Vector2 leftJumpDir;

    public bool inputSpace = false;
    public bool inputDash = false;

    public float dashDuration = 2;
    public float dashSpeed = 10;
    private float counter;

    private float HorizontalMovement;

    public bool isOnPlatform = false;
    public Rigidbody2D platformRb;

    public Vector2 velocity;

    public bool isDead = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        respawnManager = GetComponent<RespawnManager>();

        rightJumpDir = new Vector2(1, 1.3f).normalized;
        leftJumpDir = new Vector2(-1, 1.3f).normalized;
    }

    private void Update()
    {

        if (isDead)
            return;

        if (inputDash)
        {
            counter += Time.deltaTime;
            if (counter > dashDuration)
            {
                inputDash = false;
                if (isGrounded)
                    hasDashed = false;
            }
        }
        if(!isJumping)
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded || Input.GetKeyDown(KeyCode.Space) && isOnWallAir && !isGrounded)
            {
                anim.SetTrigger("jumping");
                inputSpace = true;
            }
        HorizontalMovement = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.LeftShift) && !hasDashed && !inputDash)
        {
            anim.SetTrigger("Dashing");
            hasDashed = true;
            inputDash = true;
            if (isHeadingRight)
                dashParticles.transform.Rotate(new Vector3(0, 0, 180 - dashParticles.transform.rotation.eulerAngles.z));
            else
                dashParticles.transform.Rotate(new Vector3(0, 0, - dashParticles.transform.rotation.eulerAngles.z));
            dashParticles.Play();
            counter = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
            return;


        if (inputDash)
        {
            rb.velocity = Vector2.zero;
            if (isHeadingRight)
                rb.MovePosition((Vector2)transform.position + Vector2.right * dashSpeed * Time.fixedDeltaTime);
            else
                rb.MovePosition((Vector2)transform.position + Vector2.left * dashSpeed * Time.fixedDeltaTime);
            return;
        }
        HorizontalMovement = Input.GetAxis("Horizontal");
        if (HorizontalMovement > float.Epsilon)
        {
            if(!isOnWallAir)
                sprite.flipX = true;
            isHeadingRight = true;
            anim.SetFloat("speed", 2);
        }
        else if (HorizontalMovement < -float.Epsilon)
        {
            if(!isOnWallAir)
                sprite.flipX = false;
            isHeadingRight = false;
            anim.SetFloat("speed", 2);
        }
        else
            anim.SetFloat("speed", 0);
        if (isGrounded)
        {
            //rb.MovePosition((Vector2)transform.position + HorizontalMovement * speed * Time.fixedDeltaTime * Vector2.right);
            rb.velocity = HorizontalMovement * speed * Time.fixedDeltaTime * Vector2.right;
            if (isOnPlatform)
                rb.velocity += platformRb.velocity;
        }
        else
        {
            rb.AddForce(HorizontalMovement * airSpeed * Vector2.right);
            if (rb.velocity.x > maxVelocityX)
                rb.velocity = new Vector2(maxVelocityX, rb.velocity.y);
            else if (rb.velocity.x < -maxVelocityX)
                rb.velocity = new Vector2(-maxVelocityX, rb.velocity.y);
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

        velocity = rb.velocity;
    }

    private void Jump()
    {
        groundParticles.Play();
        isJumping = true;
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
        isJumping = true;
        rb.velocity = Vector2.zero;
        Debug.Log(isOnRightWall);
        sprite.flipX = !isOnRightWall;
        anim.SetTrigger("wallJumpingOff");

        if (isOnRightWall)
        {
            rb.AddForce(1.2f * jumpForce * leftJumpDir, ForceMode2D.Impulse);
            wallParticles.transform.localPosition = new Vector3(2.23f, 0, 0);
            }
        else
        {
            rb.AddForce(1.2f * jumpForce * rightJumpDir, ForceMode2D.Impulse);
            wallParticles.transform.localPosition = Vector3.zero;
        }

        wallParticles.Play();
        isHeadingRight = !isOnRightWall;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("harmful") && !isDead)
        {
            rb.isKinematic = true;
            isDead = true;
            deathParticles.Play();
            anim.SetTrigger("death");
            rb.velocity = Vector2.zero;
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    private void Respawn()
    {
        isDead = false;
        rb.isKinematic = false;
        rb.MovePosition(respawnManager.RespawnPosition(rb.position));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("moving"))
        {
            isOnPlatform = true;
            platformRb = collision.gameObject.GetComponent<Rigidbody2D>();
            //transform.parent = collision.transform;
            //rb.isKinematic = true;
            Debug.Log("on");
        }

        isJumping = false;
        hasDashed = false;
        rb.velocity = Vector2.zero;
        Debug.Log(collision.GetContact(0).normal);
        Vector2 c = collision.GetContact(0).normal;
        if(Mathf.Abs(c.y) > Mathf.Abs(c.x))
        {
            if (c.y > 0)
            {
                isGrounded = true;
                if (isOnWallAir)
                {
                    anim.SetTrigger("wallJumpingOff");
                }
            }
        }
        else
        {
            isOnWallAir = true;
            if (!isGrounded)
            {
                isOnRightWall = c.x < 0;
                sprite.flipX = isOnRightWall;
                anim.ResetTrigger("wallJumpingOff");
                anim.SetTrigger("wallJumping");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contactCount > 0)
        {
            Vector2 c = collision.GetContact(0).normal;
            if (Mathf.Abs(c.y) > Mathf.Abs(c.x))
            {
                if (c.y > 0)
                {
                    if(!isJumping)
                        isGrounded = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("moving"))
        {
            isOnPlatform = false;
            //transform.parent = null;
            //rb.isKinematic = false;
            Debug.Log("off");
        }

        //Slide down a wall that doesn't end on the floor or jump from a wall
        if (isOnWallAir && !isGrounded && !isJumping)
        {
            anim.SetTrigger("wallJumpingOff");
            isOnWallAir = false;
        }

        //Jump from the corner
        if(isOnWallAir && !isGrounded && isJumping)
        {
            anim.SetTrigger("wallJumpingOff");
            isJumping = false;
        }

        //The character moves out from a corner
        if(isOnWallAir && isGrounded)
        {
            isOnWallAir = false;
        }
        else
        {
            isGrounded = false;
        }
    }
}
