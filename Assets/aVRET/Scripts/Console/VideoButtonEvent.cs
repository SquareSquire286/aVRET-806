using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoButtonEvent : AbstractButtonEvent
{
    public VideoPlayer videoPlayer;

    // Start is called before the first frame update
    public override void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
    }

    public override void ExecuteEvent()
    {
        if (videoPlayer.isPlaying)
            this.StopEvent();

        else videoPlayer.Play();
    }

    public override void StopEvent()
    {
        videoPlayer.Pause();
    }
}
