using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] GameObject gruntPrefab;
    [SerializeField] GameObject sharpshooterPrefab;

    Queue<GameObject> availableGrunts = new Queue<GameObject>();
    Queue<GameObject> availableShooters = new Queue<GameObject>();

    public static EnemyPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        CreatePools();
    }

    public GameObject GetGruntFromPool(Vector3 spawnerPos)
    {
        if (availableGrunts.Count == 0)
        {
            CreatePools();
        }

        var instance = availableGrunts.Dequeue();

        instance.transform.position = spawnerPos;

        instance.SetActive(true);
        return instance;
    }

    public GameObject GetShooterFromPool(Vector3 spawnerPos)
    {
        if (availableShooters.Count == 0)
        {
            CreatePools();
        }

        var instance = availableShooters.Dequeue();

        instance.transform.position = spawnerPos;

        instance.SetActive(true);
        return instance;
    }

    private void CreatePools()
    {
        for (int i = 0; i < 20; ++i)
        {
            var instanceToAdd = Instantiate(gruntPrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToGruntPool(instanceToAdd);
        }

        for (int i = 0; i < 20; ++i)
        {
            var instanceToAdd = Instantiate(sharpshooterPrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToShooterPool(instanceToAdd);
        }
    }

    public void AddToGruntPool(GameObject instance)
    {
        instance.SetActive(false);
        availableGrunts.Enqueue(instance);
    }

    public void AddToShooterPool(GameObject instance)
    {
        instance.SetActive(false);
        availableShooters.Enqueue(instance);
    }
}
