//using System;
//using System.Text;
//using System.Net;
//using System.Net.Sockets;
//using System.Collections.Generic;
//using UnityEngine;

//public class UDPClient : MonoBehaviour
//{
//    static IPAddress ip;
//    static Socket client;
//    static IPEndPoint remoteEP;

//    static bool clientStarted;

//    public static UDPClient Instance { get; set; }

//    private void Awake()
//    {
//        Instance = this;
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        StartUDPClient();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (clientStarted)
//        {

//        }
//    }

//    public void SetPlayerPos(int playerIndex, Vector3 newPos, Vector3 newRotation)
//    {
//        if (clientStarted)
//        {
//            float[] pos = new float[] { (int)MessageTypes.PlayerMovement, playerIndex, newPos.x, newPos.y, newPos.z, newRotation.x, newRotation.y, newRotation.z };
//            byte[] bpos = new byte[pos.Length * sizeof(float)];

//            Buffer.BlockCopy(pos, 0, bpos, 0, bpos.Length);

//            client.SendTo(bpos, remoteEP);
//        }
//    }

//    public void SendPlayerUpdates(ActionTypes updateType, CharacterNames characterName)
//    {
//        if (clientStarted)
//        {
//            float[] msg = new float[] { (int)MessageTypes.PlayerAction, PlayerClient.Instance.GetClientNum(), (int)characterName, (int)updateType };

//            byte[] bpos = new byte[msg.Length * sizeof(float)];

//            Buffer.BlockCopy(msg, 0, bpos, 0, bpos.Length);

//            client.SendTo(bpos, remoteEP);
//        }
//    }

//    public void SendTowers(int clientIndex, Tower[] towers)
//    {
//        if (clientStarted)
//        {
//            List<float> towerInfo = new List<float>();

//            towerInfo.Add((int)MessageTypes.TowerUpdate);
//            towerInfo.Add(clientIndex);

//            for (int i = 0; i < towers.Length; i++)
//            {
//                towerInfo.Add((int)towers[i].GetTowerType());
//            }

//            for (int i = 0; i < towers.Length; i++)
//            {
//                towerInfo.Add(towers[i].transform.position.x);
//                towerInfo.Add(towers[i].transform.position.z);
//            }

//            float[] towerMsg = new float[towerInfo.Count];

//            for (int i = 0; i < towerMsg.Length; i++)
//            {
//                towerMsg[i] = towerInfo[i];
//            }

//            byte[] bpos = new byte[towerMsg.Length * sizeof(float)];

//            Buffer.BlockCopy(towerMsg, 0, bpos, 0, bpos.Length);

//            client.SendTo(bpos, remoteEP);

//            //string message = $"TOWER_UPDATE_SENT.KEY:{clientIndex}";

//            //for (int i = 0; i < towers.Length; i++)
//            //{
//            //    message += ":" + towers[i].GetTowerName();
//            //}

//            //for (int i = 0; i < towers.Length; i++)
//            //{
//            //    message += ":" + towers[i].GetLevel();
//            //}

//            //for (int i = 0; i < towers.Length; i++)
//            //{
//            //    message += ":" + towers[i].transform.position.x + ":" + towers[i].transform.position.z;
//            //}

//            //byte[] msg = Encoding.ASCII.GetBytes(message);

//            //client.SendTo(msg, remoteEP);
//        }
//    }

//    public void SendTowerUpgrade(Vector3 position, TowerType towerType)
//    {
//        if (clientStarted)
//        {
//            float[] msg = new float[]
//            { (int)MessageTypes.TowerUpgrade, PlayerClient.Instance.GetClientNum(), (int)towerType, position.x, position.y, position.z};

//            byte[] bpos = new byte[msg.Length * sizeof(float)];

//            Buffer.BlockCopy(msg, 0, bpos, 0, bpos.Length);

//            client.SendTo(bpos, remoteEP);

//            //string message = $"TOWER_UPGRADE_SENT.KEY:{PlayerClient.Instance.GetClientNum()}:{towerName}:{position.x}:{position.y}:{position.z}";

//            //byte[] msg = Encoding.ASCII.GetBytes(message);

//            //client.SendTo(msg, remoteEP);
//        }
//    }

//    public void SendStartWave()
//    {
//        if (clientStarted)
//        {
//            float[] msg = new float[] { (int)MessageTypes.StartWave };

//            byte[] bpos = new byte[msg.Length * sizeof(float)];

//            Buffer.BlockCopy(msg, 0, bpos, 0, bpos.Length);

//            client.SendTo(bpos, remoteEP);

//            //string message = "STARTING_WAVE_MESSAGE.KEY";

//            //byte[] msg = Encoding.ASCII.GetBytes(message);

//            //client.SendTo(msg, remoteEP);
//        }
//    }

//    public void SendUpdateConfirmed()
//    {
//        if (clientStarted)
//        {
//            float[] msg = new float[] { (int)MessageTypes.UpdateConfirmed, PlayerClient.Instance.GetClientNum() };

//            byte[] bpos = new byte[msg.Length * sizeof(float)];

//            Buffer.BlockCopy(msg, 0, bpos, 0, bpos.Length);

//            client.SendTo(bpos, remoteEP);
//        }
//    }

//    public void SendLeaderboardStats()
//    {
//        if (clientStarted)
//        {
//            int totalWavesCompleted = PlayerPrefs.GetInt("WavesCompleted");

//            print(totalWavesCompleted);

//            float[] msg = new float[] { (int)MessageTypes.LeaderboardStatus, PlayerClient.Instance.GetClientNum(), totalWavesCompleted };

//            byte[] bpos = new byte[msg.Length * sizeof(float)];

//            Buffer.BlockCopy(msg, 0, bpos, 0, bpos.Length);

//            client.SendTo(bpos, remoteEP);

//        }
//    }

//    public static void StartUDPClient()
//    {
//        try
//        {
//            ip = IPAddress.Parse("127.0.0.1");
//            //ip = IPAddress.Parse("54.208.168.94");

//            remoteEP = new IPEndPoint(ip, 11111);

//            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
//        }
//        catch(Exception e)
//        {

//        }

//        clientStarted = true;
//    }


//    public bool GetIsClientStarted()
//    {
//        return clientStarted;
//    }
//}
