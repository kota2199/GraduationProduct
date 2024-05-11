using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatterySystem : MonoBehaviour
{
    [SerializeField]
    private Text batteryText;

    public float remainBattery, restrictor;


    const float minRestrictor = 1;     // �ŏ��l
    const float maxRestrictor = 3;   // �ő�l

    [SerializeField]
    private int batteryScale;

    [SerializeField]
    private Image[] batteryScales;

    [SerializeField]
    private Sprite batteryScaleImages;


    // Start is called before the first frame update
    void Start()
    {
        remainBattery = 100;
        restrictor = 1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        BaterryLimit();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            restrictor--;
            RestrictorLimit();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            restrictor++;
            RestrictorLimit();
        }
        
        if (remainBattery <= 100)
        {
            batteryScale = 4;
        }
        else if (remainBattery <= 75)
        {
            batteryScale = 3;
        }
        else if (remainBattery <= 50)
        {
            batteryScale = 2;
        }
        else if(remainBattery <= 25)
        {
            batteryScale = 1;
        }
        else if (remainBattery <= 0)
        {
            batteryScale = 0;
        }
    }

    private void UpdateUI()
    {

        batteryText.text = remainBattery.ToString("f0") + "%";
    }

    private void BaterryLimit()
    {
        if (remainBattery <= 0)
        {
            remainBattery = 0;
        }
        if (remainBattery > 100)
        {
            remainBattery = 100;
        }
    }

    private void RestrictorLimit()
    {
        // �ő�l�𒴂�����ő�l��n��
        restrictor = System.Math.Min(restrictor, maxRestrictor);
        // �ŏ��l�����������ŏ��l��n��
        restrictor = System.Math.Max(restrictor, minRestrictor);
    }
}
