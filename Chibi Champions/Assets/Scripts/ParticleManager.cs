using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    ParticleSystem.ShapeModule particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>().shape;
    }

    // Update is called once per frame
    void Update()
    {
        particleSystem.radius = GetComponentInParent<Tower>().GetRange();
    }
}
