using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Video;

public class MemorySphere : MonoBehaviour
{
    /*
     * Need to constantly check distance between sphere and player head; increase realism along both visual/auditory scales LINEARLY as the player gets closer
     * Also EXPAND the sphere's size as the player gets closer
     * Once the player sticks their head inside, SceneManager.LoadScene("MemoryActivation");
     */

    private Material material;
    public GameObject player;
    public float forgiveness; // modifiable leeway for when the player first starts to approach the memory sphere
                              // higher forgiveness values mean the sphere gets more realistic from farther away

    public string sceneName;

    private float yRot, brightness, contrast, saturation, temperature;
    private VideoPlayer videoPlayer;
    private AudioLowPassFilter lowPassFilter;

    private bool headInside;

    // Start is called before the first frame update
    void Start()
    {
        yRot = 0.08f;

        brightness = -0.2f;
        contrast = 2f;
        saturation = 0f;
        temperature = 1f;

        headInside = false;

        material = GetComponent<Renderer>().material;

        videoPlayer = GetComponent<VideoPlayer>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        // rotate slowly so that the player can see the entire scene
        transform.Rotate(new Vector3(0f, yRot, 0f));

        UnityEngine.Debug.Log((transform.position - player.transform.position).magnitude);

        // Every frame, check the distance between the memory sphere and the player
        brightness = Mathf.Lerp(0f, -0.2f, (transform.position - player.transform.position).magnitude);
        contrast = Mathf.Lerp(1f, 2f, (transform.position - player.transform.position).magnitude);
        saturation = Mathf.Lerp(1f, 0f, (transform.position - player.transform.position).magnitude);
        temperature = Mathf.Lerp(0f, 1f, (transform.position - player.transform.position).magnitude);
        lowPassFilter.cutoffFrequency = Mathf.Lerp(750f, 22000f, (transform.position - player.transform.position).magnitude);
        transform.localScale = new Vector3(Mathf.Lerp(0.75f, 0.33f, (transform.position - player.transform.position).magnitude),
                                           Mathf.Lerp(0.75f, 0.33f, (transform.position - player.transform.position).magnitude),
                                           Mathf.Lerp(-0.75f, -0.33f, (transform.position - player.transform.position).magnitude));

        // At the end of it all, update the material's four exposed parameters based on proximity
        material.SetFloat("_Brightness", brightness);
        material.SetFloat("_Contrast", contrast);
        material.SetFloat("_Saturation", saturation);
        material.SetFloat("_Temperature", temperature);
    }

    // Called automatically by every GameObject when its collider component (assuming setTrigger == true) overlaps with the collider of another GameObject
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == player)
        {
            yRot = 0f;

            headInside = true;
            Invoke("CheckForSceneChange", 5.0f);

            
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject == player)
        {
            yRot = 0f;
            headInside = true;

            // after 5 seconds of being inside the orb, change scene
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == player)
        {
            yRot = 0.05f;
            headInside = false;
        }
    }

    void CheckForSceneChange()
    {
        if (headInside)
        {
            SceneDataSingleton.SetClip(videoPlayer.clip);
            SceneManager.LoadScene(sceneName);
        }
    }
}

