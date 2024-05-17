using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGapManager : MonoBehaviour
{
    [SerializeField]
    private Transform checkPointParent;

    [SerializeField]
    private RaceData raceData;

    [SerializeField]
    private List<GameObject> positionCheckPoints = new List<GameObject>();

    [SerializeField]
    private GameObject[] cars;

    private float gapTimer;


    // Start is called before the first frame update
    void Start()
    {
        raceData.ClearData();

        cars = GameObject.FindGameObjectsWithTag("Car");

        for(int i = 0; i < cars.Length; i++)
        {
            CarInfo car = new CarInfo();
            car.CarName = cars[i].name;
            car.Lap = 1;
            car.PassedPoint = 1;
            raceData.cars.Add(car);
        }

        PositionChecker[] positionCheckers = checkPointParent.GetComponentsInChildren<PositionChecker>();
        foreach (PositionChecker checker in positionCheckers)
        {
            positionCheckPoints.Add(checker.gameObject);
        }
    }

    private void Update()
    {
        gapTimer += Time.deltaTime;
    }

    public void PassPoint(GameObject hitCar, int pointNumber, string pointTag, int lap)
    {
        if(pointTag == "CheckPoint")
        {
            for (int i = 0; i < cars.Length; i++)
            {
                if(cars[i].name == hitCar.name
                    && lap == raceData.GetCarLap(hitCar.name)
                    && pointNumber == raceData.GetCarPassedPointNumber(hitCar.name))
                {
                    raceData.UpdateCarInfo(hitCar.name, lap, pointNumber + 1, gapTimer);
                    raceData.UpdateCarPositionsByTime();
                }
            }
        }
        else if(pointTag == "ControlLine")
        {
            for (int i = 0; i < cars.Length; i++)
            {
                if (cars[i].name == hitCar.name
                    && lap != 1)
                {
                    hitCar.GetComponent<LapCounter>().checkedPoint = 0;
                    raceData.UpdateCarInfo(hitCar.name, lap, 1, gapTimer);
                    raceData.UpdateCarPositionsByTime();
                }
            }
        }
        else
        {
            Debug.Log("TagName is Different.");
        }
    }
}
