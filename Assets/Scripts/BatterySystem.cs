using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatterySystem : MonoBehaviour
{
    [SerializeField]
    private Text batteryText;

    [SerializeField]
    private RectTransform batteryFill;

    public float remainBattery, restrictor;

    const float minRestrictor = 1;     // ?????l
    const float maxRestrictor = 3;   // ?????l

    private float maxFillSize;

    private bool humanCar = false;


    // Start is called before the first frame update
    void Start()
    {
        remainBattery = 100;
        restrictor = 1;

        if(GetComponent<GameModeManager>().carOwner == GameModeManager.CarOwner.Human)
        {
            humanCar = true;
            batteryFill = batteryFill.GetComponent<RectTransform>();
            maxFillSize = batteryFill.localScale.x;
        }
        else
        {
            humanCar = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        if (humanCar)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        batteryText.text = remainBattery.ToString("f0") + "%";
        batteryFill.localScale = new Vector3(remainBattery / 100 * maxFillSize, maxFillSize, maxFillSize);
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
        // ?????l???????????????l???n??
        restrictor = System.Math.Min(restrictor, maxRestrictor);
        // ?????l?????????????????l???n??
        restrictor = System.Math.Max(restrictor, minRestrictor);
    }
}
