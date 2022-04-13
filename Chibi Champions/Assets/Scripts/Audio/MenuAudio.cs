using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!AudioManager.Instance.IsPlaying("Menu") && !AudioManager.Instance.IsPlaying("Menu"))
        {
            AudioManager.Instance.Play("Menu");
            AudioManager.Instance.Loop("Menu");
        }
    }

    public void HoverSound()
    {
        AudioManager.Instance.Play("Select");
    }

    public void ClickSound()
    {
        AudioManager.Instance.Play("Click");

    }
    public void BackSound()
    {
        AudioManager.Instance.Play("Cancel");

    }
    public void StartSound()
    {
        AudioManager.Instance.Play("Start");
        AudioManager.Instance.Stop("Menu");
    }
}
