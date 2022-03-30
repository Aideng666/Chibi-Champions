using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    PlayerController[] characters;
    Vector3 previousPosition;
    Vector3[] previousReceivedPositions;
    PlayerController localPlayer;

    Enemy[] enemies;

    Tower[] localTowers;
    Tower[] previousTowers;
    [SerializeField] List<GameObject> platforms = new List<GameObject>();
    [SerializeField] List<GameObject> towers = new List<GameObject>();

    Cure cure;

    float timeToNextSend = 0;
    float sendsPerSecond = 120;
    float interval;

    Vector3[] predictedPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero };
    Vector3[] velocities = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero };

    public static EntityManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        characters = FindObjectsOfType<PlayerController>();

        GetLocalPlayer();

        previousPosition = localPlayer.transform.position;

        previousReceivedPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero };
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<UDPClient>() != null)
        {
            interval = 1 / sendsPerSecond;
            enemies = FindObjectsOfType<Enemy>();
            localTowers = FindObjectsOfType<Tower>();

            if (previousTowers == null)
            {
                previousTowers = localTowers;
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                sendsPerSecond++;
                print($"Raised Sending Interval To {sendsPerSecond} / Second");
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                sendsPerSecond--;
                print($"Lowered Sending Interval To {sendsPerSecond} / Second");
            }

            if (CanSendMessage())
            {
                SendPlayerUpdates();

                SendEnemyUpdates();

                SendTowerUpdates();
            }

            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].transform.position += velocities[i] * Time.deltaTime;
            }

            previousTowers = localTowers;
        }
    }

    public void ReceivePlayerUpdates(int playerIndex, Vector3 position, Vector2 rotation)
    {
        for (int j = 0; j < characters.Length; j++)
        {
            if (PlayerClient.Instance.GetPlayersCharacters()[playerIndex] == characters[j].GetName())
            {
                predictedPositions[j] = Vector3.zero;

                characters[j].transform.position = position;
                characters[j].transform.rotation = Quaternion.Euler(rotation);

                PredictPlayerMovement(j, previousReceivedPositions[j], position);

                previousReceivedPositions[j] = position;
            }
        }
    }

    public void PredictPlayerMovement(int characterIndex, Vector3 previousPos, Vector3 currentPos)
    {
        float speed = Vector3.Distance(previousPos, currentPos) / interval; // normally divided by interval

        if (speed > 2)
        {
            speed = characters[characterIndex].GetSpeed();
        }

        Vector3 direction = (currentPos - previousPos).normalized;

        velocities[characterIndex] = direction * speed;

        predictedPositions[characterIndex] = currentPos + (velocities[characterIndex] * Time.deltaTime);
    }

    void SendPlayerUpdates()
    {
        for (int j = 0; j < PlayerClient.Instance.GetConnectedUsers().Length; j++)
        {
            if (PlayerClient.Instance.GetUsername() == PlayerClient.Instance.GetConnectedUsers()[j])
            {
                UDPClient.Instance.SetPlayerPos(j, localPlayer.transform.position, localPlayer.transform.rotation.eulerAngles);
            }
        }
    }

    void SendEnemyUpdates()
    {

    }

    public void ReceiveEnemyUpdates()
    {

    }

    public void SendTowerUpdates()
    {
        if (previousTowers.Length != localTowers.Length && localTowers.Length != 0)
        {
            for (int i = 0; i < PlayerClient.Instance.GetConnectedUsers().Length; i++)
            {
                if (PlayerClient.Instance.GetUsername() == PlayerClient.Instance.GetConnectedUsers()[i])
                {
                    //for (int j = 0; j < localTowers.Length; j++)
                    //{
                    //    if (localTowers[j] != previousTowers[j])
                    //    {
                    //        UDPClient.Instance.SendTowers(towers[j]);
                    //    }
                    //}

                    print($"Local Towers Length: {localTowers.Length}");

                    UDPClient.Instance.SendTowers(i, localTowers);
                }
            }
        }
    }

    public void ReceiveTowerUpdates(string[] receivedTowers, string[] levels, List<Vector2> positions)
    {
        for (int i = 0; i < receivedTowers.Length; i++)
        {
            switch(receivedTowers[i])
            {
                case "Feather Blaster":

                    Instantiate(towers[0], new Vector3(positions[i].x, towers[0].transform.position.y, positions[i].y), Quaternion.identity);

                    foreach (GameObject plat in platforms)
                    {
                        if (Vector2.Distance(new Vector2(plat.transform.position.x, plat.transform.position.z), positions[i]) < 5)
                        {
                            plat.SetActive(false);
                        }
                    }

                    FindObjectOfType<AudioManager>().Play("Build");

                    break;

                case "Chicken Laser":

                    Instantiate(towers[1], new Vector3(positions[i].x, towers[1].transform.position.y, positions[i].y), Quaternion.identity);

                    foreach (GameObject plat in platforms)
                    {
                        if (Vector2.Distance(new Vector2(plat.transform.position.x, plat.transform.position.z), positions[i]) < 5)
                        {
                            plat.SetActive(false);
                        }
                    }

                    FindObjectOfType<AudioManager>().Play("Build");

                    break;

                case "Gatling Drummet":

                    Instantiate(towers[2], new Vector3(positions[i].x, towers[2].transform.position.y, positions[i].y), Quaternion.identity);

                    foreach (GameObject plat in platforms)
                    {
                        if (Vector2.Distance(new Vector2(plat.transform.position.x, plat.transform.position.z), positions[i]) < 5)
                        {
                            plat.SetActive(false);
                        }
                    }

                    break;

                case "Web Shooter":

                    Instantiate(towers[3], new Vector3(positions[i].x, towers[3].transform.position.y, positions[i].y), Quaternion.identity);

                    foreach (GameObject plat in platforms)
                    {
                        if (Vector2.Distance(new Vector2(plat.transform.position.x, plat.transform.position.z), positions[i]) < 5)
                        {
                            plat.SetActive(false);
                        }
                    }

                    break;

                case "Tennis Bomb":

                    Instantiate(towers[4], new Vector3(positions[i].x, towers[4].transform.position.y, positions[i].y), Quaternion.identity);

                    foreach (GameObject plat in platforms)
                    {
                        if (Vector2.Distance(new Vector2(plat.transform.position.x, plat.transform.position.z), positions[i]) < 5)
                        {
                            plat.SetActive(false);
                        }
                    }

                    break;

                case "Spider House":

                    Instantiate(towers[5], new Vector3(positions[i].x, towers[5].transform.position.y, positions[i].y), Quaternion.identity);

                    foreach (GameObject plat in platforms)
                    {
                        if (Vector2.Distance(new Vector2(plat.transform.position.x, plat.transform.position.z), positions[i]) < 5)
                        {
                            plat.SetActive(false);
                        }
                    }

                    break;

                case "Ink Bomber":

                    Instantiate(towers[6], new Vector3(positions[i].x, towers[6].transform.position.y, positions[i].y), Quaternion.identity);

                    foreach (GameObject plat in platforms)
                    {
                        if (Vector2.Distance(new Vector2(plat.transform.position.x, plat.transform.position.z), positions[i]) < 5)
                        {
                            plat.SetActive(false);
                        }
                    }

                    break;

                case "Photosynthesizer":

                    Instantiate(towers[7], new Vector3(positions[i].x, towers[7].transform.position.y, positions[i].y), Quaternion.identity);

                    foreach (GameObject plat in platforms)
                    {
                        if (Vector2.Distance(new Vector2(plat.transform.position.x, plat.transform.position.z), positions[i]) < 5)
                        {
                            plat.SetActive(false);
                        }
                    }

                    break;

                case "S.A.P":

                    Instantiate(towers[8], new Vector3(positions[i].x, towers[8].transform.position.y, positions[i].y), Quaternion.identity);

                    foreach (GameObject plat in platforms)
                    {
                        if (Vector2.Distance(new Vector2(plat.transform.position.x, plat.transform.position.z), positions[i]) < 5)
                        {
                            plat.SetActive(false);
                        }
                    }

                    break;
            }
        }
        previousTowers = FindObjectsOfType<Tower>();
    }

    void GetLocalPlayer()
    {

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].GetIsPlayerCharacter())
            {
                localPlayer = characters[i];
            }
        }
    }

    bool CanSendMessage()
    {
        if (timeToNextSend <= Time.realtimeSinceStartup)
        {
            timeToNextSend = Time.realtimeSinceStartup + interval;

            return true;
        }

        return false;
    }
}
