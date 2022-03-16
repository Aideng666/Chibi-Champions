using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTestScript : MonoBehaviour
{
    public GameObject[] frames;

    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && frames[0].activeInHierarchy)
        {
            Debug.Log("PRESSED");
            frames[0].SetActive(false);
            frames[1].SetActive(true);
            anim.SetTrigger("Clicked");
        }
    }
}
