using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] TMP_InputField input;
    bool isServer;
    bool isClient;

    public static LobbyManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void MakeClient()
    {
        isClient = true;
        isServer = false;

        gameObject.AddComponent<Client>();
    }

    public void MakeServer()
    {
        isClient = false;
        isServer = true;

        gameObject.AddComponent<Server>();
    }

    public bool GetIsClient()
    {
        return isClient;
    }

    public bool GetIsServer()
    {
        return isServer;
    }

    public void SetMessage(string msg)
    {
        input.text = msg;
    }

}
