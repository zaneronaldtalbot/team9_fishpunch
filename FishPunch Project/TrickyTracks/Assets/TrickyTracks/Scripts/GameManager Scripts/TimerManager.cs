using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

    public static int minuteCount;
    public static int secondCount;
    public static float milliCount;
    public static string milliDesplay;

    public GameObject MinuteBox;
    public GameObject SecondBox;
    public GameObject MilliBox;
	
	// Update is called once per frame
	void Update () {

        milliCount += Time.deltaTime * 10;
        milliDesplay = milliCount.ToString("f0");
        MilliBox.GetComponent<Text>().text = "" + milliDesplay;

        if (milliCount >= 9)
        {
            milliCount = 0;
            secondCount += 1;
        }

        if (secondCount <= 9)
        {
            SecondBox.GetComponent<Text>().text = "0" + secondCount + ".";
        }
        else
        {
            SecondBox.GetComponent<Text>().text = "" + secondCount + ".";
        }

        if (secondCount >=60)
        {
            secondCount = 0;
            minuteCount += 1;
        }

        if (minuteCount <= 9)
        {
            MinuteBox.GetComponent<Text>().text = "0" + minuteCount + ":";
        }
        else
        {
            MinuteBox.GetComponent<Text>().text = "" + minuteCount + ":";
        }
    }
}
