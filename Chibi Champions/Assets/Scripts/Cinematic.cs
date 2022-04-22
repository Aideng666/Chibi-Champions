using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Cinematic : MonoBehaviour
{
    [SerializeField] VideoPlayer video;

    private void Update()
    {
        // Ways to skip the cinematic
        // Any key toggle
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("MenuScenes");
        }

        // When the video clip ends
        if ((video.frame) > 0 && (video.isPlaying == false))
        {
            SceneManager.LoadScene("MenuScenes");
        }
    }
}
