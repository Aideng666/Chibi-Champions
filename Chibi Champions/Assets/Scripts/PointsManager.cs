using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{
    [SerializeField] int startingPoints;

    int currentPoints = 0;

    [SerializeField] TMP_Text pointsText;

    private void Start()
    {
        currentPoints = startingPoints;
        pointsText.text = startingPoints.ToString();
    }

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

        //print("Points: " + currentPoints);
        pointsText.text = currentPoints.ToString();
    }

    public int GetCurrentPoints()
    {
        return currentPoints;
    }
}
