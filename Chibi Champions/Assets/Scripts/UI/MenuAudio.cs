using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<AudioManager>().IsPlaying("Menu") && !FindObjectOfType<AudioManager>().IsPlaying("Menu"))
        {
            FindObjectOfType<AudioManager>().Play("Menu");
        }
    }
}
