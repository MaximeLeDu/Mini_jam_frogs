using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{

    private EventsManager eventsManager;
    // Start is called before the first frame update
    void Start()
    {
        eventsManager = GameObject.FindObjectOfType<EventsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        Debug.Log("Over me");
        eventsManager.PutLightOn();
    }

    private void OnMouseExit()
    {
        Debug.Log("Not over me");
        eventsManager.PutLightOff();
    }
}
