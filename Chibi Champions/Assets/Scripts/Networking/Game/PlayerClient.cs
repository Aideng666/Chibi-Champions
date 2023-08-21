//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using TMPro;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class PlayerClient : MonoBehaviour
//{
//    ////////////////LOBBY STATE//////////////////
//    [SerializeField] TMP_InputField messageInput;
//    [SerializeField] TMP_InputField nameInput;
//    [SerializeField] GameObject namePanel;
//    [SerializeField] GameObject userListPanel;
//    [SerializeField] GameObject characterSelectPanel;
//    [SerializeField] GameObject lobbyPanel;
//    [SerializeField] GameObject leaderboardPanel;
//    [SerializeField] GameObject userListContent;
//    [SerializeField] GameObject leaderboardContent;
//    [SerializeField] GameObject userPrefab;
//    [SerializeField] GameObject messageHistoryContent;
//    [SerializeField] GameObject messagePrefab;
//    [SerializeField] TextMeshProUGUI nameText;

//    List<Tuple<string, string>> messageHistory = new List<Tuple<string, string>>();

//    static bool nameChosen;
//    static bool isReady;
//    //////////////////////////////////////////////

//    /////////////CHARACTER SELECT STATE///////////
//    [SerializeField] Image[] playerIcons = new Image[3];
//    [SerializeField] Sprite[] unconfirmedCharacterImages = new Sprite[3];
//    [SerializeField] Sprite[] confirmedCharacterImages = new Sprite[3];

//    List<string> takenCharacters = new List<string>();

//    string selectedCharacter = "Agumon";
//    int selectedCharacterIndex = 0;
//    bool isConfirmed;
//    //////////////////////////////////////////////

//    ////////////////////GAMEPLAY//////////////////
//    string[] playersCharacters = new string[3];
//    //////////////////////////////////////////////

//    static IPAddress ip;
//    static IPEndPoint server;
//    static Socket client;
//    static byte[] buffer;

//    static string username;

//    static bool clientStarted;

//    string[] connectedUsers = new string[3];

//    ClientStates currentState = ClientStates.Lobby;

//    static int clientNum = -1;

//    //List<string> currentLeaderboardStats = new List<string>();
//    List<string[]> currentLeaderboardMessageSplits = new List<string[]>();
//    bool showLeaderboard;

//    public static PlayerClient Instance { get; set; }

//    private void Awake()
//    {
//        Instance = this;
//    }
//    // Start is called before the first frame update
//    void Start()
//    {
//        DontDestroyOnLoad(gameObject);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        buffer = new byte[512];

//        if (clientStarted)
//        {
//            #region LOBBY
//            if (currentState != ClientStates.Gameplay)
//            {
//                #region ReceivingSection
//                int recv = 0;

//                try
//                {
//                    recv = client.Receive(buffer);
//                }
//                catch (Exception e)
//                {

//                }

//                if (recv > 0)
//                {
//                    string messageReceived = Encoding.ASCII.GetString(buffer, 0, recv);

//                    switch (currentState)
//                    {
//                        case ClientStates.Lobby:

//                            if (messageReceived.Contains("SEND_TO/CHARACTERSELECT.KEY"))
//                            {
//                                currentState = ClientStates.CharacterSelect;

//                                characterSelectPanel.SetActive(true);
//                                lobbyPanel.SetActive(false);
//                            }
//                            else if (messageReceived.Contains("PLAYER_READY_STATUS.KEY"))
//                            {
//                                string[] messageSplit = messageReceived.Split(':');

//                                string playerInd = messageSplit[1];
//                                string status = messageSplit[2];

//                                switch (playerInd)
//                                {
//                                    case "0":

//                                        if (status == "True")
//                                        {
//                                            userListContent.GetComponentsInChildren<Button>()[0].image.color = Color.green;
//                                        }
//                                        else
//                                        {
//                                            userListContent.GetComponentsInChildren<Button>()[0].image.color = Color.gray;
//                                        }

