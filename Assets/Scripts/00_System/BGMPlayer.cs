using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{

    [SerializeField]
    private AudioData audioData;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.clip = audioData.bgms[1].bgmClip;
        audioSource.Play();
    }
}
