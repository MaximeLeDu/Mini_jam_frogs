using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoBehaviour
{

    public Camera mainCam;
    public Camera secondCam;

    public Tutorial tutorial;


    public Color colorDark;
    public Color colorLight;

    public float timer;
    public bool pauseTimer;

    public float timeUntilDeath;

    public Light2D globalLight;
    // Start is called before the first frame update
    void Start()
    {
        mainCam.enabled = true;
        secondCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseTimer)
        {
            timer += Time.deltaTime;
            if (timer >= timeUntilDeath)
            {
                //gameOver
            }
        }
    }

    public void TogglePause()
    {
        pauseTimer = !pauseTimer;
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    public void SwitchCams()
    {
        mainCam.enabled = !mainCam.enabled;
        secondCam.enabled = !secondCam.enabled;

    }

    public void SwitchLighting()
    {
        if(globalLight.intensity > 0.8)
        {
            globalLight.color = colorDark;
            globalLight.intensity = 0.68f;
        }
        else
        {
            globalLight.intensity = 0.9f;
            globalLight.color = colorLight;
            tutorial.gameObject.SetActive(true);
            tutorial.TutorialTime();
        }
    }
}
