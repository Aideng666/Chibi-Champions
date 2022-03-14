using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    private bool menuGo=true;
    // Start is called before the first frame update
    void Start()
    {
        if (!FindObjectOfType<AudioManager>().IsPlaying("Menu") && !FindObjectOfType<AudioManager>().IsPlaying("Menu"))
        {
            FindObjectOfType<AudioManager>().Play("Menu");
            FindObjectOfType<AudioManager>().Loop("Menu");
        }
    }

    // Update is called once per frame
    void Update() {
        if (menuGo)
        {

        }
        Debug.Log(menuGo);
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
        FindObjectOfType<AudioManager>().Stop("Menu");
        menuGo = false;

    }
}
