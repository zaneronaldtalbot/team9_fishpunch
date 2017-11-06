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
    private Text first, second, third, fourth;
    private Text raceEnd;
    private Text raceCountdown;

    public Sprite greenLight;
    public Sprite redLight;

    private Image countOne, countTwo, countThree, countFour;

    private float raceCountdownTimer = 3.0f;
    private int intRaceCountdown;

    private float restartTime = 10.0f;
    private float endRaceTime = 10;
    private int intTime;
    [HideInInspector]
    public int endTime;
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

    private PlayerActor kart1, kart2, kart3, kart4;

    public GameObject Lapcounter;

    public GameObject FinishLine;

    [HideInInspector]
    public int lapNumber = 0;
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

        checkpoint = GameObject.Find("CheckPoint5");
        checkPoints.Add(checkpoint);

        checkpoint = GameObject.Find("CheckPoint6");
        checkPoints.Add(checkpoint);

        checkpoint = GameObject.Find("CheckPoint7");
        checkPoints.Add(checkpoint);

        checkpoint = GameObject.Find("CheckPoint8");
        checkPoints.Add(checkpoint);

        checkpoint = GameObject.Find("CheckPoint9");
        checkPoints.Add(checkpoint);

        checkpoint = GameObject.Find("CheckPoint10");
        checkPoints.Add(checkpoint);

        checkpoint = GameObject.Find("CheckPoint11");
        checkPoints.Add(checkpoint);

        checkpoint = GameObject.Find("CheckPoint12");
        checkPoints.Add(checkpoint);

        FinishLine = GameObject.Find("StartLine");

        countOne = GameObject.Find("Startlight1").GetComponent<Image>();
        countTwo = GameObject.Find("Startlight2").GetComponent<Image>();
        countThree = GameObject.Find("Startlight3").GetComponent<Image>();
        countFour = GameObject.Find("Startlight4").GetComponent<Image>();

        raceCountdown = GameObject.Find("RaceCountdown").GetComponent<Text>();

        raceRestart = GameObject.Find("RestartRace");
        restartText = raceRestart.GetComponent<Text>();
        first = GameObject.Find("firstPlace").GetComponent<Text>();
        second = GameObject.Find("secondPlace").GetComponent<Text>();
        third = GameObject.Find("thirdPlace").GetComponent<Text>();
        fourth = GameObject.Find("fourthPlace").GetComponent<Text>();
        raceEnd = GameObject.Find("RaceEnd").GetComponent<Text>();

        switch(psActor.playerCount)
        {
            case 2:
                kart1 = GameObject.Find("PlayerCharacter_001").GetComponent<PlayerActor>();
                kart2 = GameObject.Find("PlayerCharacter_002").GetComponent<PlayerActor>();
                break;
            case 3:

                kart1 = GameObject.Find("PlayerCharacter_001").GetComponent<PlayerActor>();
                kart2 = GameObject.Find("PlayerCharacter_002").GetComponent<PlayerActor>();
                kart3 = GameObject.Find("PlayerCharacter_003").GetComponent<PlayerActor>();

                break;
            case 4:
                kart1 = GameObject.Find("PlayerCharacter_001").GetComponent<PlayerActor>();
                kart2 = GameObject.Find("PlayerCharacter_002").GetComponent<PlayerActor>();
                kart3 = GameObject.Find("PlayerCharacter_003").GetComponent<PlayerActor>();
                kart4 = GameObject.Find("PlayerCharacter_004").GetComponent<PlayerActor>();
                break;
        }
        


    }

    private void Update()
    {
        Debug.Log("Lap: " + lapNumber);
        raceCountdownTimer -= Time.deltaTime;
        intRaceCountdown = (int)raceCountdownTimer;
        raceCountdown.text = intRaceCountdown.ToString();
        if(raceCountdownTimer < 0)
        {
            raceCountdown.enabled = false;
        }
        if(raceCountdownTimer < 3)
        {
            countOne.sprite = greenLight;
        }
        if(raceCountdownTimer <  2)
        {
            countTwo.sprite = greenLight;
        }
        if(raceCountdownTimer < 1)
        {
            countThree.sprite = greenLight;
        }
        if(raceCountdownTimer < 0)
        {
            countFour.sprite = greenLight;
        }
        if(raceCountdownTimer < -0.5f)
        {
            countFour.enabled = false;
            countThree.enabled = false;
            countTwo.enabled = false;
            countOne.enabled = false;
        }

        switch (psActor.playerCount)
        {
            case 1:
                if (lapNumber == 3)
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

                    if (restartTime < 0)
                    {
                        SceneManager.LoadScene(1);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }
                }
                break;
            case 2:
                if ((kart1.lapNumber == 3 || kart2.lapNumber == 3) && !raceOver)
                {
                    endRaceTime -= Time.deltaTime;
                    endTime = (int)endRaceTime;
                    raceEnd.enabled = true;
                    raceEnd.text = "Race ends in: " + endTime.ToString();
                }
                if (endTime < 0)
                {
                    restartTime -= Time.deltaTime;
                    raceOver = true;
                    raceEnd.enabled = false;
                    restartText.enabled = true;
                    intTime = (int)restartTime;
                    restartText.text = "Race Restarts in: " + intTime.ToString();

                    first.enabled = true;
                    second.enabled = true;
                    if (kart1.kartPosition == 1 && kart2.kartPosition == 2)
                    {
                        first.color = Color.red;
                        first.text = "1st: Player 1";
                        second.color = Color.blue;
                        second.text = "2nd: Player 2";
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 1)
                    {
                        first.color = Color.blue;
                        first.text = "1st: Player 2";
                        second.color = Color.red;
                        second.text = "2nd: Player 1";
                    }


                    iManager.enabled = false;
                    npc.enabled = false;
                    this.enabled = false;
                    psActor.enabled = true;

                    if (restartTime < 0)
                    {
                        SceneManager.LoadScene(1);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }

                }
                break;
            case 3:
                if ((kart1.lapNumber == 3 || kart2.lapNumber == 3 || kart3.lapNumber == 3) && !raceOver)
                {
                    endRaceTime -= Time.deltaTime;
                    endTime = (int)endRaceTime;
                    raceEnd.enabled = true;
                    raceEnd.text = "Race ends in: " + endTime.ToString();
                }
                if (endTime < 0)
                {
                    restartTime -= Time.deltaTime;
                    raceOver = true;
                    raceEnd.enabled = false;
                    restartText.enabled = true;
                    intTime = (int)restartTime;
                    restartText.text = "Race Restarts in: " + intTime.ToString();

                    first.enabled = true;
                    second.enabled = true;
                    third.enabled = true;
                    if (kart1.kartPosition == 1 && kart2.kartPosition == 2 && kart3.kartPosition == 3)
                    {
                        first.color = Color.red;
                        first.text = "1st: Player 1";
                        second.color = Color.blue;
                        second.text = "2nd: Player 2";
                        third.color = Color.green;
                        third.text = "3rd: Player 3";
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 3 && kart3.kartPosition == 2)
                    {
                        first.color = Color.red;
                        first.text = "1st: Player 1";
                        second.color = Color.green;
                        second.text = "2nd: Player 3";
                        third.color = Color.blue;
                        third.text = "3rd: Player 2";
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 1 && kart3.kartPosition == 3)
                    {
                        first.color = Color.blue;
                        first.text = "1st: Player 2";
                        second.color = Color.red;
                        second.text = "2nd: Player 1";
                        third.color = Color.green;
                        third.text = "3rd: Player 3";
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 3 && kart3.kartPosition == 1)
                    {
                        first.color = Color.green;
                        first.text = "1st: Player 3";
                        second.color = Color.red;
                        second.text = "2nd: Player 1";
                        third.color = Color.blue;
                        third.text = "3rd: Player 2";
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 2 && kart3.kartPosition == 1)
                    {
                        first.color = Color.green;
                        first.text = "1st: Player 3";
                        second.color = Color.blue;
                        second.text = "2nd: Player 2";
                        third.color = Color.red;
                        third.text = "3rd: Player 1";
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 1 && kart3.kartPosition == 2)
                    {
                        first.color = Color.blue;
                        first.text = "1st: Player 2";
                        second.color = Color.green;
                        second.text = "2nd: Player 3";
                        third.color = Color.red;
                        third.text = "3rd: Player 1";
                    }

                    iManager.enabled = false;
                    npc.enabled = false;
                    this.enabled = false;
                    psActor.enabled = true;

                    if (restartTime < 0)
                    {
                        SceneManager.LoadScene(1);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }

                }
                break;
            case 4:               
                if ((kart1.lapNumber == 3 || kart2.lapNumber == 3 || kart3.lapNumber == 3 || kart4.lapNumber == 3) && !raceOver)
                {
                    endRaceTime -= Time.deltaTime;
                    endTime = (int)endRaceTime;
                    raceEnd.enabled = true;
                    raceEnd.text = "Race ends in: " + endTime.ToString();
                }
                if (endTime < 0)
                {
                    restartTime -= Time.deltaTime;
                    raceOver = true;
                    raceEnd.enabled = false;
                    restartText.enabled = true;
                    intTime = (int)restartTime;
                    restartText.text = "Race Restarts in: " + intTime.ToString();

                    first.enabled = true;
                    second.enabled = true;
                    third.enabled = true;
                    //Kart 1
                    if (kart1.kartPosition == 1 && kart2.kartPosition == 2 && kart3.kartPosition == 3 && kart4.kartPosition == 4)
                    {
                        first.color = Color.red;
                        first.text = "1st: Player 1";
                        second.color = Color.blue;
                        second.text = "2nd: Player 2";
                        third.color = Color.green;
                        third.text = "3rd: Player 3";
                        fourth.color = Color.magenta;
                        fourth.text = "4th: Player 4";
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 2 && kart3.kartPosition == 4 && kart4.kartPosition == 3)
                    {
                        first.color = Color.red;
                        first.text = "1st: Player 1";
                        second.color = Color.blue;
                        second.text = "2nd: Player 2";
                        third.color = Color.magenta;
                        third.text = "3rd: Player 4";
                        fourth.color = Color.green;
                        fourth.text = "4th: Player 3";
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 3 && kart3.kartPosition == 2 && kart4.kartPosition == 4)
                    {
                        first.color = Color.red;
                        first.text = "1st: Player 1";
                        second.color = Color.green;
                        second.text = "2nd: Player 3";
                        third.color = Color.blue;
                        third.text = "3rd: Player 2";
                        fourth.color = Color.magenta;
                        fourth.text = "4th: Player 4";
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 3 && kart3.kartPosition == 4 && kart4.kartPosition == 2)
                    {
                        first.color = Color.red;
                        first.text = "1st: Player 1";
                        second.color = Color.magenta;
                        second.text = "2nd: Player 4";
                        third.color = Color.blue;
                        third.text = "3rd: Player 2";
                        fourth.color = Color.green;
                        fourth.text = "4th: Player 3";
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 4 && kart3.kartPosition == 2 && kart4.kartPosition == 3)
                    {
                        first.color = Color.red;
                        first.text = "1st: Player 1";
                        second.color = Color.green;
                        second.text = "2nd: Player 3";
                        third.color = Color.magenta;
                        third.text = "3rd: Player 4";
                        fourth.color = Color.blue;
                        fourth.text = "4th: Player 2";
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 4 && kart3.kartPosition == 3 && kart4.kartPosition == 2)
                    {
                        first.color = Color.red;
                        first.text = "1st: Player 1";
                        second.color = Color.magenta;
                        second.text = "2nd: Player 4";
                        third.color = Color.green;
                        third.text = "3rd: Player 3";
                        fourth.color = Color.blue;
                        fourth.text = "4th: Player 2";
                    }
                    //Kart 2
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 1 && kart3.kartPosition == 3 && kart4.kartPosition == 4)
                    {
                        first.color = Color.blue;
                        first.text = "1st: Player 2";
                        second.color = Color.red;
                        second.text = "2nd: Player 1";
                        third.color = Color.green;
                        third.text = "3rd: Player 3";
                        fourth.color = Color.magenta;
                        fourth.text = "4th: Player 4";
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 1 && kart3.kartPosition == 4 && kart4.kartPosition == 3)
                    {
                        first.color = Color.blue;
                        first.text = "1st: Player 2";
                        second.color = Color.red;
                        second.text = "2nd: Player 1";
                        third.color = Color.magenta;
                        third.text = "3rd: Player 4";
                        fourth.color = Color.green;
                        fourth.text = "4th: Player 3";
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 3 && kart3.kartPosition == 1 && kart4.kartPosition == 4)
                    {
                        first.color = Color.green;
                        first.text = "1st: Player 3";
                        second.color = Color.red;
                        second.text = "2nd: Player 1";
                        third.color = Color.blue;
                        third.text = "3rd: Player 2";
                        fourth.color = Color.magenta;
                        fourth.text = "4th: Player 4";
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 3 && kart3.kartPosition == 4 && kart4.kartPosition == 1)
                    {
                        first.color = Color.magenta;
                        first.text = "1st: Player 4";
                        second.color = Color.red;
                        second.text = "2nd: Player 1";
                        third.color = Color.blue;
                        third.text = "3rd: Player 2";
                        fourth.color = Color.green;
                        fourth.text = "4th: Player 3";
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 4 && kart3.kartPosition == 1 && kart4.kartPosition == 3)
                    {
                        first.color = Color.green;
                        first.text = "1st: Player 3";
                        second.color = Color.red;
                        second.text = "2nd: Player 1";
                        third.color = Color.magenta;
                        third.text = "3rd: Player 4";
                        fourth.color = Color.blue;
                        fourth.text = "4th: Player 2";
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 4 && kart3.kartPosition == 3 && kart4.kartPosition == 1)
                    {
                        first.color = Color.blue;
                        first.text = "1st: Player 1";
                        second.color = Color.magenta;
                        second.text = "2nd: Player 4";
                        third.color = Color.green;
                        third.text = "3rd: Player 3";
                        fourth.color = Color.blue;
                        fourth.text = "4th: Player 2";
                    }
                    //kart 3
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 1 && kart3.kartPosition == 2 && kart4.kartPosition == 4)
                    {
                        first.color = Color.blue;
                        first.text = "1st: Player 2";
                        second.color = Color.green;
                        second.text = "2nd: Player 3";
                        third.color = Color.red;
                        third.text = "3rd: Player 1";
                        fourth.color = Color.magenta;
                        fourth.text = "4th: Player 4";
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 1 && kart3.kartPosition == 4 && kart4.kartPosition == 2)
                    {
                        first.color = Color.blue;
                        first.text = "1st: Player 2";
                        second.color = Color.magenta;
                        second.text = "2nd: Player 4";
                        third.color = Color.red;
                        third.text = "3rd: Player 1";
                        fourth.color = Color.green;
                        fourth.text = "4th: Player 3";
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 2 && kart3.kartPosition == 1 && kart4.kartPosition == 4)
                    {
                        first.color = Color.green;
                        first.text = "1st: Player 3";
                        second.color = Color.blue;
                        second.text = "2nd: Player 2";
                        third.color = Color.red;
                        third.text = "3rd: Player 1";
                        fourth.color = Color.magenta;
                        fourth.text = "4th: Player 4";
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 2 && kart3.kartPosition == 4 && kart4.kartPosition == 1)
                    {
                        first.color = Color.magenta;
                        first.text = "1st: Player 4";
                        second.color = Color.blue;
                        second.text = "2nd: Player 2";
                        third.color = Color.red;
                        third.text = "3rd: Player 1";
                        fourth.color = Color.green;
                        fourth.text = "4th: Player 3";
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 4 && kart3.kartPosition == 1 && kart4.kartPosition == 2)
                    {
                        first.color = Color.green;
                        first.text = "1st: Player 3";
                        second.color = Color.magenta;
                        second.text = "2nd: Player 4";
                        third.color = Color.red;
                        third.text = "3rd: Player 1";
                        fourth.color = Color.blue;
                        fourth.text = "4th: Player 2";
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 4 && kart3.kartPosition == 2 && kart4.kartPosition == 1)
                    {
                        first.color = Color.magenta;
                        first.text = "1st: Player 4";
                        second.color = Color.green;
                        second.text = "2nd: Player 3";
                        third.color = Color.red;
                        third.text = "3rd: Player 1";
                        fourth.color = Color.blue;
                        fourth.text = "4th: Player 2";
                    }
                    //kart 4
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 1 && kart3.kartPosition == 2 && kart4.kartPosition == 3)
                    {
                        first.color = Color.blue;
                        first.text = "1st: Player 2";
                        second.color = Color.green;
                        second.text = "2nd: Player 3";
                        third.color = Color.magenta;
                        third.text = "3rd: Player 4";
                        fourth.color = Color.red;
                        fourth.text = "4th: Player 1";
                    }
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 1 && kart3.kartPosition == 3 && kart4.kartPosition == 2)
                    {
                        first.color = Color.blue;
                        first.text = "1st: Player 2";
                        second.color = Color.magenta;
                        second.text = "2nd: Player 4";
                        third.color = Color.green;
                        third.text = "3rd: Player 3";
                        fourth.color = Color.red;
                        fourth.text = "4th: Player 1";
                    }
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 2 && kart3.kartPosition == 1 && kart4.kartPosition == 3)
                    {
                        first.color = Color.green;
                        first.text = "1st: Player 3";
                        second.color = Color.blue;
                        second.text = "2nd: Player 2";
                        third.color = Color.magenta;
                        third.text = "3rd: Player 4";
                        fourth.color = Color.red;
                        fourth.text = "4th: Player 1";
                    }
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 2 && kart3.kartPosition == 3 && kart4.kartPosition == 1)
                    {
                        first.color = Color.magenta;
                        first.text = "1st: Player 4";
                        second.color = Color.blue;
                        second.text = "2nd: Player 2";
                        third.color = Color.green;
                        third.text = "3rd: Player 3";
                        fourth.color = Color.red;
                        fourth.text = "4th: Player 1";
                    }
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 3 && kart3.kartPosition == 1 && kart4.kartPosition == 2)
                    {
                        first.color = Color.green;
                        first.text = "1st: Player 3";
                        second.color = Color.magenta;
                        second.text = "2nd: Player 4";
                        third.color = Color.blue;
                        third.text = "3rd: Player 2";
                        fourth.color = Color.red;
                        fourth.text = "4th: Player 1";
                    }
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 3 && kart3.kartPosition == 2 && kart4.kartPosition == 1)
                    {
                        first.color = Color.magenta;
                        first.text = "1st: Player 4";
                        second.color = Color.green;
                        second.text = "2nd: Player 3";
                        third.color = Color.blue;
                        third.text = "3rd: Player 2";
                        fourth.color = Color.red;
                        fourth.text = "4th: Player 1";
                    }

                    iManager.enabled = false;
                    npc.enabled = false;
                    this.enabled = false;
                    psActor.enabled = true;

                    if (restartTime < 0)
                    {
                        SceneManager.LoadScene(1);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }

                }
                break;
        }


      
    }


    public void OnTriggerEnter(Collider coll)
    {



    }
}
