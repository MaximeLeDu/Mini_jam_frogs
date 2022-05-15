using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{

    public Transform player;
    public float camSpeed = 10f;

    private Vector3 targetPosition;

    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = player.position - transform.position;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetPosition = player.position - offset;
        transform.Translate(camSpeed * Time.deltaTime * (targetPosition - transform.position));

    }
}
