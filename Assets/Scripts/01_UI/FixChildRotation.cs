using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixChildRotation : MonoBehaviour
{

    [SerializeField]
    private Vector3 offset;

    Vector3 defaultAngle;

    void Awake()
    {
        defaultAngle = transform.localRotation.eulerAngles;
    }

    void Update()
    {
        Vector3 parentAngle = transform.parent.transform.localRotation.eulerAngles;

        transform.localRotation = Quaternion.Euler(defaultAngle - parentAngle - offset);
    }
}