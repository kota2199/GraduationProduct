using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private bool onMenu = false;

    [SerializeField]
    private GameObject menuUI;

    [SerializeField]
    private SceneTransition sceneController;

    [SerializeField]
    private ReplaceController replaceController;

    private LapCounter lapCounter;

    // Start is called before the first frame update
    void Awake()
    {
        onMenu = false;
        replaceController = GetComponent<ReplaceController>();
        lapCounter = GetComponent<LapCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire_Select") || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!onMenu)
            {
                onMenu = true;
            }
            else
            {
                onMenu = false;
            }
        }

        if (onMenu || lapCounter.isFinished)
        {
            if (Input.GetButtonDown("Fire_1"))
            {
                //ToMenu();
            }
            if (Input.GetButtonDown("Fire_2"))
            {
                Replace();
            }
            if (Input.GetButtonDown("Fire_3"))
            {
                Retry();
            }
        }

        menuUI.SetActive(onMenu);
    }

    public void ToMenu()
    {
        sceneController.ToHome();
    }
    public void Resume()
    {
        onMenu = false;
    }
    public void Replace()
    {
        replaceController.Replace();
    }
    public void Retry()
    {
        sceneController.Retry();
    }
}