//                                        break;

//                                    case "1":

//                                        if (status == "True")
//                                        {
//                                            userListContent.GetComponentsInChildren<Button>()[1].image.color = Color.green;
//                                        }
//                                        else
//                                        {
//                                            userListContent.GetComponentsInChildren<Button>()[1].image.color = Color.gray;
//                                        }

//                                        break;

//                                    case "2":

//                                        if (status == "True")
//                                        {
//                                            userListContent.GetComponentsInChildren<Button>()[2].image.color = Color.green;
//                                        }
//                                        else
//                                        {
//                                            userListContent.GetComponentsInChildren<Button>()[2].image.color = Color.gray;
//                                        }

//                                        break;
//                                }
//                            }
//                            else if (messageReceived.Contains("NEW/USER_CONNECTED.KEY"))
//                            {
//                                connectedUsers = new string[3];

//                                string[] names = messageReceived.Split(':');

//                                for (int i = 1; i < names.Length; i++)
//                                {
//                                    connectedUsers[i - 1] = names[i];

//                                    if (names[i] == username)
//                                    {
//                                        clientNum = i - 1;

//                                        print("Set Client Num to " + clientNum);
//                                    }


//                                }

//                                SetUserList(connectedUsers);
//                            }
//                            else if (messageReceived.Contains("LEADERBOARD_MSG_REQUEST.KEY"))
//                            {
//                                string[] messageSplit = messageReceived.Split(':');

//                                for (int i = 1; i < messageSplit.Length; i++)
//                                {
//                                    //currentLeaderboardStats.Add(messageSplit[i]);

//                                    currentLeaderboardMessageSplits.Add(messageSplit[i].Split('|'));
//                                }

//                                SetLeaderboardInfo();
//                            }
//                            else if (nameChosen)
//                            {
//                                string[] messageSplit = messageReceived.Split(':');

//                                messageHistory.Add(new Tuple<string, string>(messageSplit[0], messageSplit[1]));

//                                UpdateMessageHistory();
//                            }

//                            break;

//                        case ClientStates.CharacterSelect:

//                            if (messageReceived.Contains("NEW_CHARACTER/SELECTED.KEY"))
//                            {
//                                string[] messageSplit = messageReceived.Split(':');

//                                UpdateSelectedCharacters(messageSplit[1], messageSplit[2]);
//                            }
//                            else if (messageReceived.Contains("CONFIRMED_CHARACTER/SELECTED.KEY"))
//                            {
//                                string[] messageSplit = messageReceived.Split(':');

//                                UpdateSelectedCharacters(messageSplit[1], messageSplit[2], true);
//                            }

//                            if (messageReceived.Contains("GAME_START_MESSAGE.KEY"))
//                            {
//                                BeginGameState();
//                            }

//                            break;
//                    }
//                }
//                #endregion

//                #region SendingSection
//                string message;

//                switch (currentState)
//                {
//                    case ClientStates.Lobby:

//                        if (!nameChosen)
//                        {
//                            message = nameInput.text;
//                        }
//                        else
//                        {
//                            message = messageInput.text;
//                        }

//                        //Send data to client
//                        byte[] msg = Encoding.ASCII.GetBytes(message);

//                        if (Input.GetKeyDown(KeyCode.Return))
//                        {
//                            client.Send(msg);

//                            if (!nameChosen)
//                            {
//                                nameChosen = true;

//                                namePanel.SetActive(false);
//                                userListPanel.SetActive(true);

//                                nameText.text = message;
//                                username = message;
//                            }
//                            else
//                            {
//                                messageHistory.Add(new Tuple<string, string>(username, message));

//                                UpdateMessageHistory();
//                            }

//                        }

//                        break;
//                }

//                #endregion
//            }
//            #endregion
//            else
//            {
//                int recv = 0;

//                try
//                {
//                    recv = client.Receive(buffer);
//                }
//                catch (Exception e)
//                {

//                }

