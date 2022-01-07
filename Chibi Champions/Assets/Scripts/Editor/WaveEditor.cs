//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;

//[CustomEditor(typeof(WaveManager))]
//public class WaveEditor : Editor
//{
//    int[] gruntCount;
//    int[] sharpshooterCount;

//    bool countsSet = false;

//    //List<List<GameObject>> enemiesLists = new List<List<GameObject>>();

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        WaveManager manager = (WaveManager)target;

//        if (!countsSet)
//        {
//            gruntCount = new int[manager.GetWaveCount()];
//            sharpshooterCount = new int[manager.GetWaveCount()];

//            countsSet = true;
//        }

//        GUILayout.Space(10);

//        GUILayout.BeginVertical();

//        for (int i = 0; i < manager.GetWaveCount(); i++)
//        {
//            if (manager.GetEnemiesLists().Count < manager.GetWaveCount())
//            {
//                manager.GetEnemiesLists().Add(new List<GameObject>());
//            }

//            GUILayout.Label(" Set the enemies for wave " + (i + 1) + ": ");

//            GUILayout.Space(10);

//            GUILayout.Label("Number of Grunts: " + gruntCount[i]);
//            gruntCount[i] = (int)GUILayout.HorizontalSlider(gruntCount[i], 0, 10);

//            GUILayout.Space(10);

//            GUILayout.Label("Number of Sharpshooters: " + sharpshooterCount[i]);
//            sharpshooterCount[i] = (int)GUILayout.HorizontalSlider(sharpshooterCount[i], 0, 10);

//            GUILayout.Space(25);

//            SetEnemiesInWave(i + 1, gruntCount[i], sharpshooterCount[i], manager);
//        }

//        GUILayout.EndVertical();
//    }

//    void SetEnemiesInWave(int waveNum, int gruntAmount, int sharpshooterAmount, WaveManager manager)
//    {
//        manager.GetEnemiesLists()[waveNum - 1].Clear();

//        for (int i = 0; i < gruntAmount; i++)
//        {
//            manager.GetEnemiesLists()[waveNum - 1].Add(manager.GetGruntPrefab());
//        }

//        for (int i = 0; i < sharpshooterAmount; i++)
//        {
//            manager.GetEnemiesLists()[waveNum - 1].Add(manager.GetSharpshooterPrefab());
//        }

//        //Debug.Log("There are " + manager.GetEnemiesLists()[waveNum - 1].Count + " Enemies in wave " + waveNum);
//    }

//    public List<List<GameObject>> SetEnemiesInWave()
//    {
        
//    }
//}
