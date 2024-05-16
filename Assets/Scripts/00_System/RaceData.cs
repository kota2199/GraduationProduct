using UnityEngine;
using System.Collections.Generic;

// Car情報を保持するクラス
[System.Serializable]
public class CarInfo
{
    public string CarName;
    public int Lap;
    public int Position;
    public int PassedPoint;
    public float time;
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

    public CarInfo GetCarInfo(string carName)
    {
        return cars.Find(car => car.CarName == carName);
    }

    public int GetCarLap(string carName)
    {
        CarInfo car = GetCarInfo(carName);
        return car.Lap;
    }

    public int GetCarPassedPointNumber(string carName)
    {
        CarInfo car = GetCarInfo(carName);
        return car.PassedPoint;
    }

    public void UpdateCarInfo(string carName, int lap, int passedPoint, float time)
    {
        CarInfo car = GetCarInfo(carName);
        if (car != null)
        {
            car.Lap = lap;
            car.PassedPoint = passedPoint;
            car.time = time;
        }
    }

    public void UpdateCarPositionsByTime()
    {
        // CarInfoリストをtimeの小さい順にソート
        List<CarInfo> sortedCars = new List<CarInfo>(cars);
        sortedCars.Sort((carA, carB) => carA.time.CompareTo(carB.time));
        sortedCars.Sort((carA, carB) => carB.PassedPoint.CompareTo(carA.PassedPoint));
        sortedCars.Sort((carA, carB) => carB.Lap.CompareTo(carA.Lap));

        // ソートされたリストの順番に基づいてPositionを設定
        for (int i = 0; i < sortedCars.Count; i++)
        {
            sortedCars[i].Position = i + 1; // Positionは1から始まる
        }

        // ソートされたリストを元のcarsリストに反映
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i] = sortedCars[i];
        }
    }
}
