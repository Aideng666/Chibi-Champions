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
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().volume = AudioManager.Instance.GetSFXVolume();

        }

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
            if (AudioManager.Instance.dirtyPar)
            {
                if (AudioManager.Instance.isMute() == true)
                {
                    GetComponent<AudioSource>().mute = true;
                }
                else
                {
                    GetComponent<AudioSource>().mute = false;
                }
                GetComponent<AudioSource>().volume = AudioManager.Instance.GetSFXVolume();
                AudioManager.Instance.dirtyPar = false;

            }
        }
        
    }
}
