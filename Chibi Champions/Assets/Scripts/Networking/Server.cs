using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using TMPro;

public class Server : MonoBehaviour
{
    TMP_InputField input;
    string receivedMessage;
    string messageToSend;

    private static byte[] buffer;
    private static byte[] message;
    private static IPHostEntry hostInfo;
    private static IPAddress ip;
    private static IPEndPoint localEP;
    private static Socket server;
    private static IPEndPoint client;
    private static int rec = 0;
    private static Socket handler;
    private static List<Socket> handlers = new List<Socket>();

    private static List<ClientInformation> usersList = new List<ClientInformation>();

    private static List<string> activeUsers = new List<string>();

    static bool shouldSendNames;

    ServerStates currentState = ServerStates.Lobby;

    int[] requestMessage;

    int savedIndex;
    int[] connectedIndices = new int[2];

    void Start()
    {
        input = FindObjectOfType<TMP_InputField>();

        StartServer();

        //Non-Blocking Mode
        server.Blocking = false;

        int num = 0;

        foreach(Socket handler in handlers)
        {
            handlers[num].Blocking = false;

            num++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbyManager.Instance.GetActivePanel() == LobbyPanels.UserList && currentState != ServerStates.Request)
        {
            currentState = ServerStates.Lobby;
        }
        else if (LobbyManager.Instance.GetActivePanel() == LobbyPanels.Message && currentState != ServerStates.Request)
        {
            currentState = ServerStates.Chatting;
        }

        if (currentState == ServerStates.Request)
        {
            requestMessage[1] = savedIndex;

            int currentUser = 1;
            int iteration = 0;
            foreach (ClientInformation user in usersList)
            {
                if (user.GetRunConnection() && user.GetNameSelected())
                {
                    if (user.GetName() == usersList[requestMessage[0]].GetName())
                    {
                        requestMessage[1]++;
                    }
                    else if(user.GetName() == usersList[requestMessage[1]].GetName())
                    {
                        connectedIndices[0] = requestMessage[0];
                        connectedIndices[1] = requestMessage[1];

                        messageToSend = "MESSAGE:REQUEST/SENT.Key";

                        message = Encoding.ASCII.GetBytes(messageToSend);

                        SendMessageToSingleClient(iteration);

                        currentState = ServerStates.Chatting;

                        LobbyManager.Instance.SetMessagePanelActive();
                    }
                    else
                    {
                        currentUser++;
                    }
                }

                iteration++;
            }
        }
        else if (currentState == ServerStates.Lobby)
        {
            LobbyManager.Instance.SetUserList(activeUsers);

            int currentUser = 1;
            foreach (ClientInformation user in usersList)
            {
                rec = 0;

                if (!user.GetNameSelected())
                {
                    ReceiveName(user);
                }
                else
                {
                    if (user.GetRunConnection() && user.GetNameSelected())
                    {
                        if (shouldSendNames)
                        {
                            messageToSend = user.GetName();

                            print($"Sending Name: {messageToSend}");

                            message = Encoding.ASCII.GetBytes(messageToSend);

                            SendNames(messageToSend);
                        }

                        try
                        {
                            rec = handlers[currentUser - 1].Receive(buffer);
                        }
                        catch (SocketException e)
                        {
                            if (e.SocketErrorCode == SocketError.ConnectionReset)
                            {
                                handlers[currentUser - 1] = null;
                                user.SetRunConnection(false);
                            }
                        }
                        catch (Exception e)
                        {
                            print(e.ToString());
                        }

                        if (rec > 0)
                        {
                            requestMessage = new int[rec / sizeof(int)];

                            Buffer.BlockCopy(buffer, 0, requestMessage, 0, rec);

                            requestMessage[0] = currentUser - 1;

                            savedIndex = requestMessage[1];

                            print(requestMessage[0] + " || " + requestMessage[1]);

                            print("Received Request From A Client");

                            currentState = ServerStates.Request;
                        }
                    }
                }
            }
        }
        else
        {
            int currentUser = 1;

            foreach (ClientInformation user in usersList)
            {
                rec = 0;

                if (handlers[currentUser - 1] == null)
                {
                    print("Waiting To Reconnect...");

                    handlers[currentUser - 1] = server.Accept();

                    print("Successfully Reconnected!");

                    client = (IPEndPoint)handlers[currentUser - 1].RemoteEndPoint;

                    print($"Client {client.Address} connected at port {client.Port}");

                    handlers[currentUser - 1].Blocking = false;

                    user.SetRunConnection(true);
                }

                if (!user.GetNameSelected())
                {
                    ReceiveName(user);
                }
                else
                {
                    if (user.GetRunConnection() && user.GetNameSelected())
                    {
                        try
                        {
                            rec = handlers[currentUser - 1].Receive(buffer);
                        }
                        catch (SocketException e)
                        {
                            print(e.SocketErrorCode);

                            if (e.SocketErrorCode == SocketError.ConnectionReset)
                            {
                                handlers[currentUser - 1] = null;
                                user.SetRunConnection(false);
                            }
                        }
                        catch (Exception e)
                        {
                            print(e.ToString());
                        }

                        if (rec > 0)
                        {
                            receivedMessage = Encoding.ASCII.GetString(buffer, 0, rec);

                            Debug.Log("Recieved: " + Encoding.ASCII.GetString(buffer, 0, rec));

                            if (receivedMessage != "NO:MESSAGE/SENT.KEY")
                            {
                                LobbyManager.Instance.SetMessage($"{user.GetName()}: {receivedMessage}");
                            }

                            messageToSend = receivedMessage;

                            message = Encoding.ASCII.GetBytes(messageToSend);

                            if (messageToSend != "MESSAGE:REQUEST_ACCEPTED.KEY")
                            {
                                SendMessageToListOfClients(connectedIndices);
                            }
                        }
                    }
                }

                currentUser++;
            }
        }

        SearchForMoreClients();
    }

