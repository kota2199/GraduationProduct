using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceController : MonoBehaviour
{
    public Transform replacePoint;

    private Vector3 replacePosition;
    private Quaternion replaceRotation;

    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        SetReplacePoint(this.gameObject.transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Replace();
        }
    }

    public void SetReplacePoint(Transform point)
    {
        replacePosition = point.position;
        replaceRotation = point.transform.rotation;
    }

    public void Replace()
    {
        rigid.velocity = Vector3.zero;
        transform.position = replacePosition;
        transform.rotation = replaceRotation;
    }
}
