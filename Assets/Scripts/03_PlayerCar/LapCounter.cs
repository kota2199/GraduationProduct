using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCounter : MonoBehaviour
{
    private GameModeManager gameModeManager;

    //Lap
    [SerializeField]
    private GameObject checkPointsParent;

    [SerializeField]
    private Transform[] checkPoints;

    public int maxCheckPoint, checkedNum;

    [SerializeField]
    private int maxLap;

    public int lapCount;

    [SerializeField]
    private Text lapText;

    //Time
    public bool isCount;

    private float timer, selfBestTime, totalTime;

    [SerializeField]
    private Text timerText, selfBestTimeText;

    //Finished
    [SerializeField]
    private GameObject finishedPanel;

    public bool isFinished;

    [SerializeField]
    private Text finishedBestTimeText;

    //ForReplace
    private ReplaceController replacer;

    //Human or AI
    private bool humanCar = false;

    // Start is called before the first frame update

    private void Awake()
    {
        gameModeManager = GetComponent<GameModeManager>();

        if (gameModeManager.carOwner == GameModeManager.CarOwner.Human)
        {
            humanCar = true;
        }
        else
        {
            humanCar = false;
        }

        if (humanCar)
        {
            replacer = GetComponent<ReplaceController>();
        }

        checkPoints = new Transform[checkPointsParent.transform.childCount];

        for (var i = 0; i < checkPoints.Length; ++i)
        {
            checkPoints[i] = checkPointsParent.transform.GetChild(i);
        }

        maxCheckPoint = checkPoints.Length;
        checkedNum = 0;

        lapCount = 1;

        isCount = false;
        timer = 0.0f;
        selfBestTime = 0.0f;

        isFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        TimeCounter();

        if(humanCar)
        {
            UpdateUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CheckPoint")
        {
            Debug.Log(other.gameObject.name);
            Debug.Log("num" + checkedNum);
            if (other.gameObject.name == checkPoints[checkedNum].name)
            {
                checkedNum++;
                other.gameObject.GetComponent<PositionChecker>().CarPassed(lapCount, this.gameObject);

                //SetReplacePoint
                if (humanCar)
                {
                    replacer.SetReplacePoint(other.gameObject.transform);
                }
            }
        }

        if(other.gameObject.tag == "ControlLine" && maxCheckPoint <= checkedNum)
        {
            //laped
            FastestCheck(timer, lapCount);
            timer = 0.0f;

            if(lapCount >= maxLap && humanCar)
            {
                Finished();
            }
            else
            {
                lapCount++;
            }

            other.gameObject.GetComponent<PositionChecker>().CarPassed(lapCount, this.gameObject);
        }
    }

    private void TimeCounter()
    {
        isCount = CountDown.instance.isPlay;
        if (isCount)
        {
            timer += Time.deltaTime;
            totalTime += Time.deltaTime;
        }
    }

    private void FastestCheck(float lapTime, int lap)
    {
        if(lap == 1 || selfBestTime > lapTime)
        {
            selfBestTime = lapTime;
        }
    }

    private void UpdateUI()
    {
        timerText.text = "Time : " + timer.ToString("f2");
        lapText.text = lapCount.ToString() + "/" + maxLap.ToString();
        selfBestTimeText.text = "Fastest : " + selfBestTime.ToString("f2");
        finishedPanel.SetActive(isFinished);
    }

    private void Finished()
    {
        isFinished = true;
        finishedBestTimeText.text = "Fastest : " + selfBestTime.ToString("f2");
    }
}
