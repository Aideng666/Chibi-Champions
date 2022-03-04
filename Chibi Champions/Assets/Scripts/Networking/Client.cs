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
    //public GameObject cube;
    [SerializeField] TMP_InputField input;
    private static byte[] outBuffer = new byte[512];
    private static IPEndPoint remoteEP;
    private static Socket client;

    string message;

    //Lecture 5
    //private float[] pos;
    //private byte[] bpos;

    //Lecture 5 Exercise
    //Vector3 previousPos;
    //Vector3 currentPos;


    // Update is called once per frame
    void Update()
    {
        message = input.text;

        //currentPos = cube.transform.position;

        //print($"Sending Position: {cube.transform.position.x}, 0.5, {cube.transform.position.z}");

        outBuffer = Encoding.ASCII.GetBytes(message);

        //pos = new float[] { cube.transform.position.x, cube.transform.position.y, cube.transform.position.z };

        //Buffer.BlockCopy(pos, 0, bpos, 0, bpos.Length);


        //if (currentPos != previousPos)
        //{
        //    client.SendTo(bpos, remoteEP);
        //}
        
        //client.SendTo(outBuffer, remoteEP);

        //previousPos = currentPos;
    }

    public static void SendMessage()
    {
        client.SendTo(outBuffer, remoteEP);
    }

    public static void StartClient()
    {
        // rcv buffer

        byte[] buffer = new byte[512];

        try
        {
            //IPAddress ip = IPAddress.Parse("192.168.0.53");
            IPAddress ip = IPAddress.Parse("127.0.0.1");

            remoteEP = new IPEndPoint(ip, 11111);

            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
