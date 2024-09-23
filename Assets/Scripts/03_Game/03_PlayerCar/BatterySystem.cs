using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatterySystem : MonoBehaviour
{
    private CarSystem carSystem;

    [SerializeField]
    private Text batteryText;

    public float remainBattery, restrictor;

    const float minRestrictor = 1;     // ?????l
    const float maxRestrictor = 4;   // ?????l

    [SerializeField]
    private Image[] manageIndicators;

    [SerializeField]
    private Sprite available, unavailable;


    //needle
    [SerializeField]
    private Image needle;

    [SerializeField]
    private Vector2 needleRotation;

    [SerializeField]
    private float maxSpeedOfMeter;

    [SerializeField]
    private float maxNeedleAngle;

    private float needleAngle;

    private RectTransform needleRect;

    [SerializeField]
    private string restrictorUp, restrictorDown;

    //Audio
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip upSound, downSound;

    //Human or AI
    private bool humanCar = false;

    void Start()
    {
        carSystem = GetComponent<CarSystem>();

        remainBattery = 100;
        restrictor = 4;

        if(GetComponent<GameModeManager>().carOwner == GameModeManager.CarOwner.Human)
        {
            humanCar = true;
            HumanInitialize();
        }
        else
        {
            humanCar = false;
        }
    }

    private void HumanInitialize()
    {
        needleRect = needle.gameObject.GetComponent<RectTransform>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        BaterryLimit();

        if (humanCar)
        {
            if (Input.GetButtonDown(restrictorDown) || Input.GetKeyDown(KeyCode.Q))
            {
                ChangeRestrictor(-1);
            }
            if (Input.GetButtonDown(restrictorUp) || Input.GetKeyDown(KeyCode.E))
            {
                ChangeRestrictor(1);
            }

            UpdateUI();
        }
    }

    private void ChangeRestrictor(int amount)
    {
        restrictor += amount;
        RestrictorLimit();
        UpdateManageIndicator();

        audioSource.Stop();
        if(amount > 0)
        {
            audioSource.PlayOneShot(upSound);
        }
        else
        {
            audioSource.PlayOneShot(downSound);
        }
    }

    private void UpdateUI()
    {
        //text
        batteryText.text = remainBattery.ToString("f0");

        //needle's angle
        needleAngle = -1 * remainBattery * 1.8f + maxNeedleAngle;
        needleRect.rotation = Quaternion.Euler(0.0f, 0.0f, needleAngle);
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
                manageIndicators[i].sprite = unavailable;
            }
            else
            {
                manageIndicators[i].sprite = available;
            }
        }
    }
}