//                if (recv > 0)
//                {
//                    float[] messageReceived = new float[recv / sizeof(float)];

//                    Buffer.BlockCopy(buffer, 0, messageReceived, 0, recv);
         
//                    switch (messageReceived[0])
//                    {
//                        case 0: //Player Movement Update

//                            EntityManager.Instance.ReceivePlayerMovementUpdates((int)messageReceived[1],
//                            new Vector3(messageReceived[2], messageReceived[3], messageReceived[4]),
//                            new Vector3(messageReceived[5], messageReceived[6], messageReceived[7]));

//                            //UDPClient.Instance.SendUpdateConfirmed();

//                            break;

//                        case 1: // Player Action Update

//                            string character = "";

//                            if (messageReceived[2] == 0)
//                            {
//                                character = "Drumstick";
//                            }
//                            else if (messageReceived[2] == 1)
//                            {
//                                character = "Rolfe";
//                            }
//                            else if (messageReceived[2] == 2)
//                            {
//                                character = "Potter";
//                            }

//                            EntityManager.Instance.ReceivePlayerUpdates(character, (int)messageReceived[3]);

//                            UDPClient.Instance.SendUpdateConfirmed();

//                            break;

//                        case 2: // Tower Update

//                            int[] towers;
//                            List<float> xPositions = new List<float>();
//                            List<float> yPositions = new List<float>();
//                            List<Vector2> towerPositions = new List<Vector2>();

//                            if (messageReceived.Length == 2)
//                            {
//                                towers = new int[0];
//                            }
//                            else
//                            {
//                                towers = new int[(messageReceived.Length - 2) / 3];
//                            }

//                            for (int i = 0; i < towers.Length; i++)
//                            {
//                                towers[i] = (int)messageReceived[i + 2];
//                            }

//                            for (int i = 0; i < towers.Length * 2; i++)
//                            {
//                                if (i % 2 == 0)
//                                {
//                                    xPositions.Add(messageReceived[i + 2 + towers.Length]);
//                                }
//                                else
//                                {
//                                    yPositions.Add(messageReceived[i + 2 + towers.Length]);
//                                }
//                            }

//                            for (int i = 0; i < xPositions.Count; i++)
//                            {
//                                towerPositions.Add(new Vector2(xPositions[i], yPositions[i]));
//                            }

//                            EntityManager.Instance.ReceiveTowerUpdates(towers, towerPositions);

//                            UDPClient.Instance.SendUpdateConfirmed();

//                            break;

//                        case 3: // Tower Upgrade

//                            int towerType = (int)messageReceived[2];

//                            Vector3 towerPosition = new Vector3(messageReceived[3], messageReceived[4], messageReceived[5]);

//                            EntityManager.Instance.ReceiveTowerUpgrades(towerType, towerPosition);

//                            UDPClient.Instance.SendUpdateConfirmed();

//                            break;

//                        case 4: // Start Wave Update

//                            WaveManager.Instance.BeginWave();

//                            UDPClient.Instance.SendUpdateConfirmed();

//                            break;
//                    }
//                }
//            }

//        }
//    }

//    public static void StartClient()
//    {
//        if (!clientStarted)
//        {

//            buffer = new byte[512];

//            try
//            {
//                //REPLACE THE IP BELOW WITH YOUR AWS SERVER IP
//                //ip = IPAddress.Parse("54.208.168.94");
//                ip = IPAddress.Parse("127.0.0.1");
//                server = new IPEndPoint(ip, 11112);

//                client = new Socket(AddressFamily.InterNetwork,
//                    SocketType.Stream, ProtocolType.Tcp);

//                try
//                {
//                    print("Connecting to Server...");
//                    client.Connect(server);

//                    print("Connected to IP: " + client.RemoteEndPoint.ToString());

//                    client.Blocking = false;

