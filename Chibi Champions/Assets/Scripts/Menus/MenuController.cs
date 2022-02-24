using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject[] frames;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && frames[0].activeInHierarchy)
        {
            frames[0].SetActive(false);
            frames[1].SetActive(true);
        }
    }
}
