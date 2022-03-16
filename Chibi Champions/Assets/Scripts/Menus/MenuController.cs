using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject[] frames;

    public Animator fadeTransitionAnim;
    public float fadeTransitionTime = 1.5f;

    public Animator playFadeTransitionAnim;

    private bool isPlayButtonClicked = false;

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
            StartCoroutine(LoadFadeTransition());
            //StartCoroutine(LoadTransition());
        }

        if (isPlayButtonClicked)
        {
            StartCoroutine(LoadPlayFadeTransition());
            isPlayButtonClicked = false;
        }

    }

    IEnumerator LoadFadeTransition()
    {
        fadeTransitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(fadeTransitionTime);
        frames[0].SetActive(false);
        frames[1].SetActive(true);
    }

    public void PlayButtonFade()
    {
        isPlayButtonClicked = true;        
    }

    IEnumerator LoadPlayFadeTransition()
    {
        playFadeTransitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(fadeTransitionTime);
        frames[1].SetActive(false);
        frames[2].SetActive(true);
    }


    //IEnumerator LoadTransition()
    //{
    //    panelTransitionAnim.SetTrigger("Start");
    //    yield return new WaitForSeconds(wipeTransitionTime);
    //    frames[0].SetActive(false);
    //    frames[1].SetActive(true);
    //}
}
