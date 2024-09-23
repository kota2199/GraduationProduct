using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectController : MonoBehaviour
{
    private MenuChoiceController menuController;

    public bool isColorSelecting;

    [SerializeField]
    private Image[] colors;

    [SerializeField]
    private Image colorMarker;

    [SerializeField]
    private int numberOfColors;

    [SerializeField]
    private GameObject demoBody, demoWing, demoTyre1, demoTyre2, demoTyre3, demoTyre4;

    [SerializeField]
    private bool selectRight = false, selectLeft = false;

    // Start is called before the first frame update
    private void Awake()
    {
        menuController = GetComponent<MenuChoiceController>();
        isColorSelecting = false;
        numberOfColors = 0;
    }
    void Start()
    {
        UpdateUI_Color();
        isColorSelecting = true;
    }

    // Update is called once per frame
    void Update()
    {
        ReceiveInput();
        if (isColorSelecting)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RecieveRightInput();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RecieveLeftInput();
            }
        }
    }

    private void ReceiveInput()
    {
        if (Input.GetAxis("Horizontal Stick-L") > 0.15f && !selectRight)
        {
            selectRight = true;
            RecieveRightInput();
        }
        else if (Input.GetAxis("Horizontal Stick-L") < 0.15f)
        {
            selectRight = false;
        }

        if (Input.GetAxis("Horizontal Stick-L") < -0.15f && !selectLeft)
        {
            selectLeft = true;
            RecieveLeftInput();
        }
        else if (Input.GetAxis("Horizontal Stick-L") > -0.15f)
        {
            selectLeft = false;
        }
    }

    private void RecieveRightInput()
    {
        if (numberOfColors == colors.Length - 1)
        {
            numberOfColors = 0;
        }
        else
        {
            numberOfColors++;
        }

        UpdateUI_Color();
    }
    private void RecieveLeftInput()
    {
        if (numberOfColors == 0)
        {
            numberOfColors = colors.Length - 1;
        }
        else
        {
            numberOfColors--;
        }

        UpdateUI_Color();
    }

    private void UpdateUI_Color()
    {
        colorMarker.transform.SetParent(colors[numberOfColors].gameObject.transform);
        colorMarker.rectTransform.localPosition = Vector3.zero;
        colorMarker.rectTransform.anchoredPosition = Vector2.zero;

        //carColorMaterial.color = FromHex(colors[numberOfColors].gameObject.name);
        //demoBody.
    }

    public static Color FromHex(string hex)
    {
        hex = hex.Replace("#", "");
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}
