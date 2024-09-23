using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuChoiceController : MonoBehaviour
{
    /// <summary>
    /// Car Choice
    /// </summary>
    public bool isSelecting;

    private bool isExplaining;

    private bool isColorSelecting;

    public enum MenuMode
    {
        CarChoice,
        CarExplain,
        ChoiceCarColor
    }

    private MenuMode menuMode;

    [SerializeField]
    private GameObject choiceUI;

    [SerializeField]
    private GameObject[] choices;

    [SerializeField]
    private Image choiceMarker;

    [SerializeField]
    private Image explain;

    [SerializeField]
    private Sprite[] explainImage;

    [SerializeField]
    private int currentChoiceNum;

    /// <summary>
    /// Car's color choice
    /// </summary>
    [SerializeField]
    private GameObject colorChoiceUI;

    [SerializeField]
    private GameObject colorModelCar;

    private ColorSelectController selecter;

    [SerializeField]
    private string decisionButton, detailButton, cancelButton;

    [SerializeField]
    private bool selectRight = false, selectLeft = false;

    void Awake()
    {
        selecter = GetComponent<ColorSelectController>();
        isSelecting = false;
        isExplaining = false;
        isColorSelecting = false;
        menuMode = MenuMode.CarChoice;
        colorModelCar.SetActive(false);
        currentChoiceNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        UpdateUI();
        RecieveInput();
    }
    private void RecieveInput()
    {
        if (Input.GetAxis("Horizontal Stick-L") > 0.15f && !selectRight)
        {
            selectRight = true;
            RecieveRightInput();
        }
        else if(Input.GetAxis("Horizontal Stick-L") < 0.15f)
        {
            selectRight = false;
        }

        if (Input.GetAxis("Horizontal Stick-L") < -0.15f && !selectLeft)
        {
            selectLeft = true;
            RecieveLeftInput();
        }
        else if (Input.GetAxis("Horizontal Stick-L") > -0.15f && !selectLeft)
        {
            selectLeft = false;
        }
    }

    private void UpdateInput()
    {
        if (!isSelecting && Input.GetButtonDown(decisionButton))
        {
            UpdateMenuMode(MenuMode.CarChoice);
        }

        else if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown(decisionButton))
        {
            if (isSelecting)
            {
                UpdateMenuMode(MenuMode.ChoiceCarColor);
            }
            else if (isColorSelecting)
            {
                ToRaceScene();
            }
        }

        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown(detailButton))
        {
            if (isSelecting)
            {
                if (!isExplaining)
                {
                    UpdateMenuMode(MenuMode.CarExplain);
                }
                else
                {
                    UpdateMenuMode(MenuMode.CarChoice);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown(cancelButton))
        {
            Debug.Log("Cancel");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (isSelecting && currentChoiceNum > 0)
            {
                RecieveLeftInput();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isSelecting && currentChoiceNum < choices.Length - 1)
            {
                RecieveRightInput();
            }
        }
    }

    private void RecieveRightInput()
    {
        currentChoiceNum++;
    }

    private void RecieveLeftInput()
    {
        currentChoiceNum--;
    }

    private void UpdateUI()
    {
        choiceUI.SetActive(isSelecting);
        choiceMarker.rectTransform.position = choices[currentChoiceNum].transform.position;
        explain.gameObject.SetActive(isExplaining);
        explain.sprite = explainImage[currentChoiceNum];

        colorChoiceUI.SetActive(isColorSelecting);
        colorModelCar.SetActive(isColorSelecting);
    }

    private void UpdateMenuMode(MenuMode nextMenuMode)
    {
        menuMode = nextMenuMode;

        switch (menuMode)
        {
            case MenuMode.CarChoice:

                isSelecting = true;
                isExplaining = false;
                isColorSelecting = false;
                selecter.isColorSelecting = isColorSelecting;

                break;

            case MenuMode.CarExplain:

                isSelecting = true;
                isExplaining = true;
                isColorSelecting = false;
                selecter.isColorSelecting = isColorSelecting;

                break;

            case MenuMode.ChoiceCarColor:

                isSelecting = false;
                isExplaining = false;
                isColorSelecting = true;
                selecter.isColorSelecting = isColorSelecting;

                break;

            default:
                break;
        }
    }

    public void ToRaceScene()
    {
        SceneManager.LoadScene("03_TestCircuit");
    }
}
