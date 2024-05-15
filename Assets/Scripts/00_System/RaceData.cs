using UnityEngine;
using System.Collections.Generic;

// Car情報を保持するクラス
[System.Serializable]
public class CarInfo
{
    public int Lap;
    public int PassedPoint;
    public int Position;
}

// ScriptableObjectを作成するクラス
[CreateAssetMenu(fileName = "RaceData", menuName = "ScriptableObjects/RaceData", order = 1)]
public class RaceData : ScriptableObject
{
    public List<CarInfo> cars;

    public void ClearData()
    {
        cars.Clear();
    }
}
