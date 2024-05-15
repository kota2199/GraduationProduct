using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class LapData
{
    public string carName;
    public float time;
}

[Serializable]
public class LapTimeData
{
    public LapData[] position;
}

[CreateAssetMenu(fileName = "NewLapsData", menuName = "ScriptableObjects/LapsData", order = 1)]
public class LapsData : ScriptableObject
{
    public List<LapTimeData> laps;

    public void Claar()
    {
        laps.Clear();
    }
}