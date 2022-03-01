using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] GameObject featherPrefab;
    [SerializeField] GameObject paintballPrefab;

    Queue<GameObject> availableFeathers = new Queue<GameObject>();
    Queue<GameObject> availablePaintballs = new Queue<GameObject>();

    public static ProjectilePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        CreatePools();
    }

    public GameObject GetFeatherFromPool(Vector3 pos)
    {
        if (availableFeathers.Count == 0)
        {
            CreatePools();
        }

        var instance = availableFeathers.Dequeue();
        instance.SetActive(true);
        instance.transform.position = pos;
        instance.GetComponentInChildren<Feather>().gameObject.transform.position = instance.transform.position;
        instance.GetComponentInChildren<Feather>().StartDelay();
        return instance;
    }

    public GameObject GetPaintballFromPool(Vector3 pos)
    {
        if (availablePaintballs.Count == 0)
        {
            CreatePools();
        }

        var instance = availablePaintballs.Dequeue();
        instance.SetActive(true);
        instance.transform.position = pos;
        instance.GetComponentInChildren<Paintball>().gameObject.transform.position = instance.transform.position;
        instance.GetComponentInChildren<Paintball>().StartDelay();
        return instance;
    }

    private void CreatePools()
    {
        for (int i = 0; i < 50; ++i)
        {
            var instanceToAdd = Instantiate(featherPrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToFeatherPool(instanceToAdd);
        }

        for (int i = 0; i < 20; ++i)
        {
            var instanceToAdd = Instantiate(paintballPrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPaintballPool(instanceToAdd);
        }
    }

    public void AddToFeatherPool(GameObject instance)
    {
        instance.SetActive(false);
        availableFeathers.Enqueue(instance);
    }

    public void AddToPaintballPool(GameObject instance)
    {
        instance.SetActive(false);
        availablePaintballs.Enqueue(instance);
    }
}
