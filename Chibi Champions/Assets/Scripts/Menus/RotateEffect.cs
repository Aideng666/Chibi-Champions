using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    // Rotation Variables
    public float xSpeed = 0f;
    public float ySpeed = 0f;
    public float zSpeed = 0f;

    public float xMultipliter = 0f;
    public float yMultipliter = 0f;
    public float zMultipliter = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xSpeed * Time.deltaTime * xMultipliter, ySpeed * Time.deltaTime * yMultipliter, zSpeed * Time.deltaTime * zMultipliter);
    }
}
