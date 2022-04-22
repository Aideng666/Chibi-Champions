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
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("MenuScenes");
        }

        if ((video.frame) > 0 && (video.isPlaying == false))
        {
            SceneManager.LoadScene("MenuScenes");
        }
    }
}
