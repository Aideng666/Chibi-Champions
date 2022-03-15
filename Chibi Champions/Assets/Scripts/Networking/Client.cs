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

    void Start()
    {
        input = FindObjectOfType<TMP_InputField>();

        input.text = "Begin";

        StartClient();

        client.Blocking = false;
    }

    // Update is called once per frame
    void Update()
    {
        rec = 0;

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
        catch(Exception e)
        {
            print(e.ToString());
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

    public void ActivateSendMessage()
    {
        if (messageToSend != "NO:MESSAGE/SENT.KEY")
        {
            LobbyManager.Instance.SetMessage($"Sent: {messageToSend}", true);
        }

        SendMessage();
    }

    public static void SendMessage()
    {
        client.Send(message);
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
    }

    void WaitForSendMessage(KeyCode key)
    {
        bool done = false;

        while (!done)
        {
            if (Input.GetKeyDown(key))
            {
                done = true;
            }
        }
    }
}
