using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTestScript : MonoBehaviour
{
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("PRESSED");
            anim.SetTrigger("Clicked");
        }
    }
}
