using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButtonEvent : AbstractButtonEvent
{
    public override void ExecuteEvent()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
}