//                }
//                catch (ArgumentNullException anexc)
//                {
//                    print("ArgumentNullException: " + anexc.ToString());
//                }
//                catch (SocketException sexc)
//                {
//                    print("SocketException: " + sexc.ToString());
//                }
//                catch (Exception exc)
//                {
//                    print("Unexpected exception: " + exc.ToString());
//                }
//            }
//            catch (Exception e)
//            {
//                print("Exception: " + e.ToString());
//            }

//            clientStarted = true;
//        }
//    }

//    public void SetUserList(string[] users)
//    {
//        TextMeshProUGUI[] oldUsers = userListContent.GetComponentsInChildren<TextMeshProUGUI>();

//        if (oldUsers.Length == 3)
//        {
//            for (int i = 0; i < users.Length; i++)
//            {
//                if (users[i] == null)
//                {
//                    oldUsers[i].GetComponentInParent<Image>().color = Color.gray;
//                    oldUsers[i].text = "";
//                }
//                else
//                {
//                    switch (i)
//                    {
//                        case 0:

//                            oldUsers[i].GetComponentInParent<Image>().color = Color.cyan;
//                            break;

//                        case 1:

//                            oldUsers[i].GetComponentInParent<Image>().color = Color.red;
//                            break;

//                        case 2:

//                            oldUsers[i].GetComponentInParent<Image>().color = Color.yellow;
//                            break;
//                    }


//                    oldUsers[i].text = users[i];
//                }

//                for (int j = 0; j < oldUsers.Length; j++)
//                {
//                    if (username == oldUsers[i].text)
//                    {
//                        //clientNum = i;

//                        if (isReady)
//                        {
//                            oldUsers[i].GetComponentInParent<Image>().gameObject.GetComponentInChildren<Button>().image.color = Color.green;
//                        }
//                        else
//                        {
//                            oldUsers[i].GetComponentInParent<Image>().gameObject.GetComponentInChildren<Button>().image.color = Color.gray;
//                        }
//                    }
//                }
//            }
//        }
//        else
//        {
//            for (int i = 0; i < oldUsers.Length; i++)
//            {
//                Destroy(oldUsers[i].gameObject);
//            }

//            for (int i = 0; i < users.Length; i++)
//            {
//                if (users[i] == null)
//                {
//                    userPrefab.GetComponent<Image>().color = Color.gray;
//                    userPrefab.GetComponentInChildren<TextMeshProUGUI>().text = "";
//                }
//                else
//                {
//                    switch (i)
//                    {
//                        case 0:

//                            userPrefab.GetComponent<Image>().color = Color.cyan;
//                            break;

//                        case 1:

//                            userPrefab.GetComponent<Image>().color = Color.red;
//                            break;

//                        case 2:

//                            userPrefab.GetComponent<Image>().color = Color.yellow;
//                            break;
//                    }

//                    userPrefab.GetComponentInChildren<TextMeshProUGUI>().text = users[i];
//                }

//                Instantiate(userPrefab, userListContent.transform);
//            }
//        }
//    }

//    public void UpdateMessageHistory()
//    {
//        TextMeshProUGUI[] oldMessages = messageHistoryContent.GetComponentsInChildren<TextMeshProUGUI>();

//        int count = 0;
//        foreach(TextMeshProUGUI message in oldMessages)
//        {
//            message.text = messageHistory[count].Item2;

//            count++; 
//        }

//        if (count < messageHistory.Count)
//        {
//            for (int i = count; i < messageHistory.Count; i++)
//            {
//                messagePrefab.GetComponentInChildren<TextMeshProUGUI>().text = messageHistory[count].Item2;

//                for (int j = 0; j < connectedUsers.Length; j++)
//                {
//                    if (messageHistory[i].Item1 == connectedUsers[0])
//                    {
//                        messagePrefab.GetComponent<Image>().color = Color.cyan;
//                    }
//                    if (messageHistory[i].Item1 == connectedUsers[1])
//                    {
//                        messagePrefab.GetComponent<Image>().color = Color.red;
//                    }
//                    if (messageHistory[i].Item1 == connectedUsers[2])
//                    {
//                        messagePrefab.GetComponent<Image>().color = Color.yellow;
//                    }
//                }

