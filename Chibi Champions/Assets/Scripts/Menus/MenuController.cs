using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject[] frames;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && frames[0].activeInHierarchy)
        {
            frames[0].SetActive(false);
            frames[1].SetActive(true);
            FindObjectOfType<AudioManager>().Play("Click");
        }

        //if (Input.GetKeyDown(KeyCode.Escape) && !frames[0].activeInHierarchy)
        //{
        //    for (int i = 1; i < frames.Length; ++i)
        //    {
        //        frames[i].SetActive(false);
        //    }

        //    frames[0].SetActive(true);
        //}
    }
}
