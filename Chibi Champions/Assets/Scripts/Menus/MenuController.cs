using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject[] frames;

    public Animator[] fadeAnims;

    public float fadeTransitionTime = 1.5f;

    private bool isPlayButtonClicked = false;
    private bool isTutorialButtonClicked = false;
    private bool isOptionsButtonClicked = false;
    private bool isSingleplayerButtonClicked = false;
    private bool isMultiplayerButtonClicked = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && frames[0].activeInHierarchy)
        {
            AudioManager.Instance.Play("Click");
            StartCoroutine(LoadFadeTransitions(frames[0], frames[1]));
        }

        if (isPlayButtonClicked)
        {
            StartCoroutine(LoadFadeTransitions(frames[1], frames[2]));
            isPlayButtonClicked = false;
        }
        else if (isTutorialButtonClicked)
        {
            StartCoroutine(LoadFadeTransitions(frames[1], frames[3]));
            isTutorialButtonClicked = false;
        }
        else if (isOptionsButtonClicked)
        {
            StartCoroutine(LoadFadeTransitions(frames[1], frames[4]));
            isOptionsButtonClicked = false;
        }
        else if (isSingleplayerButtonClicked)
        {
            StartCoroutine(LoadFadeTransitions(frames[2], frames[6]));
            isSingleplayerButtonClicked = false;
        }
        else if (isMultiplayerButtonClicked)
        {
            StartCoroutine(LoadFadeTransitions(frames[2], frames[5]));
            isMultiplayerButtonClicked = false;
        }

        if (ChangeScene.hasReturnedToMenu)
        {
            frames[0].SetActive(false);
            frames[1].SetActive(true);
            ChangeScene.hasReturnedToMenu = false;
        }

        if (ChangeScene.hasPausedQuit)
        {
            frames[0].SetActive(false);
            frames[1].SetActive(true);
            ChangeScene.hasPausedQuit = false;
        }
    }

    public void PlayButtonFade()
    {
        isPlayButtonClicked = true;
    }

    public void TutorialButtonFade()
    {
        isTutorialButtonClicked = true;
    }

    public void OptionsButtonFade()
    {
        isOptionsButtonClicked = true;
    }

    public void SingleButtonFade()
    {
        isSingleplayerButtonClicked = true;
    }

    public void MultiplayerButtonFade()
    {
        isMultiplayerButtonClicked = true;
    }

    IEnumerator LoadFadeTransitions(GameObject currentFrame, GameObject targetFrame)
    {
        for (int i = 0; i < fadeAnims.Length; ++i)
        {
            fadeAnims[i].SetTrigger("End");
        }

        yield return new WaitForSeconds(fadeTransitionTime);
        currentFrame.SetActive(false);
        targetFrame.SetActive(true);
    }   
}
