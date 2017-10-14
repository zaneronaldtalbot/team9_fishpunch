using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapsManager : MonoBehaviour {

    public GameObject Lapcounter;
    [Tooltip("Put in Current checkpoint")]
    public GameObject currentCheckPoint;
    [Tooltip("Put in Next Checkpoint")]
    public GameObject nextCheckpoint;

    public GameObject FinishLine;
    [HideInInspector]
    public int lapNumber = 0;

    public void OnTriggerEnter()
    {
        if (currentCheckPoint == FinishLine)
        {
            lapNumber += 1;
            currentCheckPoint.SetActive(false);
            nextCheckpoint.SetActive(true);


            Lapcounter.GetComponent<Text>().text = "" + lapNumber;
        }
        else
        {
            currentCheckPoint.SetActive(false);
            nextCheckpoint.SetActive(true);
        }
    }
}