//                Instantiate(messagePrefab, messageHistoryContent.transform);
//            }
//        }


//        messageHistoryContent.GetComponent<RectTransform>().sizeDelta = new Vector2(messageHistoryContent.GetComponent<RectTransform>().sizeDelta.x, 100 * messageHistory.Count);
//    }

//    public void UpdateSelectedCharacters(string playerIndex, string character, bool confirmed = false)
//    {
//        int index = int.Parse(playerIndex);

//        if (confirmed)
//        {
//            takenCharacters.Add(character);
//            playersCharacters[index] = character;
//        }

//        switch (character)
//        {
//            case "Drumstick":

//                if (confirmed)
//                {
//                    playerIcons[index].sprite = confirmedCharacterImages[0];
//                }
//                else
//                {
//                    playerIcons[index].sprite = unconfirmedCharacterImages[0];
//                }

//                break;

//            case "Rolfe":

//                if (confirmed)
//                {
//                    playerIcons[index].sprite = confirmedCharacterImages[1];
//                }
//                else
//                {
//                    playerIcons[index].sprite = unconfirmedCharacterImages[1];
//                }

//                break;

//            case "Potter":

//                if (confirmed)
//                {
//                    playerIcons[index].sprite = confirmedCharacterImages[2];
//                }
//                else
//                {
//                    playerIcons[index].sprite = unconfirmedCharacterImages[2];
//                }

//                break;
//        }
//    }

//    public void BeginGameState()
//    {

//        currentState = ClientStates.Gameplay;

//        gameObject.AddComponent<UDPClient>();

//        SceneManager.LoadScene("Main");
//    }

//    public void SetIsReady()
//    {
//        isReady = !isReady;

//        SetUserList(connectedUsers);

//        string readyMessage = $"PLAYER_READY_STATUS.KEY:{isReady.ToString()}";

//        byte[] msg = Encoding.ASCII.GetBytes(readyMessage);

//        client.Send(msg);
//    }

//    public void RequestLeaderboard()
//    {
//        string requestMessage = $"LEADERBOARD_MSG_REQUEST.KEY:{clientNum}";

//        byte[] msg = Encoding.ASCII.GetBytes(requestMessage);

//        client.Send(msg);

//        showLeaderboard = !showLeaderboard;

//        if (showLeaderboard)
//        {
//            leaderboardPanel.SetActive(true);
//        }
//        else
//        {
//            leaderboardPanel.SetActive(false);
//        }
//    }

//    void SetLeaderboardInfo()
//    {
//        print("Setting Info For Leaderboard");
//        TextMeshProUGUI[] oldStats = leaderboardContent.GetComponentsInChildren<TextMeshProUGUI>();

//        bool addNewEntry = false;

//        SortCurrentLeaderboard();

//        for (int i = 0; i < currentLeaderboardMessageSplits.Count; i++)
//        {
//            for (int j = 0; j < oldStats.Length; j++)
//            {
//                if (oldStats[j].text.Contains(currentLeaderboardMessageSplits[i][0]))
//                {
//                    addNewEntry = false;
//                    break;
//                }
                
//                if (j == oldStats.Length - 1 && !oldStats[j].text.Contains(currentLeaderboardMessageSplits[i][0]))
//                {
//                    addNewEntry = true;
//                }
//            }

//            if (oldStats.Length == 0)
//            {
//                addNewEntry = true;
//            }

//            if (addNewEntry)
//            {
//                messagePrefab.GetComponent<Image>().color = Color.gray;
//                messagePrefab.GetComponentInChildren<TextMeshProUGUI>().text 
//                    = currentLeaderboardMessageSplits[i][0] + ": " + currentLeaderboardMessageSplits[i][1] + " Waves";

//                Instantiate(messagePrefab, leaderboardContent.transform);

//                addNewEntry = false;
//            }
//        }

