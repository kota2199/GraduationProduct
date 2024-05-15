using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChecker : MonoBehaviour
{
    [SerializeField]
    private TimeGapManager gapManager;

    public int checkPointNumber;

    // Start is called before the first frame update
    void Start()
    {
        gapManager = GameObject.FindObjectOfType<TimeGapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Car")
        {
            gapManager.PassPoint(checkPointNumber, other.gameObject);
        }
    }
}
