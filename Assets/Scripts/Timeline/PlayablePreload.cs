using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class PlayablePreload : MonoBehaviour
{

    public TimelineAsset simpleTimeline;
    public PlayableDirector playableDirector;

    private void Awake()
    {

        //supposedly loading a simple timeline will improve runtime performance
        PlayableDirector pd = new PlayableDirector();
        if(simpleTimeline)
            pd.playableAsset = simpleTimeline;

        pd.RebuildGraph();
        pd.Evaluate();

        //also supposed to improve performance during runtime
        if(playableDirector)
        {
            playableDirector.RebuildGraph();
            playableDirector.Evaluate();
        }
        
    }
}
