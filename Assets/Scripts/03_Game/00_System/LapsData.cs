using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class CarData
{
    public string carName;
    public float time;
}

[Serializable]
public class LapTimeData
{
    public List<CarData> position;
}

[CreateAssetMenu(fileName = "NewLapsData", menuName = "ScriptableObjects/LapsData", order = 1)]
public class LapsData : ScriptableObject
{
    public List<LapTimeData> laps;

    public void ClearData()
    {
        laps.Clear();
    }
}