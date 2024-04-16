using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;
using System;
using System.IO;

namespace UnityEngine.Timeline
{
    public class AndroidVideoPlayableBehaviour : PlayableBehaviour
    {
        public string localVideoClipURL;
        public string androidVideoClipURL;
        public VideoPlayer videoPlayer;
        public OVROverlay overlay;
        //public OscTimelineSynchronizer synchronizer;
        //public VideoClip videoClip;
        public bool mute = false;
        public bool loop = true;
        public double preloadTime = 0.3;
        public double clipInTime = 0.0;

        private bool playedOnce = false;
        private bool preparing = false;


        [Tooltip("video synchronization tolerance in seconds")] public float syncTolerance = 0.02f;


        private bool videoPausedBeforeAppPause = false;

        public bool IsPrepared { get; private set; }
        public bool IsPlaying { get; private set; }
        public long Duration { get; private set; }
        public long PlaybackPosition { get; private set; }


        public void OnPlayableDestroy()
        {

            videoPlayer.prepareCompleted -= VideoPreparedCallback;
            videoPlayer.errorReceived -= VideoErrorReceivedCallback;
        }

        public void PrepareVideo(Playable playable)
        {


            if (overlay != null)
            {

                if (!overlay.isExternalSurface)
                {
                    PrepareUnityVideoPlayer(playable);
                }
                else
                {
                    
                    PrepareAndroidVideoPlayer(playable);
                }
            }
            else
            {
                PrepareUnityVideoPlayer(playable);
            }
        }

        public void PrepareUnityVideoPlayer(Playable playable)
        {
            if (videoPlayer == null || videoPlayer.isPrepared || preparing)
                return;

            videoPlayer.targetCameraAlpha = 0.0f;
            //videoPlayer.time = clipInTime;
            videoPlayer.Prepare();
            preparing = true;


            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.url = localVideoClipURL;

            videoPlayer.playOnAwake = false;
            //videoPlayer.waitForFirstFrame = true;
            videoPlayer.isLooping = loop;

            SyncVideoToPlayable(playable);


            //videoPlayer.loopPointReached += LoopPointReached;

        }

        private void VideoErrorReceivedCallback(VideoPlayer source, string message)
        {
            Debug.Log(source.ToString() + " error " + message);
        }
        private void VideoPreparedCallback(VideoPlayer source)
        {
            Debug.Log(source.ToString() + " video is prepared");
        }

        public void PrepareAndroidVideoPlayer(Playable playable)
        {
            if (!IsPrepared)
            {
                Prepare(androidVideoClipURL, null);
                //Seek(clipInTime);
                //SyncVideoToPlayable(playable);//set to start of clip
                //Pause();
            }


        }

        void LoopPointReached(VideoPlayer vp)
        {
            playedOnce = !loop;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (overlay != null)
            {
                if (!overlay.isExternalSurface)
                {
                    PrepareFrameUnity(playable, info);
                }
                else
                {
                    PrepareFrameAndroid(playable, info);
                }
            }
            else
            {
                PrepareFrameUnity(playable, info);
            }


        }

