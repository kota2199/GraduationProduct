using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateStore : MonoBehaviour
{
    private int numberOfBoots;
    public int points;
    public int playerLevel;
    public int experiencePoint;
    public float stage1RecordTime;
    public int getReward;

    // Start is called before the first frame update
    void Start()
    {
        numberOfBoots = PlayerPrefs.GetInt("Boots");
        if (numberOfBoots <= 0)
        {
            numberOfBoots++;
            PlayerPrefs.SetInt("Boots", numberOfBoots);
        }

        points = PlayerPrefs.GetInt("MyPoints");
        playerLevel = PlayerPrefs.GetInt("MyLevel");
        experiencePoint = PlayerPrefs.GetInt("ExperiencePoint");
        stage1RecordTime = PlayerPrefs.GetFloat("Stage1RecordTime");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetPoint()
    {
        PlayerPrefs.SetInt("MyPoint", points);
        PlayerPrefs.Save();
    }
    public void LevelUp()
    {
        playerLevel++;
        PlayerPrefs.SetInt("MyLevel",playerLevel);
        PlayerPrefs.Save();
    }
    public void AllDelete()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
    public void Stage1Record()
    {
        PlayerPrefs.SetFloat("Stage1RecordTime", stage1RecordTime);
        PlayerPrefs.Save();
    }
    public void SaveRewardTA()
    {
        points += getReward;
        PlayerPrefs.SetInt("MyPoints",points);
        PlayerPrefs.Save();
    }
    public void SaveRewardOnlineFirst()
    {
        points += 200;
        PlayerPrefs.SetInt("MyPoints", points);
        PlayerPrefs.Save();
    }
    public void SaveRewardOnlineSecond()
    {
        points += 100;
        PlayerPrefs.SetInt("MyPoints", points);
        PlayerPrefs.Save();
    }
}
