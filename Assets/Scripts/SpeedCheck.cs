using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedCheck : MonoBehaviour
{
    private Rigidbody rigid;

    public float speed;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private Image needle;

    [SerializeField]
    private Vector2 needleRotation;

    [SerializeField]
    private float maxSpeedOfMeter;

    [SerializeField]
    private float maxNeedleAngle;

    private float needleAngle;

    private RectTransform needleRect;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        needleRect = needle.gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        speed = rigid.velocity.magnitude * 2;
        speedText.text = speed.ToString("f0");

        needleAngle = -1 * speed + maxNeedleAngle;
        needleRect.rotation = Quaternion.Euler(0.0f, 0.0f, needleAngle);
    }
}
