using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] GameObject content;
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
        message.GetComponent<TextMeshProUGUI>().text = msg;

        Instantiate(message, content.transform);
    }

    public void SendMessage()
    {
        if (isClient)
        {
            GetComponent<Client>().ActivateSendMessage();
        }

        if (isServer)
        {
            GetComponent<Server>().ActivateSendMessage();
        }
    }
}
