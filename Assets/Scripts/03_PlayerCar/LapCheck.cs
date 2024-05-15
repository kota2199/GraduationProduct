using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCheck : MonoBehaviour
{
    private int passedCheckNumber;
    [SerializeField]
    private int maxCheckNumber;

    public int lapNumber;

    public float bestTime;

    public Text lapText;

    private bool firstLapIgnore;

    public GameObject finishUI;
    public GameObject timeText;
    public GameObject recordText;

    public Text[] bestTimeText;

    public Text rewardText;

    private int reward;

    // Start is called before the first frame update
    void Start()
    {
        passedCheckNumber = 1;
        firstLapIgnore = false;
        lapNumber = 1;
        lapText.text = "Lap:1/3";
        bestTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int minute = (int)bestTime / 60;//分.timeを60で割った値.
        float second = bestTime % 60;//秒.timeを60で割った余り.
        string minText, secText;//テキスト形式の分・秒を用意.

        if (minute < 10)
            minText = "0" + minute.ToString();//("0"埋め), ToStringでint→stringに変換.
        else
            minText = minute.ToString();

        if (second < 10)
            secText = "0" + second.ToString("f2");//上に同じく.
        else
            secText = second.ToString("f2");

        bestTimeText[0].text ="Best:" + minText + ":" + secText;
        bestTimeText[1].text = "Best:" + minText + ":" + secText;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint")
        {
            if (other.GetComponent<LapCheckNumber>().checkNumber == passedCheckNumber)
            {
                firstLapIgnore = true;
                passedCheckNumber++;
            }
        }

        if (other.gameObject.tag == "Checker")
        {
            if (firstLapIgnore)
            {
                if (passedCheckNumber == maxCheckNumber)
                {
                    if (lapNumber == 1)
                    {
                        bestTime = GetComponent<TimeCount>().time;
                    }
                    else if (GetComponent<TimeCount>().time < bestTime)
                    {
                        bestTime = GetComponent<TimeCount>().time;
                    }
                    lapNumber++;
                    lapText.text = "Lap:" + lapNumber + "/3";
                    passedCheckNumber = 1;
                    GetComponent<TimeCount>().time = 0;

                    if (lapNumber >= 4)
                    {
                        lapText.text = "Finish";
                        timeText.SetActive(false);
                        finishUI.SetActive(true);
                        SetReward();
                        if (bestTime < GameObject.Find("GameMaster").GetComponent<DateStore>().stage1RecordTime || GameObject.Find("GameMaster").GetComponent<DateStore>().stage1RecordTime == 0)
                        {
                            recordText.SetActive(true);
                            GameObject.Find("GameMaster").GetComponent<DateStore>().stage1RecordTime = bestTime;
                            GameObject.Find("GameMaster").GetComponent<DateStore>().Stage1Record();
                        }
                    }
                }
            }
        }
    }

    void SetReward()
    {
        if (bestTime < 20f)
        {
            reward = 10;
            rewardText.text = "Points : " + reward.ToString();
            GameObject.Find("GameMaster").GetComponent<DateStore>().getReward = reward;
            GameObject.Find("GameMaster").GetComponent<DateStore>().SaveRewardTA();
        }
        if (bestTime >= 20f)
        {
            reward = 10;
            rewardText.text = "Points : " + reward.ToString();
            GameObject.Find("GameMaster").GetComponent<DateStore>().getReward = reward;
            GameObject.Find("GameMaster").GetComponent<DateStore>().SaveRewardTA();
        }
    }
}
