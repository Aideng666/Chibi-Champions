using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine.UI;

public class PlayerClient : MonoBehaviour
{

    ////////////////LOBBY STATE//////////////////
    [SerializeField] TMP_InputField messageInput;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] GameObject namePanel;
    [SerializeField] GameObject userListPanel;
    [SerializeField] GameObject characterSelectPanel;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject userListContent;
    [SerializeField] GameObject userPrefab;
    [SerializeField] GameObject messageHistoryContent;
    [SerializeField] GameObject messagePrefab;
    [SerializeField] TextMeshProUGUI nameText;

    List<Tuple<string, string>> messageHistory = new List<Tuple<string, string>>();

    static bool nameChosen;
    static bool isReady;
    //////////////////////////////////////////////

    /////////////CHARACTER SELECT STATE///////////
    [SerializeField] Image[] playerIcons = new Image[3];
    [SerializeField] Sprite[] unconfirmedCharacterImages = new Sprite[3];
    [SerializeField] Sprite[] confirmedCharacterImages = new Sprite[3];

    List<string> takenCharacters = new List<string>();

    string selectedCharacter = "Agumon";
    bool isConfirmed;
    //////////////////////////////////////////////

    static IPAddress ip;
    static IPEndPoint server;
    static Socket client;
    static byte[] buffer;

    static string username;

    static bool clientStarted;

    string[] connectedUsers = new string[3];

    ClientStates currentState = ClientStates.Lobby;

    // Start is called before the first frame update
    void Start()
    {
        //StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        if (clientStarted)
        {
            #region ReceivingSection
            int recv = 0;

            try
            {
                recv = client.Receive(buffer);
            }
            catch (Exception e)
            {

            }

            if (recv > 0)
            {
                string messageReceived = Encoding.ASCII.GetString(buffer, 0, recv);

                switch(currentState)
                {
                    case ClientStates.Lobby:

                        if (messageReceived.Contains("SEND_TO/CHARACTERSELECT.KEY"))
                        {
                            currentState = ClientStates.CharacterSelect;

                            characterSelectPanel.SetActive(true);
                            lobbyPanel.SetActive(false);
                        }
                        else if (messageReceived.Contains("PLAYER_READY_STATUS.KEY"))
                        {
                            string[] messageSplit = messageReceived.Split(':');

                            string playerInd = messageSplit[1];
                            string status = messageSplit[2];

                            switch (playerInd)
                            {
                                case "0":

                                    if (status == "True")
                                    {

                                        print("Changing The First Color");
                                        userListContent.GetComponentsInChildren<Button>()[0].image.color = Color.green;
                                    }
                                    else
                                    {
                                        userListContent.GetComponentsInChildren<Button>()[0].image.color = Color.gray;
                                    }

                                    break;

                                case "1":

                                    if (status == "True")
                                    {
                                        userListContent.GetComponentsInChildren<Button>()[1].image.color = Color.green;
                                    }
                                    else
                                    {
                                        userListContent.GetComponentsInChildren<Button>()[1].image.color = Color.gray;
                                    }

                                    break;

                                case "2":

                                    if (status == "True")
                                    {
                                        userListContent.GetComponentsInChildren<Button>()[2].image.color = Color.green;
                                    }
                                    else
                                    {
                                        userListContent.GetComponentsInChildren<Button>()[2].image.color = Color.gray;
                                    }

                                    break;
                            }
                        }
                        else if (messageReceived.Contains("NEW/USER_CONNECTED.KEY"))
                        {
                            connectedUsers = new string[3];

                            string[] names = messageReceived.Split(':');

                            for (int i = 1; i < names.Length; i++)
                            {
                                connectedUsers[i - 1] = names[i];
                            }

                            SetUserList(connectedUsers);
                        }
                        else if (nameChosen)
                        {
                            string[] messageSplit = messageReceived.Split(':');

                            messageHistory.Add(new Tuple<string, string>(messageSplit[0], messageSplit[1]));

                            UpdateMessageHistory();
                        }

                        break;

                    case ClientStates.CharacterSelect:

                        if (messageReceived.Contains("NEW_CHARACTER/SELECTED.KEY"))
                        {
                            string[] messageSplit = messageReceived.Split(':');

                            UpdateSelectedCharacters(messageSplit[1], messageSplit[2]);
                        }
                        else if (messageReceived.Contains("CONFIRMED_CHARACTER/SELECTED.KEY"))
                        {
                            string[] messageSplit = messageReceived.Split(':');

                            UpdateSelectedCharacters(messageSplit[1], messageSplit[2], true);
                        }
                        else
                        {
                            print("Server: " + messageReceived);
                        }

                        break;
                }
            }
            #endregion

            #region SendingSection
            string message;

            switch (currentState)
            {
                case ClientStates.Lobby:

                if (!nameChosen)
                {
                    message = nameInput.text;
                }
                else
                {
                    message = messageInput.text;
                }

                //Send data to client
                byte[] msg = Encoding.ASCII.GetBytes(message);

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    client.Send(msg);

                    if (!nameChosen)
                    {
                        nameChosen = true;

                        namePanel.SetActive(false);
                        userListPanel.SetActive(true);

                        print($"Name: {message}");

                        nameText.text = message;
                        username = message;
                    }
                    else
                    {
                        messageHistory.Add(new Tuple<string, string>(username, message));

                        UpdateMessageHistory();
                    }

                }

                break;

                case ClientStates.CharacterSelect:



                    break;
            }

            #endregion
        }
    }

    public static void StartClient()
    {
        buffer = new byte[512];

        try
        {
            //REPLACE THE IP BELOW WITH YOUR AWS SERVER IP
            //IPAddress ip = IPAddress.Parse("54.208.168.94");
            ip = IPAddress.Parse("127.0.0.1");
            server = new IPEndPoint(ip, 11112);

            client = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                print("Connecting to Server...");
                client.Connect(server);

                print("Connected to IP: " + client.RemoteEndPoint.ToString());

                client.Blocking = false;

            }
            catch (ArgumentNullException anexc)
            {
                print("ArgumentNullException: " + anexc.ToString());
            }
            catch (SocketException sexc)
            {
                print("SocketException: " + sexc.ToString());
            }
            catch (Exception exc)
            {
                print("Unexpected exception: " + exc.ToString());
            }
        }
        catch (Exception e)
        {
            print("Exception: " + e.ToString());
        }

        clientStarted = true;
    }

    public void SetUserList(string[] users)
    {
        TextMeshProUGUI[] oldUsers = userListContent.GetComponentsInChildren<TextMeshProUGUI>();

        if (oldUsers.Length == 3)
        {
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i] == null)
                {
                    oldUsers[i].GetComponentInParent<Image>().color = Color.gray;
                    oldUsers[i].text = "";
                }
                else
                {
                    switch (i)
                    {
                        case 0:

                            oldUsers[i].GetComponentInParent<Image>().color = Color.cyan;
                            break;

                        case 1:

                            oldUsers[i].GetComponentInParent<Image>().color = Color.red;
                            break;

                        case 2:

                            oldUsers[i].GetComponentInParent<Image>().color = Color.yellow;
                            break;
                    }


                    oldUsers[i].text = users[i];
                }

                for (int j = 0; j < oldUsers.Length; j++)
                {
                    print("Text From Old Users " + oldUsers[i].text);

                    if (username == oldUsers[i].text)
                    {
                        if (isReady)
                        {
                            oldUsers[i].GetComponentInParent<Image>().gameObject.GetComponentInChildren<Button>().image.color = Color.green;
                        }
                        else
                        {
                            oldUsers[i].GetComponentInParent<Image>().gameObject.GetComponentInChildren<Button>().image.color = Color.gray;
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < oldUsers.Length; i++)
            {
                Destroy(oldUsers[i].gameObject);
            }

            for (int i = 0; i < users.Length; i++)
            {
                if (users[i] == null)
                {
                    userPrefab.GetComponent<Image>().color = Color.gray;
                    userPrefab.GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
                else
                {
                    switch (i)
                    {
                        case 0:

                            userPrefab.GetComponent<Image>().color = Color.cyan;
                            break;

                        case 1:

                            userPrefab.GetComponent<Image>().color = Color.red;
                            break;

                        case 2:

                            userPrefab.GetComponent<Image>().color = Color.yellow;
                            break;
                    }

                    userPrefab.GetComponentInChildren<TextMeshProUGUI>().text = users[i];
                }

                Instantiate(userPrefab, userListContent.transform);
            }
        }
    }

    public void UpdateMessageHistory()
    {
        TextMeshProUGUI[] oldMessages = messageHistoryContent.GetComponentsInChildren<TextMeshProUGUI>();

        print($"Number Of Old Messages: {oldMessages.Length} Number Of Messages In History: {messageHistory.Count}");

        int count = 0;
        foreach(TextMeshProUGUI message in oldMessages)
        {
            message.text = messageHistory[count].Item2;

            count++; 
        }

        if (count < messageHistory.Count)
        {
            for (int i = count; i < messageHistory.Count; i++)
            {
                messagePrefab.GetComponentInChildren<TextMeshProUGUI>().text = messageHistory[count].Item2;

                for (int j = 0; j < connectedUsers.Length; j++)
                {
                    if (messageHistory[i].Item1 == connectedUsers[0])
                    {
                        messagePrefab.GetComponent<Image>().color = Color.cyan;
                    }
                    if (messageHistory[i].Item1 == connectedUsers[1])
                    {
                        messagePrefab.GetComponent<Image>().color = Color.red;
                    }
                    if (messageHistory[i].Item1 == connectedUsers[2])
                    {
                        messagePrefab.GetComponent<Image>().color = Color.yellow;
                    }
                }

                Instantiate(messagePrefab, messageHistoryContent.transform);
            }
        }


        messageHistoryContent.GetComponent<RectTransform>().sizeDelta = new Vector2(messageHistoryContent.GetComponent<RectTransform>().sizeDelta.x, 100 * messageHistory.Count);
    }

    public void UpdateSelectedCharacters(string playerIndex, string character, bool confirmed = false)
    {
        int index = int.Parse(playerIndex);

        if (confirmed)
        {
            takenCharacters.Add(character);
        }

        switch (character)
        {
            case "Drumstick":

                if (confirmed)
                {
                    playerIcons[index].sprite = confirmedCharacterImages[0];
                }
                else
                {
                    playerIcons[index].sprite = unconfirmedCharacterImages[0];
                }

                break;

            case "Rolfe":

                if (confirmed)
                {
                    playerIcons[index].sprite = confirmedCharacterImages[1];
                }
                else
                {
                    playerIcons[index].sprite = unconfirmedCharacterImages[1];
                }

                break;

            case "Potter":

                if (confirmed)
                {
                    playerIcons[index].sprite = confirmedCharacterImages[2];
                }
                else
                {
                    playerIcons[index].sprite = unconfirmedCharacterImages[2];
                }

                break;
        }
    }

    public void SetIsReady()
    {
        isReady = !isReady;

        SetUserList(connectedUsers);

        print(isReady.ToString());

        string readyMessage = $"PLAYER_READY_STATUS.KEY:{isReady.ToString()}";

        byte[] msg = Encoding.ASCII.GetBytes(readyMessage);

        client.Send(msg);
    }

    public void SetSelectedCharacter(string character)
    {
        if (!isConfirmed)
        {
            for (int i = 0; i < takenCharacters.Count; i++)
            {
                if (takenCharacters[i] == character)
                {
                    return;
                }
            }

            selectedCharacter = character;

            print("Selected: " + selectedCharacter);

            string characterMessage = $"NEW_CHARACTER/SELECTED.KEY:{selectedCharacter}";

            byte[] msg = Encoding.ASCII.GetBytes(characterMessage);

            client.Send(msg);
        }
    }

    public void ConfirmSelectedCharacter()
    {
        if (!isConfirmed)
        {
            for (int i = 0; i < takenCharacters.Count; i++)
            {
                if (takenCharacters[i] == selectedCharacter)
                {
                    return;
                }
            }

            takenCharacters.Add(selectedCharacter);

            string confirmedMessage = $"CONFIRMED_CHARACTER/SELECTED.KEY";

            byte[] msg = Encoding.ASCII.GetBytes(confirmedMessage);

            client.Send(msg);

            isConfirmed = true;
        }
    }

    public void ExitClient()
    {
        string message = "exit";

        //Send data to client
        byte[] msg = Encoding.ASCII.GetBytes(message);

        client.Send(msg);

        client.Shutdown(SocketShutdown.Both);
        client.Close();

        userListPanel.SetActive(false);
        namePanel.SetActive(true);

        nameChosen = false;
        username = null;
        clientStarted = false;
    }

    public string GetUsername()
    {
        return username;
    }
}
