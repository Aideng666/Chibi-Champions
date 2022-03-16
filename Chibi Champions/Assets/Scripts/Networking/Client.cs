using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;

public class Client : MonoBehaviour
{
    TMP_InputField input;
    private static byte[] buffer;
    private static byte[] message;
    private static IPEndPoint remoteEP;
    private static Socket client;
    private static int rec = 0;

    string messageToSend;
    string receivedMessage;

    static bool nameSelected;
    static string username = "noName";

    ClientStates currentState = ClientStates.Lobby;

    List<string> otherClientsOnline = new List<string>();

    int[] requestMessage;
    byte[] bRequest;

    //bool sendName;
    static string nameToRequest = "";

    void Start()
    {
        input = FindObjectOfType<TMP_InputField>();

        StartClient();

        client.Blocking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbyManager.Instance.GetActivePanel() == LobbyPanels.UserList && currentState != ClientStates.Request)
        {
            currentState = ClientStates.Lobby;
        }
        else if (LobbyManager.Instance.GetActivePanel() == LobbyPanels.Message && currentState != ClientStates.Request)
        {
            currentState = ClientStates.Chatting;
        }

        if (!nameSelected)
        {
            SelectName();
        }
        else if(currentState == ClientStates.Request)
        {
            LobbyManager.Instance.ActivateRequestPanel();

            messageToSend = "MESSAGE:REQUEST_ACCEPTED.KEY";

            message = Encoding.ASCII.GetBytes(messageToSend);
        }
        else if(currentState == ClientStates.Lobby)
        {
            rec = 0;

            if (nameToRequest != "")
            {
                SetRequestInfo();
            }

            try
            {
                rec = client.Receive(buffer);
            }
            catch (SocketException e)
            {
                //print(e.SocketErrorCode);
            }

            if (rec > 0)
            {
                receivedMessage = Encoding.ASCII.GetString(buffer, 0, rec);

                if (receivedMessage == "MESSAGE:REQUEST/SENT.Key")
                {
                    currentState = ClientStates.Request;
                }
                else
                {
                    foreach (string clientName in otherClientsOnline)
                    {
                        if (receivedMessage == clientName || receivedMessage == username)
                        {
                            return;
                        }
                    }

                    otherClientsOnline.Add(receivedMessage);

                    print($"Total Clients Online: {otherClientsOnline.Count + 1}");

                    LobbyManager.Instance.SetUserList(otherClientsOnline);
                }
            }
        }
        else
        {
            rec = 0;
            input = FindObjectOfType<TMP_InputField>();

            if (input.text.Length > 0)
            {
                messageToSend = input.text;
            }
            else
            {
                messageToSend = "NO:MESSAGE/SENT.KEY";
            }

            message = Encoding.ASCII.GetBytes(messageToSend);

            try
            {
                rec = client.Receive(buffer);
            }
            catch (SocketException e)
            {
                print(e.SocketErrorCode);
            }

            if (rec > 0)
            {
                receivedMessage = Encoding.ASCII.GetString(buffer, 0, rec);

                if (receivedMessage != "NO:MESSAGE/SENT.KEY")
                {
                    LobbyManager.Instance.SetMessage($"Received: {receivedMessage}");
                }
            }
        }
    }

    public void ActivateSendMessage()
    {
        if (messageToSend != "NO:MESSAGE/SENT.KEY" && nameSelected)
        {
            LobbyManager.Instance.SetMessage($"{username}: {messageToSend}", true);
        }

        if (!nameSelected)
        {
            username = messageToSend;
        }

        SendMessage();
    }

    public void SetNameToRequest(string nameOfRequested)
    {
        nameToRequest = nameOfRequested;
    }

    public void ActivateSendMessageRequest()
    {
        SendMessageRequest();
    }

    public static void SendMessage()
    {
        client.Send(message);

        print("Message Sent From Client");

        if (!nameSelected)
        {
            nameSelected = true;
        }
    }

    public void SendMessageRequest()
    {
        client.Send(bRequest);

        currentState = ClientStates.Chatting;
    }

    public void SetRequestInfo()
    {
        int currentIndex = 0;
        foreach(string name in otherClientsOnline)
        {
            if (name == nameToRequest)
            {
                requestMessage = new int[] { 0, currentIndex };
            }
        }

        bRequest = new byte[requestMessage.Length * sizeof(int)];

        Buffer.BlockCopy(requestMessage, 0, bRequest, 0, bRequest.Length);
    }

    public void AcceptMessageRequest()
    {
        print("Sending The Confirmation");

        client.Send(message);

        currentState = ClientStates.Chatting;
    }

    public void DeclineMessageRequest()
    {

    }

    public static void StartClient()
    {
        buffer = new byte[512];

        try
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");

            remoteEP = new IPEndPoint(ip, 11111);

            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try 
            {
                Debug.Log("Connecting to Server...");
                client.Connect(remoteEP);

                Debug.Log("Connected to IP: " + client.RemoteEndPoint.ToString());
            }
            catch (ArgumentNullException anexc)
            {
                Console.WriteLine("ArgumentNullException: {0}", anexc.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException: {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("UnexpectedException: {0}", e.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        nameSelected = false;
    }

    void SelectName()
    {
            if (input.text.Length > 0)
            {
                messageToSend = input.text;
            }
            else
            {
                messageToSend = "user";
            }

            message = Encoding.ASCII.GetBytes(messageToSend);
    }

    public void SetClientState(ClientStates clientState)
    {
        currentState = clientState;
    }

    public string GetUsername()
    {
        return username;
    }
}