        public void PrepareFrameUnity(Playable playable, FrameData info)
        {

            if (videoPlayer == null || !videoPlayer.isPrepared)
                return;

            // Pause or Play the video to match whether the graph is being scrubbed or playing
            //  If we need to hold the last frame, this will treat the last frame as a pause
            bool shouldBePlaying = info.evaluationType == FrameData.EvaluationType.Playback;

            if (videoPlayer.isPrepared && !videoPlayer.isLooping && playable.GetTime() >= videoPlayer.length)
                shouldBePlaying = false;


            if (shouldBePlaying)
            {

                // this will use the timeline time to prevent drift
                if (!videoPlayer.isPlaying)
                {
                    videoPlayer.timeReference = VideoTimeReference.ExternalTime;
                    videoPlayer.Play();
                    Debug.Log("PrepareFrameUnity - play video");
                    SyncVideoToPlayable(playable);
                }
                if (videoPlayer.timeReference == VideoTimeReference.ExternalTime)
                    videoPlayer.externalReferenceTime = clipInTime + playable.GetTime();
            }
            else
            {
                Debug.Log("Prepare Frame should not be playing - pause and sync");
                if (!videoPlayer.isPaused)
                {
                    videoPlayer.timeReference = VideoTimeReference.Freerun;
                    videoPlayer.Pause();
                }

                SyncVideoToPlayable(playable);
            }

            // use the accumulated blend value to set the alpha and the audio volume
            videoPlayer.targetCameraAlpha = info.effectiveWeight;
            if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
            {
                for (ushort i = 0; i < videoPlayer.audioTrackCount; ++i)
                    videoPlayer.SetDirectAudioVolume(i, info.effectiveWeight);
            }
            /*
            if (videoPlayer == null || localVideoClipURL == null || localVideoClipURL == "")
                return;

            videoPlayer.timeReference = Application.isPlaying ? VideoTimeReference.ExternalTime :
                                                            VideoTimeReference.Freerun;

            if (videoPlayer.isPlaying && Application.isPlaying)
                videoPlayer.externalReferenceTime = playable.GetTime(); //(clipInTime + (playable.GetTime() * videoPlayer.playbackSpeed)) % videoPlayer.length;//            
            else if (!Application.isPlaying)
                SyncVideoToPlayable(playable);
                */
        }

