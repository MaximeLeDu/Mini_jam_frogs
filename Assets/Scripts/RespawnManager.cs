using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{

    public List<Vector2> respawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 RespawnPosition(Vector2 position)
    {
        Vector2 currentBestRespawn = Vector2.zero;
        foreach(Vector2 respawnPos in respawnPoints)
        {
            if (position.x < respawnPos.x)
                return currentBestRespawn;
            currentBestRespawn = respawnPos;
        }

        return currentBestRespawn;
    }
}
