using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;

public class PlayerClient : MonoBehaviour
{
    [SerializeField] TMP_InputField messageInput;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] GameObject messagePanel;
    [SerializeField] GameObject namePanel;
    [SerializeField] TextMeshProUGUI nameText;

    static IPAddress ip;
    static IPEndPoint server;
    static Socket client;
    static byte[] buffer;

    static bool nameChosen;

    static bool clientStarted;

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
                print("Recieved: " + Encoding.ASCII.GetString(buffer, 0, recv));
            }

            string message;

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
                if (message == "exit")
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }

                client.Send(msg);

                if (!nameChosen)
                {
                    nameChosen = true;

                    namePanel.SetActive(false);
                    messagePanel.SetActive(true);

                    print($"Name: {message}");

                    nameText.text = message;
                }
                else
                {
                    print($"Sent: {message}");
                }

            }
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
            server = new IPEndPoint(ip, 11111);

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
}
