using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineDelayTester : MonoBehaviour
{

    public PlayableDirector master;
    public PlayableDirector test;
    public double error;
    public double playbackSpeed;


    // Update is called once per frame
    void LateUpdate()
    {
        if(master && test)
        {
            error = master.time - test.time;
            playbackSpeed = test.playableGraph.GetRootPlayable(0).GetSpeed();
        }
    }
}
