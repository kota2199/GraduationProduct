using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    [SerializeField]
    private AudioClip carSound;

    private AudioSource audioSource;

    [SerializeField, Range(0.5f, 3.0f)]
    private float pitch;

    private SpeedCheck speedCheck;

    private float speed;

    // Start is called before the first frame update
    void Awake()
    {
        speedCheck = transform.parent.gameObject.GetComponent<SpeedCheck>();

        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = speedCheck.speed;

        pitch = speed / 100.0f;

        audioSource.pitch = pitch;
    }
}
