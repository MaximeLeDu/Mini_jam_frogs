using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Camera mainCam;
    public Camera secondCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam.enabled = true;
        secondCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCams()
    {
        mainCam.enabled = !mainCam.enabled;
        secondCam.enabled = !secondCam.enabled;
    }
}
