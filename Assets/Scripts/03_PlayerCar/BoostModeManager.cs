using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostModeManager : MonoBehaviour
{
    private int remainBoostMode, maxBoostCount;

    public float addBoostPower;

    [SerializeField]
    private float length;

    [SerializeField]
    private Text isBoostCountText;

    [SerializeField]
    private Image[] boostIndicators;

    [SerializeField]
    private Sprite availableImage, unavailableImage;

    private bool isBoost;

    //Audio
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip boostSound;

    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        isBoost = false;

        addBoostPower = 1f;
        maxBoostCount = 3;
        remainBoostMode = maxBoostCount;
        length = 10f;

        isBoostCountText.gameObject.SetActive(false);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateUI();
        UpdateBoostCount();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AcceleArea")
        {
            if (remainBoostMode > 0)
            {
                isBoost = true;
                isBoostCountText.gameObject.SetActive(true);
                isBoostCountText.text = length.ToString();

                addBoostPower = 1.5f;
                remainBoostMode--;

                audioSource.Stop();
                audioSource.PlayOneShot(boostSound);

                UpdateUI();

                Invoke("InitializeBoostPower", 10);
            }
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < boostIndicators.Length; i++)
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

    private void UpdateBoostCount()
    {
        if (isBoost)
        {
            length -= Time.deltaTime;
            isBoostCountText.text = length.ToString("f0") + "s";
        }
    }

    private void InitializeBoostPower()
    {
        isBoost = false;
        addBoostPower = 1.0f;
        isBoostCountText.gameObject.SetActive(false);
        length = 10f;
    }
}
