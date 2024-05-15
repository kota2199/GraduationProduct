using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{

    public float time = 0f;//時間を記録する小数も入る変数.

    public Text timeText;

    public bool timeCounting;

    void Start()
    {
        timeCounting = false;
    }

    void Update()
    {
        if (timeCounting)
        {
            time += Time.deltaTime;//毎フレームの時間を加算.
            int minute = (int)time / 60;//分.timeを60で割った値.
            float second = time % 60;//秒.timeを60で割った余り.
            string minText, secText;//テキスト形式の分・秒を用意.

            if (minute < 10)
                minText = "0" + minute.ToString();//("0"埋め), ToStringでint→stringに変換.
            else
                minText = minute.ToString();

            if (second < 10)
                secText = "0" + second.ToString("f2");//上に同じく.
            else
                secText = second.ToString("f2");

            timeText.text = minText + ":" + secText;
        }
    }
}
