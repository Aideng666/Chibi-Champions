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

    public static UDPClient Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

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

        }
    }

    public void SetPlayerPos(int playerIndex, Vector3 newPos, Vector3 newRotation)
    {
        if (clientStarted)
        {
            float[] pos = new float[] { playerIndex, newPos.x, newPos.y, newPos.z, newRotation.x, newRotation.y, newRotation.z };
            byte[] bpos = new byte[pos.Length * sizeof(float)];

            Buffer.BlockCopy(pos, 0, bpos, 0, bpos.Length);

            client.SendTo(bpos, remoteEP);
        }
    }

    public void SendTowers(Tower[] towers)
    {
        if (clientStarted)
        {
            float[] message = new float[towers.Length];
            
            for (int i = 0; i < towers.Length; i++)
            {

            }
        }
    }

    public void SendAnimationUpdates(string characterName, AnimationTypes type, bool animOn = true) // false means that the anim's bool was set to false no true
    {
        if (clientStarted)
        {
            string message;
            byte[] msg;

            //switch (type)
            //{
            //    case AnimationTypes.WalkForward:

            //        message = $"ANIMATION_TRIGGERED_MESSAGE.KEY:{characterName}:WalkForward:{animOn.ToString()}";

            //        msg = Encoding.ASCII.GetBytes(message);

            //        client.SendTo(msg, remoteEP);

            //        break;

            //    case AnimationTypes.WalkBackward:

            //        message = $"ANIMATION_TRIGGERED_MESSAGE.KEY:{characterName}:WalkBackward:{animOn.ToString()}";

            //        msg = Encoding.ASCII.GetBytes(message);

            //        client.SendTo(msg, remoteEP);

            //        break;

            //    case AnimationTypes.WalkLeft:

            //        message = $"ANIMATION_TRIGGERED_MESSAGE.KEY:{characterName}:WalkLeft:{animOn.ToString()}";

            //        msg = Encoding.ASCII.GetBytes(message);

            //        client.SendTo(msg, remoteEP);

            //        break;

            //    case AnimationTypes.WalkRight:

            //        message = $"ANIMATION_TRIGGERED_MESSAGE.KEY:{characterName}:WalkRight:{animOn.ToString()}";

            //        msg = Encoding.ASCII.GetBytes(message);

            //        client.SendTo(msg, remoteEP);

            //        break;

            //    case AnimationTypes.Jump:

            //        message = $"ANIMATION_TRIGGERED_MESSAGE.KEY:{characterName}:Jump:{animOn.ToString()}";

            //        msg = Encoding.ASCII.GetBytes(message);

            //        client.SendTo(msg, remoteEP);

            //        break;

            //    case AnimationTypes.Death:

            //        message = $"ANIMATION_TRIGGERED_MESSAGE.KEY:{characterName}:Death:{animOn.ToString()}";

            //        msg = Encoding.ASCII.GetBytes(message);

            //        client.SendTo(msg, remoteEP);

            //        break;

            //    case AnimationTypes.BasicAttack:

            //        message = $"ANIMATION_TRIGGERED_MESSAGE.KEY:{characterName}:Attack:{animOn.ToString()}";

            //        msg = Encoding.ASCII.GetBytes(message);

            //        client.SendTo(msg, remoteEP);

            //        break;

            //    case AnimationTypes.UseAbility:

            //        message = $"ANIMATION_TRIGGERED_MESSAGE.KEY:{characterName}:Ability:{animOn.ToString()}";

            //        msg = Encoding.ASCII.GetBytes(message);

            //        client.SendTo(msg, remoteEP);

            //        break;

            //    case AnimationTypes.Respawn:

            //        message = $"ANIMATION_TRIGGERED_MESSAGE.KEY:{characterName}:Respawn:{animOn.ToString()}";

            //        msg = Encoding.ASCII.GetBytes(message);

            //        client.SendTo(msg, remoteEP);

            //        break;
            //}
        }
    }

    public void SendStartWave()
    {
        if (clientStarted)
        {
            string message = "STARTING_WAVE_MESSAGE.KEY";

            byte[] msg = Encoding.ASCII.GetBytes(message);

            client.SendTo(msg, remoteEP);
        }
    }

    public static void StartUDPClient()
    {
        try
        {
            ip = IPAddress.Parse("127.0.0.1");
            //ip = IPAddress.Parse("54.208.168.94");

            remoteEP = new IPEndPoint(ip, 11111);

            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        catch(Exception e)
        {

        }

        clientStarted = true;
    }
}
