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
    Vector3 originalPosition;
    bool newPositionPredicted;
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
                //characters[i].GetComponent<CharacterController>().Move(velocities[i] * Time.deltaTime);

                characters[i].transform.position += velocities[i] * Time.deltaTime;
            }

            previousTowers = localTowers;
        }
    }

    public void ReceivePlayerUpdates(int playerIndex, Vector3 position, Vector2 rotation)
    {
        print("Received Update");

        for (int j = 0; j < characters.Length; j++)
        {
            if (PlayerClient.Instance.GetPlayersCharacters()[playerIndex] == characters[j].GetName())
            {
                predictedPositions[j] = Vector3.zero;
                //StopCoroutine(MoveToPredictedPosition(j));
                print($"Received A Move Update For {characters[j].GetName()}");
                characters[j].transform.position = position;
                characters[j].transform.rotation = Quaternion.Euler(rotation);

                //originalPosition = previousReceivedPositions[playerIndex];
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

        //print(predictedPositions[characterIndex]);

        newPositionPredicted = true;

        //StartCoroutine(MoveToPredictedPosition(characterIndex));
    }

    IEnumerator MoveToPredictedPosition(int characterIndex)
    {
        print("Starting Move For Player " + characterIndex);

        bool positionReached = false;
        float totalTime = interval;
        float t = 0;

        while(!positionReached)
        {
            print("Moving");
            characters[characterIndex].transform.position = Vector3.Lerp(originalPosition, predictedPositions[characterIndex], t);

            t += Time.deltaTime;

            if (t >= totalTime)
            {
                positionReached = true;
            }

            yield return null;
        }

        print("Finished Move");

        //PredictPlayerMovement(characterIndex, originalPosition, predictedPosition);

        yield return null;
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

                    UDPClient.Instance.SendTowers(localTowers);
                }
            }
        }
    }

    public void ReceiveTowerUpdates()
    {

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

        print($"Local Character: {localPlayer.GetName()}");
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
