using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    private bool menuEnd=false;
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update() {

        if (!FindObjectOfType<AudioManager>().IsPlaying("Menu") && !FindObjectOfType<AudioManager>().IsPlaying("Menu") && menuEnd==false)
        {
            FindObjectOfType<AudioManager>().Play("Menu");
        }
    }

    public void HoverSound()
    {
        FindObjectOfType<AudioManager>().Play("Select");
    }

    public void ClickSound()
    {
        FindObjectOfType<AudioManager>().Play("Click");

    }
    public void BackSound()
    {
        FindObjectOfType<AudioManager>().Play("Cancel");

    }
    public void StartSound()
    {
        FindObjectOfType<AudioManager>().Play("Start");
        menuEnd = true;
        FindObjectOfType<AudioManager>().Stop("Menu");

    }
}
