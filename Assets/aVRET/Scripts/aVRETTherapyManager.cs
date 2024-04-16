using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

public class aVRETTherapyManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;

    public aVRETSlider mainSlider, volumeSlider;
    public GameObject monitor;

    private float brightness, contrast, saturation, temperature, lowPass;

    public Material sphereMaterial;
    private Material monitorMaterial;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (SceneDataSingleton.GetClip() != null)
        {
            videoPlayer.clip = SceneDataSingleton.GetClip();
            monitor.GetComponent<VideoPlayer>().clip = SceneDataSingleton.GetClip();
        }

        videoPlayer.Play();
        monitor.GetComponent<VideoPlayer>().Play();

        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        monitorMaterial = monitor.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // change all the settings based on the mainSlider position

        brightness = -0.2f + (0.3f * (Mathf.Abs(mainSlider.gameObject.transform.position.z - mainSlider.GetMinX()) / mainSlider.GetRange()));
        contrast = 2f - (Mathf.Abs(mainSlider.gameObject.transform.position.z - mainSlider.GetMinX()) / mainSlider.GetRange());
        saturation = (Mathf.Abs(mainSlider.gameObject.transform.position.z - mainSlider.GetMinX())) / mainSlider.GetRange();
        temperature = 1f - (Mathf.Abs(mainSlider.gameObject.transform.position.z - mainSlider.GetMinX()) / mainSlider.GetRange());
        lowPassFilter.cutoffFrequency = 750f + (21250f * (Mathf.Abs(mainSlider.gameObject.transform.position.z - mainSlider.GetMinX()) / mainSlider.GetRange()));

        audioSource.volume = (Mathf.Abs(volumeSlider.gameObject.transform.position.z - volumeSlider.GetMinX())) / volumeSlider.GetRange();

        UnityEngine.Debug.Log(saturation);

        sphereMaterial.SetFloat("_Brightness", brightness);
        sphereMaterial.SetFloat("_Contrast", contrast);
        sphereMaterial.SetFloat("_Saturation", saturation);
        sphereMaterial.SetFloat("_Temperature", temperature);

        monitorMaterial.SetFloat("_Brightness", brightness);
        monitorMaterial.SetFloat("_Contrast", contrast);
        monitorMaterial.SetFloat("_Saturation", saturation);
        monitorMaterial.SetFloat("_Temperature", temperature);
    }
}
