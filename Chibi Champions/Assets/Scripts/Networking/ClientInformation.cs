//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Net;
//using System.Net.Sockets;

//public class ClientInformation
//{
//    private static Socket handler;

//    private string username;

//    bool nameSelected;
//    bool runConnection;

//    public ClientInformation(Socket socket)
//    {
//        handler = socket;
//        username = "noName";

//        runConnection = true;

//        nameSelected = false;

//        handler.Blocking = false;
//    }

//    public void SetName(string name)
//    {
//        username = name;

//        SetNameSelected(true);
//    }

//    public string GetName()
//    {
//        return username;
//    }

//    public void SetNameSelected(bool selected)
//    {
//        nameSelected = selected;
//    }

//    public bool GetNameSelected()
//    {
//        return nameSelected;
//    }

//    public void SetRunConnection(bool run)
//    {
//        runConnection = run;
//    }

//    public bool GetRunConnection()
//    {
//        return runConnection;
//    }

//    public Socket GetHandler()
//    {
//        return handler;
//    }
//}
