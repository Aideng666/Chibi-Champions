using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particle.isPlaying)
        {
            Destroy(gameObject);
        }

        if (GetComponent<AudioSource>() != null)
        {
            if (FindObjectOfType<AudioManager>().isMute() == true)
            {
                GetComponent<AudioSource>().mute = true;
            }
            else
            {
                GetComponent<AudioSource>().mute = false;
            }
            GetComponent<AudioSource>().volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        }
    }
}
