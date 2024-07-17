using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private string sceneName;

    private FadeInOut fadeController;

    void Awake()
    {
        fadeController = GetComponent<FadeInOut>();
    }

    public void ToHome()
    {
        sceneName = "Home";
        fadeController.FadeOut();
        Invoke("SceneTrans",2);
    }
    public void ToRace()
    {
        sceneName = "TestCurcuit";
        fadeController.FadeOut();
        Invoke("SceneTrans", 2);
    }
    public void Retry()
    {
        sceneName = SceneManager.GetActiveScene().name;
        fadeController.FadeOut();
        Invoke("SceneTrans", 2);
    }

    public void SceneTrans()
    {
        SceneManager.LoadScene(sceneName);
    }
}
