using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LapsManager : MonoBehaviour
{

    private ItemManager iManager;
    private NewPlacementController npc;
    private PlayerSelectActor psActor;

    public GameObject newManager;

    [HideInInspector]
    public bool raceOver = false;

    private GameObject raceRestart;
    private Text restartText;

    private float restartTime = 10.0f;
    private int intTime;

    public List<GameObject> checkPoints;

    private GameObject checkpoint;

    [HideInInspector]
    public bool check1 = false;
    [HideInInspector]
    public bool check2 = false;
    [HideInInspector]
    public bool check3 = false;
    [HideInInspector]
    public bool check4 = false;



    public GameObject Lapcounter;

    public GameObject FinishLine;
    [HideInInspector]
    public int lapNumber = 3;

    void Start()
    {
        npc = this.gameObject.GetComponent<NewPlacementController>();
        iManager = this.gameObject.GetComponent<ItemManager>();
        psActor = this.gameObject.GetComponent<PlayerSelectActor>();
        checkpoint = GameObject.Find("CheckPoint1");
        checkPoints.Add(checkpoint);

        checkpoint = GameObject.Find("CheckPoint2");
        checkPoints.Add(checkpoint);

        checkpoint = GameObject.Find("CheckPoint3");
        checkPoints.Add(checkpoint);

        checkpoint = GameObject.Find("CheckPoint4");
        checkPoints.Add(checkpoint);

        FinishLine = GameObject.Find("StartLine");

        raceRestart = GameObject.Find("RestartRace");
        restartText = raceRestart.GetComponent<Text>();
    }

    private void Update()
    {
        Debug.Log("Lap: " + lapNumber);

        if(lapNumber == 3)
        {
            restartTime -= Time.deltaTime;

            raceOver = true;
            restartText.enabled = true;
            intTime = (int)restartTime;
            restartText.text = "Race Restarts in: " + intTime.ToString();

            iManager.enabled = false;
            npc.enabled = false;
            this.enabled = false;
            psActor.enabled = true;

            if(restartTime < 0)
            {
                SceneManager.LoadScene(1);
                Instantiate(newManager);
                GameObject.Destroy(this.gameObject);
            }
        }
    }


    public void OnTriggerEnter(Collider coll)
    {



    }
}
