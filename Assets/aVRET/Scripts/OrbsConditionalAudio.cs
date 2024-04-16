using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensures that the preamble on how the memory orbs work is only spoken once by the virtual therapist (i.e., only the first time that the MemoryOrbs scene is loaded)

public class OrbsConditionalAudio : MonoBehaviour
{
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (!SceneDataSingleton.orbSceneLoadedYet)
        {
            audioSource.Play();
            SceneDataSingleton.orbSceneLoadedYet = true;
        }
    }
}
