using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public void TutorialTime()
    {
        Invoke(nameof(EndTutorial), 10);
    }

    public void EndTutorial()
    {
        gameObject.SetActive(false);
    }
}