        public void PrepareFrameAndroid(Playable playable, FrameData info)
        {
            NativeVideoPlayer.SetListenerRotation(Camera.main.transform.rotation);
            //IsPlaying = NativeVideoPlayer.IsPlaying;
            PlaybackPosition = NativeVideoPlayer.PlaybackPosition;
            Duration = NativeVideoPlayer.Duration;

            /*
            if (IsPlaying && (int)OVRManager.display.displayFrequency != 60)
            {
                OVRManager.display.displayFrequency = 60.0f;
            }
            else if (!IsPlaying && (int)OVRManager.display.displayFrequency != 72)
            {
                OVRManager.display.displayFrequency = 72.0f;
            }
            */


            // Pause or Play the video to match whether the graph is being scrubbed or playing
            //  If we need to hold the last frame, this will treat the last frame as a pause

            bool shouldBePlaying = info.evaluationType == FrameData.EvaluationType.Playback;

            if (IsPrepared)
            {
                if (playable.GetTime() >= (Duration / 1000) + clipInTime)
                    shouldBePlaying = false;



                if (shouldBePlaying)
                {
                    //if (!NativeVideoPlayer.IsPlaying)
                    if (!IsPlaying)
                    {
                        Play();
                    }
                    SyncViaPlayspeed(playable);

                }
                else
                {
                    /*
                    if (playable.GetTime() >= (Duration / 1000) - clipInTime)
                    {
                        if (IsPlaying)
                            Stop();
                    }
                    else
                    {*/
                    SyncVideoToPlayable(playable);

                    if (IsPlaying)
                        Pause();
                    //}

                }
            }


        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {

            //if (videoPlayer == null)
            //return;
            //if(IsPrepared)
                Play();
            /*
            if (!playedOnce)
            {
                if (!videoPlayer.isPlaying)
                {
                    videoPlayer.Play();
                }
                SyncVideoToPlayable(playable);

                preparing = !videoPlayer.isPrepared;
            }*/

            Debug.Log("Behaviour Play");
            SyncVideoToPlayable(playable);

        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {


            /*
            if (videoPlayer == null)
                return;
                */
            //SyncVideoToPlayable(playable);
            // The effective weight will be greater than 0 if the graph is paused and the playhead is still on this clip.
            if (info.effectiveWeight <= 0)
            {

                Debug.Log("On Behaviour Pause - stop");
                Stop();
            }

            else
            {
                Pause();
                Debug.Log("On Behaviour Pause - pause");
            }




        }

        /*
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{

			if (videoPlayer == null || videoPlayer.url == null)
				return;

            videoPlayer.targetCameraAlpha = info.weight;

		    if (Application.isPlaying)
		    {
		        for (ushort i = 0; i < videoPlayer.audioTrackCount; ++i)
		        {
		            if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
		                videoPlayer.SetDirectAudioVolume(i, info.weight);
		            else if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
		            {
		                AudioSource audioSource = videoPlayer.GetTargetAudioSource(i);
		                if (audioSource != null)
		                    audioSource.volume = info.weight;
		            }
		        }
		    }
		}
        */

        public override void OnGraphStart(Playable playable)
        {

            if (overlay != null)
            {
                overlay.isExternalSurface = NativeVideoPlayer.IsAvailable;
                Debug.Log("external surface:" + overlay.isExternalSurface);
            }

            IsPrepared = false;

            //videoPlayer.timeReference = VideoTimeReference.ExternalTime;
            //Debug.Log("Graph Start");
            playedOnce = false;
            videoPlayer.prepareCompleted += VideoPreparedCallback;
            videoPlayer.errorReceived += VideoErrorReceivedCallback;
            Debug.Log("callbacks registered");
        }

        public override void OnGraphStop(Playable playable)
        {
            //Stop();
            //Debug.Log("Graph Stop");
            /*
		    if (!Application.isPlaying)
		        StopVideo();
                */
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            //Debug.Log("Playable destroyed");
            Stop();

        }

        //Based on Oculus Quest 180 video sample
        public void Prepare(string moviePath, string drmLicencesUrl)
        {
            if (moviePath != string.Empty)
            {
                Debug.Log("Playing Video: " + moviePath);
                if (overlay.isExternalSurface)
                {

                    OVROverlay.ExternalSurfaceObjectCreated surfaceCreatedCallback = () =>
                    {
                        Debug.Log("Playing ExoPlayer with SurfaceObject");
                        NativeVideoPlayer.PlayVideo(moviePath, drmLicencesUrl, overlay.externalSurfaceObject);
                        NativeVideoPlayer.SetLooping(loop);                        
                        IsPrepared = true;                        
                    };

                    if (overlay.externalSurfaceObject == IntPtr.Zero)
                    {
                        Debug.Log("Exo player surfacecreatedcallback set");
                        overlay.externalSurfaceObjectCreated = surfaceCreatedCallback;
                    }
                    else
                    {
                        Debug.Log("Exo player surfacecreatedcallback invoked");
                        surfaceCreatedCallback.Invoke();
                    }
                }

            }

        }

        public void Play()
        {

            if (overlay != null)
            {
                if (overlay.isExternalSurface)
                {
                    
                    //if (!NativeVideoPlayer.IsPlaying)
                    if(IsPrepared)
                    {
                        NativeVideoPlayer.Play();
                        IsPlaying = true;
                    }

                    Debug.Log("Android Video Play Triggered. Is Playing:" + NativeVideoPlayer.IsPlaying);

                    if ((int)OVRManager.display.displayFrequency != 60)
                    {
                        OVRManager.display.displayFrequency = 60.0f;
                    }
                }
                else
                {
                    if (!videoPlayer.isPlaying)
                        videoPlayer.Play();
                }
            }
            else
            {
                if (!videoPlayer.isPlaying)
                    videoPlayer.Play();
            }

        }

        private void Seek(double time)
        {

            long milliseconds = (long) (time * 1000.0);
            if(NativeVideoPlayer.PlaybackPosition != milliseconds)
            {
                NativeVideoPlayer.PlaybackPosition = milliseconds;
            }
        }

        public void Pause()
        {

            if (overlay != null)
            {

                if (overlay.isExternalSurface)
                {
                    NativeVideoPlayer.Pause();
                }
                else
                {
                    videoPlayer.Pause();
                }
            }
            else
            {
                if(videoPlayer != null)
                    videoPlayer.Pause();
            }
            IsPlaying = false;
        }

        public void Stop()
        {
            if(overlay != null)
            {
                if (overlay.isExternalSurface)
                {
                    if(IsPlaying)
                        NativeVideoPlayer.Pause();
                    //IsPrepared = false;

                    if ((int)OVRManager.display.displayFrequency != 72)
                    {
                        OVRManager.display.displayFrequency = 72.0f;
                    }
                }
                else
                {
                    StopVideo();
                }
            }
            else
            {
                StopVideo();
            }

            IsPlaying = false;

        }
        /*
        public void PlayVideo()
        {             
            if (videoPlayer == null)
                return;

            videoPlayer.Play();
            preparing = false;


            if (!Application.isPlaying)
                PauseVideo();

            Debug.Log("Play Video called! is playing?:" + videoPlayer.isPlaying);
        }

        public void PauseVideo()
        {
            if (videoPlayer == null)
                return;

            videoPlayer.Pause();
            preparing = false;
        }
        */
        
        public void StopVideo()
        {
            if (videoPlayer == null)
                return;

            playedOnce = false;
            videoPlayer.Stop();
            preparing = false;
        }
    
        private void SyncViaPlayspeed(Playable playable)
        {
            
            double time = NativeVideoPlayer.PlaybackPosition / 1000.0;

            double targetTime = Math.Min(Math.Max(playable.GetTime(), 0) + clipInTime, Duration);

            double timeError =  targetTime - time; // negative result means the player is ahead of the timeline

            float playbackSpeed = 1.0f;
            
            if(timeError > 10 || timeError < -10)
            {

                Seek(targetTime);

                //if (synchronizer)
                //    synchronizer.SendDebugMessage("Hard sync via playspeed, sync error: " + timeError);
                Debug.Log("Hard sync via playspeed, sync error: " + timeError);
            }
            else
            {

                //test to vary playback speed continuously over 10 seconds
                //NativeVideoPlayer.SetPlaybackSpeed((Mathf.Sin(Time.time*2/Mathf.PI)*.25f)+1);
                
                if ((timeError > syncTolerance) || (timeError < -syncTolerance))
                {
                    playbackSpeed = Mathf.Clamp(1 + ((float)timeError), 0.5f, 1.5f);
                    //if (synchronizer)
                    //    synchronizer.SendDebugMessage("Soft sync: " + playbackSpeed + "Time Error: " + timeError);
                    //Debug.Log("Soft sync: " + playbackSpeed + "Time Error: " + timeError);
                }
                NativeVideoPlayer.SetPlaybackSpeed(playbackSpeed);
                
            }




        }

        private void SyncVideoToPlayable(Playable playable)
        {

            if (overlay != null)
            {
                if (overlay.isExternalSurface)
                {
                    double targetTime = System.Math.Min(System.Math.Max(playable.GetTime(), 0) + clipInTime, Duration);
                    Seek(targetTime);

                   // if (synchronizer)
                     //   synchronizer.SendDebugMessage("Hard sync video to playable:" + targetTime);
                }
                else
                {
                    SyncVideoToPlayableAndroid(playable);
                }
            }
            else
            {
                SyncVideoToPlayableAndroid(playable);
            }
        }


        private void SyncVideoToPlayableAndroid(Playable playable)
        {
            if (videoPlayer == null)
                return;

            //videoPlayer.time = (clipInTime + (playable.GetTime() * videoPlayer.playbackSpeed)) % videoPlayer.length;

            double t = Math.Max(playable.GetTime(), 0);
            double l = double.PositiveInfinity;

            if (videoPlayer.isPrepared)
                l = videoPlayer.length;


            if (videoPlayer.isLooping)
                t = (t + clipInTime) % l;
            else
                t = Math.Min(t + clipInTime, l);

            //videoPlayer.timeReference = VideoTimeReference.InternalTime;

            videoPlayer.externalReferenceTime = playable.GetTime() + clipInTime;
            videoPlayer.time = t;
            Debug.Log("Video Sync'd to:" + t + "; time:" + videoPlayer.time + "; external reference time:" + videoPlayer.externalReferenceTime + "; isPrepared" + videoPlayer.isPrepared + "; isPlaying" + videoPlayer.isPlaying);
        }

        void OnApplicationPause(bool appWasPaused)
        {
            Debug.Log("OnApplicationPause: " + appWasPaused);
            if (appWasPaused)
            {
                videoPausedBeforeAppPause = !IsPlaying;
            }

            // Pause/unpause the video only if it had been playing prior to app pause
            if (!videoPausedBeforeAppPause)
            {
                if (appWasPaused)
                {
                    Pause();
                }
                else
                {
                    Play();
                }
            }
        }
    }
}
