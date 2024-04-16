using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DimensionChangeButtonEvent : AbstractButtonEvent
{
    public GameObject threeDPlayer, twoDPlayer;
    public Text buttonTitle;

    public override void ExecuteEvent()
    {
        if (twoDPlayer.activeSelf)
        {
            twoDPlayer.SetActive(false);
            threeDPlayer.GetComponent<Renderer>().enabled = true;

            buttonTitle.text = "Project to 2D";
        }

        else
        {
            twoDPlayer.SetActive(true);
            threeDPlayer.GetComponent<Renderer>().enabled = false;

            buttonTitle.text = "Project to 3D";
        }
    }
}
