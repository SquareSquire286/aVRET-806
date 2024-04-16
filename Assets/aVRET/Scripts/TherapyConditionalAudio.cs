using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensures that the preamble on the main therapy console is only spoken once by the virtual therapist (i.e., only the first time that the MainTherapy scene is loaded)


public class TherapyConditionalAudio : MonoBehaviour
{
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (!SceneDataSingleton.mainTherapyLoadedYet)
        {
            audioSource.Play();
            SceneDataSingleton.mainTherapyLoadedYet = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