//        currentLeaderboardMessageSplits = new List<string[]>();
//    }

//    void SortCurrentLeaderboard()
//    {
//        string[] temp;

//        for (int i = 0; i < currentLeaderboardMessageSplits.Count; i++)
//        {
//            for (int j = i + 1; j < currentLeaderboardMessageSplits.Count; j++)
//            {
//                if (currentLeaderboardMessageSplits[i][0] == currentLeaderboardMessageSplits[j][0])
//                {
//                    if (int.Parse(currentLeaderboardMessageSplits[i][1]) >= int.Parse(currentLeaderboardMessageSplits[j][1]))
//                    {
//                        currentLeaderboardMessageSplits.RemoveAt(j);
//                    }
//                    else
//                    {
//                        currentLeaderboardMessageSplits.RemoveAt(i);
//                    }
//                }
//            }
//        }

//        for (int i = 0; i < currentLeaderboardMessageSplits.Count; i++)
//        {
//            for (int j = i + 1; j < currentLeaderboardMessageSplits.Count; j++)
//            {
//                if (int.Parse(currentLeaderboardMessageSplits[j][1]) > int.Parse(currentLeaderboardMessageSplits[i][1]))
//                {
//                    temp = currentLeaderboardMessageSplits[j];
//                    currentLeaderboardMessageSplits[j] = currentLeaderboardMessageSplits[i];
//                    currentLeaderboardMessageSplits[i] = temp;
//                }
//            }
//        }
//    }

//    public void SetSelectedCharacter(string character)
//    {
//        if (!isConfirmed)
//        {
//            for (int i = 0; i < takenCharacters.Count; i++)
//            {
//                if (takenCharacters[i] == character)
//                {
//                    return;
//                }
//            }

//            selectedCharacter = character;

//            string characterMessage = $"NEW_CHARACTER/SELECTED.KEY:{selectedCharacter}";

//            byte[] msg = Encoding.ASCII.GetBytes(characterMessage);

//            client.Send(msg);
//        }
//    }

//    public void ConfirmSelectedCharacter()
//    {
//        if (!isConfirmed && selectedCharacter != "Agumon")
//        {
//            for (int i = 0; i < takenCharacters.Count; i++)
//            {
//                if (takenCharacters[i] == selectedCharacter)
//                {
//                    return;
//                }
//            }

//            takenCharacters.Add(selectedCharacter);

//            for (int i = 0; i < connectedUsers.Length; i++)
//            {
//                if (connectedUsers[i] == username)
//                {
//                    playersCharacters[i] = selectedCharacter;
//                }
//            }

//            string confirmedMessage = $"CONFIRMED_CHARACTER/SELECTED.KEY";

//            byte[] msg = Encoding.ASCII.GetBytes(confirmedMessage);

//            client.Send(msg);

//            isConfirmed = true;
//        }
//    }

//    public void ExitClient()
//    {
//        string message = "exit";

//        //Send data to client
//        byte[] msg = Encoding.ASCII.GetBytes(message);

//        client.Send(msg);

//        client.Shutdown(SocketShutdown.Both);
//        client.Close();

//        userListPanel.SetActive(false);
//        namePanel.SetActive(true);

//        nameChosen = false;
//        username = null;
//        clientStarted = false;
//    }

//    public string GetUsername()
//    {
//        return username;
//    }

//    public string[] GetConnectedUsers()
//    {
//        return connectedUsers;
//    }

//    public bool GetClientStarted()
//    {
//        return clientStarted;
//    }

//    public string[] GetPlayersCharacters()
//    {
//        return playersCharacters;
//    }

//    public void SetSelectedCharacterIndex(int index)
//    {
//        selectedCharacterIndex = index;
//    }

//    public int GetSelectedCharacterIndex()
//    {
//        return selectedCharacterIndex;
//    }

//    public void SetClientNum(int num)
//    {
//        clientNum = num;
//    }

//    public int GetClientNum()
//    {
//        return clientNum;
//    }
//}
