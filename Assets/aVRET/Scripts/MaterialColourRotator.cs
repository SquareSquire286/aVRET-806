using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColourRotator : MonoBehaviour
{
    public Material mat;
    public string propertyName = "_Tint";
    public float baseVal = 83f, amplitude = 8f, period = 540f, alpha = 1f;

    float colourVal;

    // Start is called before the first frame update
    void Start()
    {
        colourVal = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // RGB components of the material's specified property oscillate based on three different sinusoidal functions
        // Values oscillate between 75 and 91 by default, but this can be changed in the Inspector window
        mat.SetVector(propertyName, new Vector4((baseVal + amplitude * Mathf.Cos(colourVal)) / 255f, 
                                                (baseVal - amplitude * Mathf.Cos(colourVal)) / 255f, 
                                                (baseVal + amplitude * Mathf.Sin(colourVal)) / 255f, 
                                                alpha));

        colourVal += 1f / period; // increment the colour ticker every frame by a fraction of the period (# of frames needed for one full oscillation)
    }
}
