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

    public void SetMessage(string msg, bool sentMessage = false)
    {
        message.GetComponentInChildren<TextMeshProUGUI>().text = msg;

        if (sentMessage)
        {
            message.GetComponent<Image>().color = Color.magenta;
        }
        else
        {
            message.GetComponent<Image>().color = Color.cyan;
        }

        Instantiate(message, content.transform);

        RectTransform contentRect = content.GetComponent<RectTransform>();

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentRect.sizeDelta.y + 50);

        FindObjectOfType<Scrollbar>().value = 0;
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
