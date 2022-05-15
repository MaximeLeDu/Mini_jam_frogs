using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Rigidbody2D rb;

    public List<Vector2> waypoints;
    public float speed = 5f;
    public int rotSpeed;
    private int index = 0;

    public Vector2 currentVelocity;

    private readonly float approxError = 0.5f;

    public Vector2 dir;

    public bool isMoving;
    public bool isSpinning;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isMoving)
        {
            dir = (waypoints[index] - (Vector2)transform.position);
            if (dir.SqrMagnitude() < approxError)
                index = (index + 1) % waypoints.Count;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentVelocity = rb.velocity;
        if (isMoving)
        {
            dir = (waypoints[index] - (Vector2)transform.position);
            rb.velocity = (dir.normalized * speed);
            if (dir.SqrMagnitude() < approxError)
                index = (index + 1) % waypoints.Count;
        }
        else if (isSpinning)
        {
            rb.angularVelocity = rotSpeed;
        }

    }
}
