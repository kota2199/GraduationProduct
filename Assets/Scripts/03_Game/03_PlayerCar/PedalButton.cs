using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedalButton : MonoBehaviour
{
    private bool acceleFlag;
    private bool brakeFlag;

    public float acceleAmout;
    public int brakeAmount;

    public Slider acceleSlider;


    // Start is called before the first frame update
    void Start()
    {
        acceleAmout = 0;
        brakeAmount = 0;
        acceleFlag = false;
        brakeFlag = false;
        acceleSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (acceleFlag  == true && brakeFlag == false)
        {
            acceleAmout = 1 * acceleSlider.value;
            GetComponent<BatterySystem>().remainBattery -= 0.8f * Time.deltaTime * acceleSlider.value;
        }
        else if (acceleFlag == false && brakeFlag == false)
        {
            acceleAmout = 0;
        }
        
        if (acceleFlag == false && brakeFlag == true)
        {
            brakeAmount = 1;
            if (GetComponent<SpeedCheck>().speed > 0)
            {
                if (GetComponent<SpeedCheck>().speed >= 10)
                {
                    GetComponent<BatterySystem>().remainBattery += 0.1f;
                }
            }
        }
        else if(acceleFlag == false && brakeFlag == false)
        {
            brakeAmount = 0;
        }
    }
    public void PushAccel()
    {
        acceleFlag = true;
    }
    public void ReleaseAccele()
    {
        acceleFlag = false;
    }
    public void PushBrake()
    {
        brakeFlag = true;
    }
    public void ReleaseBrake()
    {
        brakeFlag = false;
    }
}
