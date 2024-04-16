using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class AbstractButton : MonoBehaviour
{
    private Material initialMaterial;

    protected bool isPressed; // Used as a toggle between releasedPosition and pressedPosition

    public Vector3 releasedPosition, pressedPosition; // change the button's 3D location to give it the appearance of having been pressed down or having been released to its normal position

    public AbstractButtonEvent affectedObject; // when button is pressed, execute the particular event to which it is attached

    // Start is called before the first frame update
    public virtual void Start()
    {
        initialMaterial = GetComponent<Renderer>().material;
        isPressed = false;
        transform.position = releasedPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void SetPressStatus(bool newStatus)
    {
        isPressed = newStatus;

        if (isPressed)
            this.OnPress();

        else this.OnRelease();
    }

    public virtual void OnPress()
    {
        transform.position = pressedPosition;
        affectedObject.ExecuteEvent();
    }

    public virtual void OnRelease()
    {
        transform.position = releasedPosition;
    }

    public virtual void ApplyHighlight(Material highlightMaterial)
    {
        GetComponent<Renderer>().material = highlightMaterial;
    }

    public virtual void RemoveHighlight()
    {
        GetComponent<Renderer>().material = initialMaterial;
    }
}
