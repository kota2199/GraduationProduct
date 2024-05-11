using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToHome()
    {
        sceneName = "Home";
        Invoke("SceneTrans",2);
    }
    public void ToTutorial()
    {

    }
    public void ToOnlineRace()
    {
        sceneName = "PreStage_Online";
        Invoke("SceneTrans", 2);
    }
    public void ToTA()
    {
        sceneName = "PreStage_TA";
        Invoke("SceneTrans", 2);
    }
    public void SceneTrans()
    {
        SceneManager.LoadScene(sceneName);
    }
}
