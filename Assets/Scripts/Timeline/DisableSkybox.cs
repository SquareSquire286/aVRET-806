using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSkybox : MonoBehaviour
{

    public Camera camera;

    private void Awake()
    {
        TriggerClearFlags();
    }

    public void TriggerClearFlags()
    {
        camera.clearFlags = CameraClearFlags.Nothing;
    }


    
}
