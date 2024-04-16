using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonEvent : AbstractButtonEvent
{
    public override void ExecuteEvent()
    {
        SceneManager.LoadScene("MemoryOrbs");
    }
}
