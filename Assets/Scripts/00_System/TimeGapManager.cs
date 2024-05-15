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


    // Start is called before the first frame update
    void Start()
    {
        raceData.ClearData();

        cars = GameObject.FindGameObjectsWithTag("Car");

        PositionChecker[] positionCheckers = checkPointParent.GetComponentsInChildren<PositionChecker>();
        foreach(PositionChecker checker in positionCheckers)
        {
            positionCheckPoints.Add(checker.gameObject);
        }

        for(int i = 0; i < cars.Length; i++)
        {
            //InitializeData
            CarInfo newCar = new CarInfo();
            newCar.Lap = 0;
            newCar.PassedPoint = 0;
            newCar.Position = raceData.cars.Count + 1;

            // ScriptableObjectのリストに追加
            raceData.cars.Add(newCar);
        }
    }

    public void PassPoint(int passedPoint, GameObject hitCar)
    {
        int lap = hitCar.GetComponent<LapCounter>().lapCount;
        Debug.Log("CarName : " + hitCar.gameObject.name + " Lap : " + lap.ToString() + " Passed Point : " + passedPoint);

        //hitCarとcarsを照会。for
        for(int i = 0; i < cars.Length; i++)
        {
            if(cars[i].name == hitCar.name)
            {
                if (raceData.cars[i].PassedPoint + 1 == passedPoint)
                {

                }
                else
                if (raceData.cars[i].Lap + 1 == lap && passedPoint == 0)
                {

                }
                break;
            }
        }

        //該当carのデータのPassed + 1 == passedPoint
        //該当carのデータのLap + 1 == lap || passedPoint == 0
    }
}
