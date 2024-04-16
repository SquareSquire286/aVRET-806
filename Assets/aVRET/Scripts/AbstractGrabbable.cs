using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ***********************************************************************
// Purpose: The interface for all objects 
//          that can be grabbed and held by the user.
// 
// Class Variables: 
//                   isGrabbed -> dictates the objects per
//                                frame behaviour
//
//                   handGrabbingMe -> the HandAnchor game object that is 
//                                     currently holding the object
// ***********************************************************************
public abstract class AbstractGrabbable : MonoBehaviour
{
    public bool isGrabbed;
    public GameObject handGrabbingMe;
    public Material initialMaterial;
    public Rigidbody rigidbody;
    public Vector3 previousPosition;
    public Quaternion previousRotation;
    public int updateIterations;

    // ************************************************************
    // Functionality: Start is called before the first frame update
    // 
    // Parameters: none
    // return: none
    // ************************************************************
    public virtual void Start()
    {

    }


    // ****************************************************************************
    // Functionality: Update is called once per frame. If the object's
    //                grab status is true, then execute its while-grabbed behaviour
    //                on a per frame basis
    // 
    // Parameters: none
    // return: none
    // ****************************************************************************
    public virtual void Update()
    {
        if (isGrabbed)
            WhileGrabbed();
    }


    // ****************************************************************************
    // Functionality: If the object's grab status chnages from True to False, 
    //                execute the object's while-not-grabbed behaviour
    // 
    // Parameters: none
    // return: none
    // ****************************************************************************
    public virtual void SetGrabStatus(bool newStatus, GameObject hand)
    {
        isGrabbed = newStatus;
        handGrabbingMe = hand;

        if (GetGrabStatus())
        {
            transform.parent = handGrabbingMe.transform;
            rigidbody.isKinematic = true;
            this.RemoveHighlight();

            previousPosition = transform.position;
            previousRotation = transform.rotation;
        }
    }


    // ****************************************************************************
    // Functionality: Called by Grabber whenever a candidate object is 
    //                found for a HandAnchor to hold - the last condition is that
    //                the objectmust not alreadybe held by the other HandAnchor
    // 
    // Parameters: none
    // return: isGrabbed - boolean
    // ****************************************************************************
    public virtual bool GetGrabStatus()
    {
        return isGrabbed;
    }


    // ****************************************************************************
    // Functionality: Defined in concrete subclasses - called every 
    //                frame that isGrabbed is true
    // 
    // Parameters: none
    // return: none
    // ****************************************************************************
    public virtual void WhileGrabbed()
    {
        this.RemoveHighlight();
    }


    // ****************************************************************************
    // Functionality: Defined in concrete subclasses - called on the 
    //                exact frame that isGrabbed becomes false
    // 
    // Parameters: none
    // return: none
    // ****************************************************************************
    public virtual void WhenReleased()
    {

    }

    public virtual void ApplyHighlight(Material highlightMaterial)
    {
        if (isGrabbed)
            return;

        if (GetComponent<Renderer>() == null)
        {
            if (transform.childCount > 0)
                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = highlightMaterial;

            else transform.parent.gameObject.GetComponent<Renderer>().material = highlightMaterial;
        }

        else GetComponent<Renderer>().material = highlightMaterial;
    }

    public virtual void RemoveHighlight()
    {
        if (GetComponent<Renderer>() == null)
        {
            if (transform.childCount > 0)
                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = initialMaterial;

            else transform.parent.gameObject.GetComponent<Renderer>().material = initialMaterial;
        }

        else GetComponent<Renderer>().material = initialMaterial;
    }
}