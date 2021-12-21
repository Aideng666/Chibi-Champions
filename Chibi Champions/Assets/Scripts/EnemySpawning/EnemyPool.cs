using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    Queue<GameObject> availableEnemies = new Queue<GameObject>();

    public static EnemyPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        CreatePool();
    }

    public GameObject GetEnemyFromPool()
    {
        if (availableEnemies.Count == 0)
        {
            CreatePool();
        }

        var instance = availableEnemies.Dequeue();
        instance.SetActive(true);
        return instance;
    }

    private void CreatePool()
    {
        for (int i = 0; i < 20; ++i)
        {
            var instanceToAdd = Instantiate(enemyPrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableEnemies.Enqueue(instance);
    }
}
