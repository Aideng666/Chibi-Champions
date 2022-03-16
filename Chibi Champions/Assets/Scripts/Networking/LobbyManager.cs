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
    bool isServer;
    bool isClient;

    LobbyPanels activePanel = LobbyPanels.Main;

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
            clientStatusPrefab.GetComponentInChildren<TextMeshProUGUI>().text = $"{name} is online";

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

    public LobbyPanels GetActivePanel()
    {
        return activePanel;
    }
}
