using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    public sealed class AndroidVideoSchedulerPlayableBehaviour : PlayableBehaviour
    {
		private IEnumerable<TimelineClip> m_Clips;
        private PlayableDirector m_Director;

        internal PlayableDirector director
        {
            get { return m_Director; }
            set { m_Director = value; }
        }

        internal IEnumerable<TimelineClip> clips
        {
            get { return m_Clips; }
            set { m_Clips = value; }
        }
        
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (m_Clips == null)
                return;

            int inputPort = 0;
            foreach (TimelineClip clip in m_Clips)
            {
                ScriptPlayable<AndroidVideoPlayableBehaviour> scriptPlayable =
                    (ScriptPlayable<AndroidVideoPlayableBehaviour>)playable.GetInput(inputPort);

                AndroidVideoPlayableBehaviour androidVideoPlayableBehaviour = scriptPlayable.GetBehaviour();

                if (androidVideoPlayableBehaviour != null)
                {
                    double preloadTime = Math.Max(0.0, androidVideoPlayableBehaviour.preloadTime);
                    if (m_Director.time >= clip.start + clip.duration ||
                        m_Director.time <= clip.start - preloadTime)
                        androidVideoPlayableBehaviour.Stop();
                    else if (m_Director.time > clip.start - preloadTime)
                    {
                        androidVideoPlayableBehaviour.PrepareVideo(playable);
                    }

                }

                ++inputPort;
            }
            /*
            // Searches for clips that are in the 'preload' area and prepares them for playback
            var timelineTime = playable.GetGraph().GetRootPlayable(0).GetTime();
            for (int i = 0; i < playable.GetInputCount(); i++)
            {
                if (playable.GetInput(i).GetPlayableType() != typeof(VideoPlayableBehaviour))
                    continue;

                if (playable.GetInputWeight(i) <= 0.0f)
                {
                    ScriptPlayable<AndroidVideoPlayableBehaviour> scriptPlayable = (ScriptPlayable<AndroidVideoPlayableBehaviour>)playable.GetInput(i);
                    AndroidVideoPlayableBehaviour androidVideoPlayableBehaviour = scriptPlayable.GetBehaviour();
                    double preloadTime = Math.Max(0.0, androidVideoPlayableBehaviour.preloadTime);
                    double clipStart = androidVideoPlayableBehaviour.clipInTime;

                    if (timelineTime > clipStart - preloadTime && timelineTime <= clipStart)
                        androidVideoPlayableBehaviour.PrepareVideo(playable);
                }
            }
            */  
        }
        /*
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {

            if (m_Clips == null)
                return;

            int inputPort = 0;
            foreach (TimelineClip clip in m_Clips)
            {
                ScriptPlayable<AndroidVideoPlayableBehaviour> scriptPlayable =
					(ScriptPlayable<AndroidVideoPlayableBehaviour>)playable.GetInput(inputPort);

				AndroidVideoPlayableBehaviour androidVideoPlayableBehaviour = scriptPlayable.GetBehaviour();

				if (androidVideoPlayableBehaviour != null)
				{
					double preloadTime = Math.Max(0.0, androidVideoPlayableBehaviour.preloadTime);
					if (m_Director.time >= clip.start + clip.duration ||
						m_Director.time <= clip.start - preloadTime)
						androidVideoPlayableBehaviour.Stop();
					else if (m_Director.time > clip.start - preloadTime)
                    {
                        androidVideoPlayableBehaviour.preparevideo(playable);
                        Debug.Log("prepare video");
                    }
						
				}
					
                ++inputPort;
            }
        }
        */
    }
}
