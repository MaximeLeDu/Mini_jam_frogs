using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{

    public Transform player;
    Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = player.position - transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector3)((Vector2)player.position - offset);

    }
}
