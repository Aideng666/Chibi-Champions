using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Server : MonoBehaviour
{
    //[SerializeField] GameObject cube;
    string message;

    private static byte[] buffer;
    private static IPHostEntry hostInfo;
    private static IPAddress ip;
    private static IPEndPoint localEP;
    private static Socket server;
    private static IPEndPoint client;
    private static EndPoint remoteClient;
    private static int rec = 0;

    //Lecture 5
    //private float[] pos;

    private static bool serverRunning;

    void Start()
    {
        StartServer();

        //Lecture 5
        //Non-Blocking Mode
        server.Blocking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (serverRunning)
        {
            try
            {
                rec = server.ReceiveFrom(buffer, ref remoteClient);
            }
            catch (SocketException e)
            {
                print(e.ToString());
            }

            print($"Received from: {remoteClient.ToString()}");

            message = Encoding.ASCII.GetString(buffer, 0, rec);

            LobbyManager.Instance.SetMessage(message);

            print(message);

            //LECTURE 5
            //pos = new float[rec / 4];

            //Buffer.BlockCopy(buffer, 0, pos, 0, rec);

            //cube.transform.position = new Vector3((float)Convert.ToDouble(xPos), 0.5f, (float)Convert.ToDouble(zPos));
            //cube.transform.position = new Vector3(pos[0], pos[1], pos[2]);

            //print($"Received Position: {cube.transform.position}");

            // If client sends floats
            // print($"Data: {BitConverter.ToSingle(buffer, 0)}");
        }

        //if (cube.transform.position.x > 5 || cube.transform.position.x < -5 ||
        //    cube.transform.position.z > 5 || cube.transform.position.z < -5)
        //{
        //    ShutdownServer();
        //}
    }

    public static void StartServer()
    {
        buffer = new byte[512];
        hostInfo = Dns.GetHostEntry(Dns.GetHostName());
        //ip = hostInfo.AddressList[4];
        ip = IPAddress.Parse("127.0.0.1");

        print($"Server Name: {hostInfo.HostName} | IP: {ip}");

        localEP = new IPEndPoint(ip, 11111);
        server = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        client = new IPEndPoint(IPAddress.Any, 0);
        remoteClient = (EndPoint)client;

        try
        {
            server.Bind(localEP);

            print("Waiting for data...");
        }
        catch (Exception e)
        {
            print(e.ToString());
        }

        serverRunning = true;
    }

    public void ShutdownServer()
    {
        server.Shutdown(SocketShutdown.Both);
        server.Close();

        serverRunning = false;
    }
}

