using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aVRETSlider : AbstractGrabbable
{
    [SerializeField] float minX, maxX;

    public override void SetGrabStatus(bool newStatus, GameObject hand)
    {
        isGrabbed = newStatus;
        handGrabbingMe = hand;

        if (isGrabbed)
            this.RemoveHighlight();

        // do not change the slider's transform, since it won't snap to the player's hand. It needs to remain on the console
    }

    public override void WhileGrabbed()
    {
        // use Mathf.Clamp to keep slider on console but respond to player's hand position
        transform.position = new Vector3(transform.position.x, 
                                        transform.position.y, 
                                        Mathf.Clamp(handGrabbingMe.transform.position.z, 
                                        this.GetMaxX(), 
                                        this.GetMinX()));
        this.RemoveHighlight();
    }

    public override void WhenReleased()
    {
        isGrabbed = false;
        handGrabbingMe = null;
    }
    
    public float GetMinX()
    {
        return minX;
    }

    public float GetMaxX() 
    { 
        return maxX; 
    }

    public float GetRange()
    {
        return Mathf.Abs(maxX - minX);
    }
}
