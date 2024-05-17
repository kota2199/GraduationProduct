using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCounter : MonoBehaviour
{
    private GameModeManager gameModeManager;

    //Lap
    [SerializeField]
    private GameObject[] checkPoints;

    public int maxCheckPoint, checkedPoint;

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

    [SerializeField]
    private Text finishedBestTimeText;

    // Start is called before the first frame update

    private void Awake()
    {
        gameModeManager = GetComponent<GameModeManager>();

        maxCheckPoint = checkPoints.Length;
        checkedPoint = 0;

        lapCount = 1;

        isCount = false;
        timer = 0.0f;
        selfBestTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        TimeCounter();

        if(gameModeManager.carOwner == GameModeManager.CarOwner.Human)
        {
            UpdateUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CheckPoint")
        {
            checkedPoint++;
            other.gameObject.GetComponent<PositionChecker>().CarPassed(lapCount, this.gameObject);
        }

        if(other.gameObject.tag == "ControlLine" && maxCheckPoint <= checkedPoint)
        {
            //laped
            FastestCheck(timer, lapCount);
            timer = 0.0f;

            if(lapCount >= maxLap)
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
    }

    private void Finished()
    {
        finishedPanel.SetActive(true);
        finishedBestTimeText.text = "Fastest : " + selfBestTime.ToString("f2");
    }
}
