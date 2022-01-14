using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
 void Update()
    {
        if (!FindObjectOfType<AudioManager>().IsPlaying("Level1"))
        {
            FindObjectOfType<AudioManager>().Play("Level1");
        }
    }
}
