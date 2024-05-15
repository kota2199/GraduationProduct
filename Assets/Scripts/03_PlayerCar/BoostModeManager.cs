using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostModeManager : MonoBehaviour
{
    private int remainBoostMode, maxBoostCount;

    public float addBoostPower;

    [SerializeField]
    private Text remainBoostText;

    [SerializeField]
    private Image[] boostIndicators;

    [SerializeField]
    private Sprite availableImage, unavailableImage;

    // Start is called before the first frame update
    void Start()
    {
        addBoostPower = 1f;
        maxBoostCount = 3;
        remainBoostMode = maxBoostCount;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AcceleArea")
        {
            if (remainBoostMode > 0)
            {
                addBoostPower = 1.5f;
                remainBoostMode--;

                UpdateUI();

                Invoke("InitializeBoostPower", 10);
            }
        }
    }

    private void UpdateUI()
    {
        remainBoostText.text = remainBoostMode.ToString() + "/" + maxBoostCount.ToString();

        for(int i = 0; i < boostIndicators.Length; i++)
        {
            if(remainBoostMode > i)
            {
                boostIndicators[i].sprite = availableImage;
            }
            else
            {
                boostIndicators[i].sprite = unavailableImage;
            }
        }
    }

    private void InitializeBoostPower()
    {
        addBoostPower = 1.0f;
    }
}
