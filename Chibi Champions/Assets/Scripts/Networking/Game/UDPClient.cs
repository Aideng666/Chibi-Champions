using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;

public class UDPClient : MonoBehaviour
{
    static IPAddress ip;
    static Socket client;
    static IPEndPoint remoteEP;

    static bool clientStarted;

    // Start is called before the first frame update
    void Start()
    {
        StartUDPClient();
    }

    // Update is called once per frame
    void Update()
    {
        if (clientStarted)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                string message = PlayerClient.Instance.GetUsername();

                byte[] msg = Encoding.ASCII.GetBytes(message);

                client.SendTo(msg, remoteEP);
            }
        }
    }

    public static void StartUDPClient()
    {
        try
        {
            ip = IPAddress.Parse("127.0.0.1");

            remoteEP = new IPEndPoint(ip, 11111);

            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        catch(Exception e)
        {

        }

        clientStarted = true;
    }
}
