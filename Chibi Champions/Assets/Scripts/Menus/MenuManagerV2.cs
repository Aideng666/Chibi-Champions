using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class MenuManagerV2 : MonoBehaviour
{
    public CinemachineBrain mainCamera;
    public CinemachineVirtualCamera frame0Cam;
    public CinemachineVirtualCamera frame1Cam;

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
        if (Input.anyKeyDown && frame[0].activeInHierarchy)
        {
            frame[0].SetActive(false);
            frame[1].SetActive(true);
            ES.SetSelectedGameObject(startButton);
            frame0Cam.gameObject.SetActive(false);
            frame1Cam.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !frame[0].activeInHierarchy)
        {
            for (int i = 1; i < frame.Length; ++i)
            {
                frame[i].SetActive(false);
            }

            frame[0].SetActive(true);

            frame1Cam.gameObject.SetActive(false);
            frame0Cam.gameObject.SetActive(true);
        }
    }
}
