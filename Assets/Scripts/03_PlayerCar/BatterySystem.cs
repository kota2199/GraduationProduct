using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatterySystem : MonoBehaviour
{
    [SerializeField]
    private Text batteryText;

    [SerializeField]
    private Image batteryFill;

    private RectTransform batteryFillRect;

    [SerializeField]
    private Color batteryNormalColor, rowBatteryColor;

    public float remainBattery, restrictor;

    const float minRestrictor = 1;     // ?????l
    const float maxRestrictor = 4;   // ?????l

    [SerializeField]
    private GameObject[] manageIndicators;

    private float maxFillSize;

    private bool humanCar = false;


    // Start is called before the first frame update
    void Start()
    {
        remainBattery = 100;
        restrictor = 4;

        if(GetComponent<GameModeManager>().carOwner == GameModeManager.CarOwner.Human)
        {
            humanCar = true;
            batteryFillRect = batteryFill.GetComponent<RectTransform>();
            maxFillSize = batteryFillRect.localScale.x;
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
            UpdateManageIndicator();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            restrictor++;
            RestrictorLimit();
            UpdateManageIndicator();
        }

        if (humanCar)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        batteryText.text = remainBattery.ToString("f0") + "%";
        batteryFillRect.localScale = new Vector3(remainBattery / 100 * maxFillSize, maxFillSize, maxFillSize);

        if(remainBattery > 20)
        {
            batteryText.color = batteryNormalColor;
            batteryFill.color = batteryNormalColor;
        }
        else
        {
            batteryText.color = rowBatteryColor;
            batteryFill.color = rowBatteryColor;
        }
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

    private void UpdateManageIndicator()
    {
        for (int i = 0; i < manageIndicators.Length; i++)
        {

            if(i > restrictor- 1)
            {
                manageIndicators[i].SetActive(false);
            }
            else
            {
                manageIndicators[i].SetActive(true);
            }
        }
    }
}
