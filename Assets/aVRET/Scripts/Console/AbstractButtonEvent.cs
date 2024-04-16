using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractButtonEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void ExecuteEvent()
    {
        // defined in concrete subclasses
    }

    public virtual void StopEvent()
    {
        // defined in concrete subclasses
    }
}
