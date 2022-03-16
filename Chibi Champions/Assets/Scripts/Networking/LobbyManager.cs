using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] GameObject clientStatusPrefab;
    [SerializeField] GameObject textContent;
    [SerializeField] GameObject userListContent;
    [SerializeField] GameObject requestPanel;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject userListPanel;
    [SerializeField] GameObject namePanel;
    [SerializeField] GameObject messagePanel;
    bool isServer;
    bool isClient;

    LobbyPanels activePanel = LobbyPanels.Main;

    public static LobbyManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        switch (activePanel)
        {
            case LobbyPanels.Main:

                requestPanel.SetActive(false);
                mainPanel.SetActive(true);
                userListPanel.SetActive(false);
                namePanel.SetActive(false);
                messagePanel.SetActive(false);

                break;

            case LobbyPanels.Name:

                requestPanel.SetActive(false);
                mainPanel.SetActive(false);
                userListPanel.SetActive(false);
                namePanel.SetActive(true);
                messagePanel.SetActive(false);

                break;

            case LobbyPanels.Request:

                requestPanel.SetActive(true);
                mainPanel.SetActive(false);
                userListPanel.SetActive(false);
                namePanel.SetActive(false);
                messagePanel.SetActive(false);

                break;

            case LobbyPanels.UserList:

                requestPanel.SetActive(false);
                mainPanel.SetActive(false);
                userListPanel.SetActive(true);
                namePanel.SetActive(false);
                messagePanel.SetActive(false);

                break;

            case LobbyPanels.Message:

                requestPanel.SetActive(false);
                mainPanel.SetActive(false);
                userListPanel.SetActive(false);
                namePanel.SetActive(false);
                messagePanel.SetActive(true);

                break;
        }
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

        Instantiate(message, textContent.transform);

        RectTransform contentRect = textContent.GetComponent<RectTransform>();

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentRect.sizeDelta.y + 50);

        if (FindObjectOfType<Scrollbar>() != null)
        {
            FindObjectOfType<Scrollbar>().value = 0;
        }
    }

    public void SetUserList(List<string> listOfNames)
    {
        Image[] listToClear = userListContent.GetComponentsInChildren<Image>();

        foreach(Image user in listToClear)
        {
            Destroy(user.gameObject);
        }

        foreach(string name in listOfNames)
        {
            clientStatusPrefab.GetComponentInChildren<TextMeshProUGUI>().text = $"{name}";

            Instantiate(clientStatusPrefab, userListContent.transform);
        }

        RectTransform contentRect = userListContent.GetComponent<RectTransform>();

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 50 * listOfNames.Count);

        if (FindObjectOfType<Scrollbar>() != null)
        {
            FindObjectOfType<Scrollbar>().value = 0;
        }
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

    public void SendRequestMessage()
    {
        GetComponent<Client>().ActivateSendMessageRequest();
    }

    public void SendRequestName(string nameOfRequested)
    {
        GetComponent<Client>().SetNameToRequest(nameOfRequested);
    }

    public void SetMainPanelActive()
    {
        activePanel = LobbyPanels.Main;
    }

    public void SetNamePanelActive()
    {
        activePanel = LobbyPanels.Name;
    }

    public void SetMessagePanelActive()
    {
        activePanel = LobbyPanels.Message;
    }

    public void SetUserListPanelActive()
    {
        activePanel = LobbyPanels.UserList;
    }

    public void SetRequestPanelActive()
    {
        activePanel = LobbyPanels.Request;
    }

    public LobbyPanels GetActivePanel()
    {
        return activePanel;
    }

    public void ActivateRequestPanel()
    {
        if (!requestPanel.activeInHierarchy)
        {
            print("Received Request!");

            requestPanel.SetActive(true);
        }
    }

    public void AcceptMessageRequest()
    {
        FindObjectOfType<Client>().AcceptMessageRequest();
    }

    public void DeclineMessageRequest()
    {
        SetUserListPanelActive();

        FindObjectOfType<Client>().DeclineMessageRequest();
    }
}
