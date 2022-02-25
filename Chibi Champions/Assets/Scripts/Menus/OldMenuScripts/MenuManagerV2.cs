using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class MenuManagerV2 : MonoBehaviour
{
    public CinemachineBrain mainCamera;

    public CinemachineVirtualCamera[] frameCams;

    public GameObject[] frame;
    public GameObject startButton;
    public EventSystem ES;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Main Menu Base
        if (Input.anyKeyDown && frame[0].activeInHierarchy)
        {
            frame[0].SetActive(false);
            frame[1].SetActive(true);
            //ES.SetSelectedGameObject(startButton);
            frameCams[0].gameObject.SetActive(false);
            frameCams[1].gameObject.SetActive(true);
        }

        // Pressing ESC goes all the way back to the main menu base (Frame0)
        if (Input.GetKeyDown(KeyCode.Escape) && !frame[0].activeInHierarchy)
        {
            for (int i = 1; i < frame.Length; ++i)
            {
                frame[i].SetActive(false);
            }

            frame[0].SetActive(true);

            for(int i = 1; i < frameCams.Length; ++i)
            {
                frameCams[i].gameObject.SetActive(false);
            }

            frameCams[0].gameObject.SetActive(true);
        }
    }

    // FRAME 1 STUFF

    public void Play()
    {
        frame[1].SetActive(false);
        frame[2].SetActive(true);
        frameCams[1].gameObject.SetActive(false);
        frameCams[2].gameObject.SetActive(true);
    }

    public void Options()
    {
        frame[1].SetActive(false);
        frame[3].SetActive(true);
        frameCams[1].gameObject.SetActive(false);
        frameCams[3].gameObject.SetActive(true);
    }

    // Calls the quit function
    public void Quit()
    {
        Application.Quit();
    }

    // Back Button Functions
    public void BackButtonCurrentFrame(GameObject frame)
    {
        frame.SetActive(false);
    }

    public void BackButtonTargetFrame(GameObject frame)
    {
        frame.SetActive(true);
    }

    public void BackButtonCurrentCam(CinemachineVirtualCamera cam)
    {
        cam.gameObject.SetActive(false);
    }

    public void BackButtonTargetCam(CinemachineVirtualCamera cam)
    {
        cam.gameObject.SetActive(true);
    }
}
