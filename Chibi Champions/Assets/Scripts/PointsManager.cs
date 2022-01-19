using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    int currentPoints = 0;

    public void AddPoints(int value)
    {
        currentPoints += value;
    }

    public void SpendPoints(int value)
    {
        currentPoints -= value;
    }

    // Update is called once per frame
    void Update()
    {
        Mathf.Clamp(currentPoints, 0, 100000);

        print("Points: " + currentPoints);
    }

    public int GetCurrentPoints()
    {
        return currentPoints;
    }
}
