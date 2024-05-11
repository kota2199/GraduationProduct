using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource aS;
    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayHomeBGM()
    {

    }
    public void PlayBGM2()
    {

    }
    public void PositiveButton()
    {

    }
    public void NegativeButton()
    {

    }
    public void StopBGM()
    {
        aS.Stop();
    }
}
