using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject[] frames;
    public Animator[] fadeAnims;

    //public Animator fadeTransitionAnim;
    public float fadeTransitionTime = 1.5f;

    //public Animator panelTransitionAnim;
    //public float wipeTransitionTime = 2f;

    // Update is called once per frame
    void Update()
    {    
        if (Input.anyKeyDown && frames[0].activeInHierarchy)
        {
            //frames[0].SetActive(false);
            //frames[1].SetActive(true);
            FindObjectOfType<AudioManager>().Play("Click");
            StartCoroutine(LoadFadeTransition(frames[0], frames[1]));
            //StartCoroutine(LoadTransition());
        }
    }

    IEnumerator LoadFadeTransition(GameObject currentFrame, GameObject targetFrame)
    {
        for (int i = 0; i < fadeAnims.Length; ++i)
        {
            fadeAnims[i].SetTrigger("End");
        }
        //fadeTransitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(fadeTransitionTime);
        currentFrame.SetActive(false);
        targetFrame.SetActive(true);
        //frames[0].SetActive(false);
        //frames[1].SetActive(true);
    }

    //IEnumerator LoadTransition()
    //{
    //    panelTransitionAnim.SetTrigger("Start");
    //    yield return new WaitForSeconds(wipeTransitionTime);
    //    frames[0].SetActive(false);
    //    frames[1].SetActive(true);
    //}
}
