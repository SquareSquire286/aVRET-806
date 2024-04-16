using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

namespace UnityEngine.Timeline
{
	[Serializable]
    public class AndroidVideoPlayableAsset : PlayableAsset
	{
        public ExposedReference<VideoPlayer> videoPlayer;

        [SerializeField, NotKeyable]
		public string localVideoClipURL;

        [SerializeField, NotKeyable]
        public string androidVideoClipURL;

        public ExposedReference<OVROverlay> overlay;


       // public ExposedReference<OscTimelineSynchronizer> synchronizer;

        [SerializeField, NotKeyable]
        public bool mute = false;

        [SerializeField, NotKeyable]
        public bool loop = true;

        [SerializeField, NotKeyable]
        public double preloadTime = 0.3;

        [SerializeField, NotKeyable]
        public double clipInTime = 0.0;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
            ScriptPlayable<AndroidVideoPlayableBehaviour> playable =
                ScriptPlayable<AndroidVideoPlayableBehaviour>.Create(graph);

            AndroidVideoPlayableBehaviour playableBehaviour = playable.GetBehaviour();

            playableBehaviour.videoPlayer = videoPlayer.Resolve(graph.GetResolver());
            playableBehaviour.localVideoClipURL = localVideoClipURL;
            playableBehaviour.androidVideoClipURL = androidVideoClipURL;
            playableBehaviour.overlay = overlay.Resolve(graph.GetResolver());
            playableBehaviour.mute = mute;
            playableBehaviour.loop = loop;
            playableBehaviour.preloadTime = preloadTime;
            playableBehaviour.clipInTime = clipInTime;
           // playableBehaviour.synchronizer = synchronizer.Resolve(graph.GetResolver());

            return playable;
		}
	}
}
