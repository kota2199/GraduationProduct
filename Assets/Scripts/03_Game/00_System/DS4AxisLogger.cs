using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DS4AxisLogger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Vertical Stick-L : " + Input.GetAxis("Vertical Stick-L"));
    }
}
