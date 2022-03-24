using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    PlayerController[] characters;
    Vector3 previousPosition;
    PlayerController localPlayer;

    Enemy[] enemies;

    Cure cure;

    public static EntityManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        characters = FindObjectsOfType<PlayerController>();
        enemies = FindObjectsOfType<Enemy>();

        GetLocalPlayer();

        previousPosition = localPlayer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLocalPlayer();

        UpdateEnemies();
    }

    public void UpdateRemotePlayers(int playerIndex, Vector3 position)
    {

        for (int j = 0; j < characters.Length; j++)
        {
            if (PlayerClient.Instance.GetPlayersCharacters()[playerIndex] == characters[j].GetName())
            {
                print($"Character That Moved: {characters[j].GetName()}");
                characters[j].transform.position = position;
            }
        }
    }

    void UpdateLocalPlayer()
    {
        if (previousPosition != localPlayer.transform.position)
        {
            for (int j = 0; j < PlayerClient.Instance.GetConnectedUsers().Length; j++)
            {
                if (PlayerClient.Instance.GetUsername() == PlayerClient.Instance.GetConnectedUsers()[j])
                {
                    UDPClient.Instance.SetPlayerPos(j, localPlayer.transform.position);
                }
            }
        }

        previousPosition = localPlayer.transform.position;
    }

    void UpdateEnemies()
    {
        enemies = FindObjectsOfType<Enemy>();
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
}
