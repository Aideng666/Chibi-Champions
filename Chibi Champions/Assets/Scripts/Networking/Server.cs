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

    private static bool serverRunning;

    //string previousMessage;

    void Start()
    {
        input = FindObjectOfType<TMP_InputField>();

        input.text = "Return";

        StartServer();

        //Non-Blocking Mode
        server.Blocking = false;
        handler.Blocking = false;
    }

    // Update is called once per frame
    void Update()
    {
        rec = 0;

        if (handler == null)
        {
            print("Waiting To Reconnect...");

            handler = server.Accept();

            print("Successfully Reconnected!");

            client = (IPEndPoint)handler.RemoteEndPoint;

            print($"Client {client.Address} connected at port {client.Port}");

            handler.Blocking = false;

            serverRunning = true;
        }

        if (serverRunning)
        {
            try
            {
                rec = handler.Receive(buffer);
            }
            catch (SocketException e)
            {
                print(e.SocketErrorCode);

                if (e.SocketErrorCode == SocketError.ConnectionReset)
                {
                    handler = null;
                    serverRunning = false;
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
                    LobbyManager.Instance.SetMessage($"Received: {receivedMessage}");
                }
            }

            if (input.text.Length > 0)
            {
                messageToSend = input.text;
            }
            else
            {
                messageToSend = "NO:MESSAGE/SENT.KEY";
            }

            message = Encoding.ASCII.GetBytes(messageToSend);
        }
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

            handler = server.Accept();

            print("Connected!");

            client = (IPEndPoint)handler.RemoteEndPoint;

            print($"Client {client.Address} connected at port {client.Port}");
        }
        catch (Exception e)
        {
            print(e.ToString());
        }

        serverRunning = true;
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
        handler.Send(message);
    }

    public void ShutdownServer()
    {
        server.Shutdown(SocketShutdown.Both);
        server.Close();

        serverRunning = false;
    }

    void WaitForSendMessage(KeyCode key)
    {
        bool done = false;

        while (!done)
        {
            print("Waiting For Input");

            if (Input.GetKeyDown(key))
            {
                done = true;
            }
        }
    }
}

