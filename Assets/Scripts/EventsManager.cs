using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class EventsManager : MonoBehaviour
{

    public Light2D interactibleLight;
    public Camera roomCam;

    public List<Rigidbody2D> characters;
    public List<Rigidbody2D> frogs;
    public Light2D firepit;
    public List<Light2D> lights;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PutLightOn()
    {
        interactibleLight.enabled = true;
        interactibleLight.transform.position = roomCam.ScreenToWorldPoint(Input.mousePosition);
    }

    public void PutLightOff()
    {
        interactibleLight.enabled = false;
    }
}
