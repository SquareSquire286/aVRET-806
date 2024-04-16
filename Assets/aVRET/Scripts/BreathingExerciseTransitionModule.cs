using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreathingExerciseTransitionModule : MonoBehaviour
{
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        // When the breathing exercise is loaded, it means that the user is just beginning their therapy session
        SceneDataSingleton.orbSceneLoadedYet = false;
        SceneDataSingleton.mainTherapyLoadedYet = false;

        Invoke("StartTherapy", clip.length);    
    }

    void StartTherapy()
    {
        SceneManager.LoadScene("MemoryOrbs");
    }
}
