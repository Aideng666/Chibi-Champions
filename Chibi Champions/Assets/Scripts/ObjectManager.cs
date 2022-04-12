using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    TennisBall[] tennisBalls;

    PlayerController[] players;

    public static ObjectManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        tennisBalls = FindObjectsOfType<TennisBall>();

        players = FindObjectsOfType<PlayerController>();
    }


    public TennisBall[] GetTennisBalls()
    {
        return tennisBalls;
    }

    public PlayerController[] GetPlayers()
    {
        return players;
    }
}
