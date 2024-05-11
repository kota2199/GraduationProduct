using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    private GameObject master;
    DateStore dateStore;

    public Text pointsText;

    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("GameMaster");
        dateStore = master.GetComponent<DateStore>();
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = "Points : " + dateStore.points.ToString();
    }
}