    public static void StartServer()
    {
        buffer = new byte[512];
        hostInfo = Dns.GetHostEntry(Dns.GetHostName());
        //ip = hostInfo.AddressList[4];
        ip = IPAddress.Parse("127.0.0.1");

        print($"Server Name: {hostInfo.HostName} | IP: {ip}");

        localEP = new IPEndPoint(ip, 11111);
        server = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            server.Bind(localEP);
            server.Listen(10);

            print("Waiting for Connection...");

            handlers.Add(server.Accept());

            print($"Connected To Socket Number {handlers.Count}");

            usersList.Add(new ClientInformation(handlers[handlers.Count - 1]));

            client = (IPEndPoint)handlers[handlers.Count - 1].RemoteEndPoint;

            print($"Client {client.Address} connected at port {client.Port}");

        }
        catch (SocketException e)
        {
            print(e.SocketErrorCode);
        }
    }

    void SearchForMoreClients()
    {
        try
        {
            handlers.Add(server.Accept());

            print($"Connected To Socket Number {handlers.Count}");

            usersList.Add(new ClientInformation(handlers[handlers.Count - 1]));

            client = (IPEndPoint)handlers[handlers.Count - 1].RemoteEndPoint;

            LobbyManager.Instance.SetMessage($"Found New Client: {client.Address} connected at port: {client.Port}");
        }
        catch(Exception e)
        {
            return;
        }
    }

    public void ActivateSendMessage()
    {
        if (messageToSend != "NO:MESSAGE/SENT.KEY")
        {
            LobbyManager.Instance.SetMessage($"Server: {messageToSend}", true);
        }

        SendMessage();
    }

    public static void SendMessage()
    {
        int currentHandler = 0;
        foreach (Socket handler in handlers)
        {
            handlers[currentHandler].Send(message);

            currentHandler++;
        }
    }

    public static void SendMessageToSingleClient(int clientIndex)
    {
        handlers[clientIndex].Send(message);
    }

    public static void SendMessageToListOfClients(int[] clientsToSend)
    {
        print("Sending Messages Back To Clients");

        foreach(int index in clientsToSend)
        {
            handlers[index].Send(message);
        }
    }

    public static void SendNames(string name)
    {
        int currentHandler = 0;
        foreach (Socket handler in handlers)
        {
            if (name != usersList[currentHandler].GetName())
            {
                handlers[currentHandler].Send(message);

                print($"Sending: {name} To {usersList[currentHandler].GetName()}");
            }

            currentHandler++;
        }

        if (name == usersList[usersList.Count - 1].GetName())
        {
            print("Setting ShouldSentNames to false");
            shouldSendNames = false;
        }
    }

    public void ShutdownServer()
    {
        server.Shutdown(SocketShutdown.Both);
        server.Close();
    }

    void ReceiveName(ClientInformation currentClient)
    {
        Socket currentHandler = currentClient.GetHandler();

        if (currentClient.GetRunConnection())
        {
            try
            {
                rec = currentHandler.Receive(buffer);
            }
            catch (SocketException e)
            {
                print(e.SocketErrorCode);

                if (e.SocketErrorCode == SocketError.ConnectionReset)
                {
                    currentHandler = null;
                    currentClient.SetRunConnection(false);
                }
            }
            catch (Exception e)
            {
                print(e.ToString());
            }

            if (rec > 0)
            {
                receivedMessage = Encoding.ASCII.GetString(buffer, 0, rec);

                currentClient.SetName(receivedMessage);

                activeUsers.Add(receivedMessage);

                print("Setting ShouldSendNames to True");

                shouldSendNames = true;
            }
        }
    }
}

