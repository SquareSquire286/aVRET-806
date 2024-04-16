using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DebugModule : MonoBehaviour
{
    private float xRot, yRot, xPos;

    // Start is called before the first frame update
    void Start()
    {
        xRot = 0;
        yRot = 0;
        xPos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.forward * 0.01f;

        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward * 0.01f;

        if (Input.GetKey(KeyCode.A))
            yRot -= 0.1f;

        if (Input.GetKey(KeyCode.D))
            yRot += 0.1f;

        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
        
    }
}
