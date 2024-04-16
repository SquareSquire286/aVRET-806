using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	[Serializable]
    [TrackClipType(typeof(AndroidVideoPlayableAsset))]
    [TrackColor(0.008f, 0.698f, 0.655f)]
    public class AndroidVideoPlayableTrack : TrackAsset
	{
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            PlayableDirector playableDirector = go.GetComponent<PlayableDirector>();

            ScriptPlayable<AndroidVideoSchedulerPlayableBehaviour> playable =
                ScriptPlayable<AndroidVideoSchedulerPlayableBehaviour>.Create(graph, inputCount);

            AndroidVideoSchedulerPlayableBehaviour androidVideoSchedulerPlayableBehaviour =
                   playable.GetBehaviour();

            if (androidVideoSchedulerPlayableBehaviour != null)
            {
                androidVideoSchedulerPlayableBehaviour.director = playableDirector;
                androidVideoSchedulerPlayableBehaviour.clips = GetClips();
            }

            return playable;
        }
    }
}

