using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LapsManager : MonoBehaviour
{

    bool set = false;

    private ItemManager iManager;
    private NewPlacementController npc;
    private PlayerSelectActor psActor;

    public AudioSource countDown1;
    public AudioSource countDown2;
    public AudioSource countDown3;
    public AudioSource countDownEnd;

    private Image backBoard;

    public GameObject newManager;

    [HideInInspector]
    public bool raceOver = false;

    private GameObject raceRestart;
    private Text restartText;
    private Text first, second, third, fourth;
    private Text raceEnd;
    private Text raceCountdown;

    public Sprite firstP, secondP, thirdP, fourthP;
    private Image posOne, posTwo, posThree, posFour;

    public Sprite greenLight;
    public Sprite redLight;

    private Image countOne, countTwo, countThree, countFour;

    [HideInInspector]
    public float raceCountdownTimer = 3.0f;
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

    private Image firstPlace, secondPlace, thirdPlace, fourthPlace;

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
        posOne = GameObject.Find("firstPlayer").GetComponent<Image>();
        posTwo = GameObject.Find("secondPlayer").GetComponent<Image>();
        posThree = GameObject.Find("thirdPlayer").GetComponent<Image>();
        posFour = GameObject.Find("fourthPlayer").GetComponent<Image>();
        raceEnd = GameObject.Find("RaceEnd").GetComponent<Text>();
        thirdPlace = GameObject.Find("thirdPlace").GetComponent<Image>();
        fourthPlace = GameObject.Find("fourthPlace").GetComponent<Image>();
        firstPlace = GameObject.Find("firstPlace").GetComponent<Image>();
        secondPlace = GameObject.Find("secondPlace").GetComponent<Image>();

        countDown1 = countOne.GetComponent<AudioSource>();
        countDown2 = countTwo.GetComponent<AudioSource>();
        countDown3 = countThree.GetComponent<AudioSource>();
        countDownEnd = countFour.GetComponent<AudioSource>();

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

        posOne.enabled = false;
        posTwo.enabled = false;
        posThree.enabled = false;
        posFour.enabled = false;
        firstPlace.enabled = false;
        secondPlace.enabled = false;
        thirdPlace.enabled = false;
        fourthPlace.enabled = false;

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
        if(raceCountdownTimer < 3 && raceCountdownTimer > 2)
        {
            if(!countDown1.isPlaying)
            countDown1.Play();
            countOne.sprite = greenLight;
        }
        if(raceCountdownTimer <  2 && raceCountdownTimer > 1)
        {
            if(!countDown2.isPlaying)
            countDown2.Play();
            countTwo.sprite = greenLight;
        }
        if(raceCountdownTimer < 1 && raceCountdownTimer > 0)
        {
            if(!countDown3.isPlaying)
            countDown3.Play();
         countThree.sprite = greenLight;
        }
        if(raceCountdownTimer < 0 && raceCountdownTimer > -0.5f)
        {
            if (!countDownEnd.isPlaying)
            {
                countDownEnd.Play();
            }
            countFour.sprite = greenLight;
        }
        if(raceCountdownTimer < -0.5f)
        {
            if (!set)
            {
                GameObject.Find("Backboard").SetActive(false);
                set = true;
            }
            countFour.enabled = false;
            countThree.enabled = false;
            countTwo.enabled = false;
            countOne.enabled = false;
        }


        switch (psActor.playerCount)
        {

            case 2:

                if ((kart1.lapNumber == 4 || kart2.lapNumber == 4) && !raceOver)
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

                    posOne.enabled = true;
                    posTwo.enabled = true;
                    firstPlace.enabled = true;
                    secondPlace.enabled = true;
                    if (kart1.kartPosition == 1 && kart2.kartPosition == 2)
                    {
                        posOne.sprite = firstP;
                        posTwo.sprite = secondP;
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 1)
                    {
                        posOne.sprite = secondP;
                        posTwo.sprite = firstP;
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


                if ((kart1.lapNumber == 4 || kart2.lapNumber == 4 || kart3.lapNumber == 4) && !raceOver)
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

                    posOne.enabled = true;
                    posTwo.enabled = true;
                    posThree.enabled = true;
                    firstPlace.enabled = true;
                    secondPlace.enabled = true;
                    thirdPlace.enabled = true;
                    if (kart1.kartPosition == 1 && kart2.kartPosition == 2 && kart3.kartPosition == 3)
                    {
                        posOne.sprite = firstP;
                        posTwo.sprite = secondP;
                        posThree.sprite = thirdP;
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 3 && kart3.kartPosition == 2)
                    {
                        posOne.sprite = firstP;
                        posTwo.sprite = thirdP;
                        posThree.sprite = secondP;
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 1 && kart3.kartPosition == 3)
                    {
                        posOne.sprite = secondP;
                        posTwo.sprite = firstP;
                        posThree.sprite = thirdP;
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 3 && kart3.kartPosition == 1)
                    {
                        posOne.sprite = secondP;
                        posTwo.sprite = thirdP;
                        posThree.sprite = firstP;
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 2 && kart3.kartPosition == 1)
                    {
                        posOne.sprite = thirdP;
                        posTwo.sprite = secondP;
                        posThree.sprite = firstP;
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 1 && kart3.kartPosition == 2)
                    {
                        posOne.sprite = thirdP;
                        posTwo.sprite = firstP;
                        posThree.sprite = secondP;
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
                if ((kart1.lapNumber == 4 || kart2.lapNumber == 4 || kart3.lapNumber == 4 || kart4.lapNumber == 4) && !raceOver)
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

                    posOne.enabled = true;
                    posTwo.enabled = true;
                    posThree.enabled = true;
                    posFour.enabled = true;
                    firstPlace.enabled = true;
                    secondPlace.enabled = true;
                    thirdPlace.enabled = true;
                    fourthPlace.enabled = true;
                    //Kart 1
                    if (kart1.kartPosition == 1 && kart2.kartPosition == 2 && kart3.kartPosition == 3 && kart4.kartPosition == 4)
                    {
                        posOne.sprite = firstP;
                        posTwo.sprite = secondP;
                        posThree.sprite = thirdP;
                        posFour.sprite = fourthP;
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 2 && kart3.kartPosition == 4 && kart4.kartPosition == 3)
                    {
                        posOne.sprite = firstP;
                        posTwo.sprite = secondP;
                        posThree.sprite = fourthP;
                        posFour.sprite = thirdP;
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 3 && kart3.kartPosition == 2 && kart4.kartPosition == 4)
                    {
                        posOne.sprite = firstP;
                        posTwo.sprite = thirdP;
                        posThree.sprite = secondP;
                        posFour.sprite = fourthP;
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 3 && kart3.kartPosition == 4 && kart4.kartPosition == 2)
                    {
                        posOne.sprite = firstP;
                        posTwo.sprite = thirdP;
                        posThree.sprite = fourthP;
                        posFour.sprite = secondP;
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 4 && kart3.kartPosition == 2 && kart4.kartPosition == 3)
                    {
                        posOne.sprite = firstP;
                        posTwo.sprite = fourthP;
                        posThree.sprite = secondP;
                        posFour.sprite = thirdP;
                    }
                    else if (kart1.kartPosition == 1 && kart2.kartPosition == 4 && kart3.kartPosition == 3 && kart4.kartPosition == 2)
                    {
                        posOne.sprite = firstP;
                        posTwo.sprite = fourthP;
                        posThree.sprite = thirdP;
                        posFour.sprite = secondP;
                    }
                    //Kart 2
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 1 && kart3.kartPosition == 3 && kart4.kartPosition == 4)
                    {
                        posOne.sprite = secondP;
                        posTwo.sprite = firstP;
                        posThree.sprite = thirdP;
                        posFour.sprite = fourthP;
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 1 && kart3.kartPosition == 4 && kart4.kartPosition == 3)
                    {
                        posOne.sprite = secondP;
                        posTwo.sprite = firstP;
                        posThree.sprite = fourthP;
                        posFour.sprite = thirdP;
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 3 && kart3.kartPosition == 1 && kart4.kartPosition == 4)
                    {
                        posOne.sprite = secondP;
                        posTwo.sprite = thirdP;
                        posThree.sprite = firstP;
                        posFour.sprite = fourthP;
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 3 && kart3.kartPosition == 4 && kart4.kartPosition == 1)
                    {
                        posOne.sprite = secondP;
                        posTwo.sprite = thirdP;
                        posThree.sprite = fourthP;
                        posFour.sprite = firstP;
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 4 && kart3.kartPosition == 1 && kart4.kartPosition == 3)
                    {
                        posOne.sprite = secondP;
                        posTwo.sprite = fourthP;
                        posThree.sprite = firstP;
                        posFour.sprite = thirdP;
                    }
                    else if (kart1.kartPosition == 2 && kart2.kartPosition == 4 && kart3.kartPosition == 3 && kart4.kartPosition == 1)
                    {
                        posOne.sprite = secondP;
                        posTwo.sprite = fourthP;
                        posThree.sprite = thirdP;
                        posFour.sprite = firstP;
                    }
                    //kart 3
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 1 && kart3.kartPosition == 2 && kart4.kartPosition == 4)
                    {
                        posOne.sprite = thirdP;
                        posTwo.sprite = firstP;
                        posThree.sprite = secondP;
                        posFour.sprite = fourthP;
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 1 && kart3.kartPosition == 4 && kart4.kartPosition == 2)
                    {
                        posOne.sprite = thirdP;
                        posTwo.sprite = firstP;
                        posThree.sprite = fourthP;
                        posFour.sprite = secondP;
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 2 && kart3.kartPosition == 1 && kart4.kartPosition == 4)
                    {
                        posOne.sprite = thirdP;
                        posTwo.sprite = secondP;
                        posThree.sprite = firstP;
                        posFour.sprite = fourthP;
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 2 && kart3.kartPosition == 4 && kart4.kartPosition == 1)
                    {
                        posOne.sprite = thirdP;
                        posTwo.sprite = secondP;
                        posThree.sprite = fourthP;
                        posFour.sprite = firstP;
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 4 && kart3.kartPosition == 1 && kart4.kartPosition == 2)
                    {
                        posOne.sprite = thirdP;
                        posTwo.sprite = fourthP;
                        posThree.sprite = firstP;
                        posFour.sprite = secondP;
                    }
                    else if (kart1.kartPosition == 3 && kart2.kartPosition == 4 && kart3.kartPosition == 2 && kart4.kartPosition == 1)
                    {
                        posOne.sprite = thirdP;
                        posTwo.sprite = fourthP;
                        posThree.sprite = secondP;
                        posFour.sprite = firstP;
                    }
                    //kart 4
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 1 && kart3.kartPosition == 2 && kart4.kartPosition == 3)
                    {
                        posOne.sprite = fourthP;
                        posTwo.sprite = firstP;
                        posThree.sprite = secondP;
                        posFour.sprite = thirdP;
                    }
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 1 && kart3.kartPosition == 3 && kart4.kartPosition == 2)
                    {
                        posOne.sprite = fourthP;
                        posTwo.sprite = firstP;
                        posThree.sprite = thirdP;
                        posFour.sprite = secondP;
                    }
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 2 && kart3.kartPosition == 1 && kart4.kartPosition == 3)
                    {
                        posOne.sprite = fourthP;
                        posTwo.sprite = secondP;
                        posThree.sprite = firstP;
                        posFour.sprite = thirdP;
                    }
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 2 && kart3.kartPosition == 3 && kart4.kartPosition == 1)
                    {
                        posOne.sprite = fourthP;
                        posTwo.sprite = secondP;
                        posThree.sprite = thirdP;
                        posFour.sprite = firstP;
                    }
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 3 && kart3.kartPosition == 1 && kart4.kartPosition == 2)
                    {
                        posOne.sprite = fourthP;
                        posTwo.sprite = thirdP;
                        posThree.sprite = firstP;
                        posFour.sprite = secondP;
                    }
                    else if (kart1.kartPosition == 4 && kart2.kartPosition == 3 && kart3.kartPosition == 2 && kart4.kartPosition == 1)
                    {
                        posOne.sprite = fourthP;
                        posTwo.sprite = thirdP;
                        posThree.sprite = secondP;
                        posFour.sprite = firstP;
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
}
