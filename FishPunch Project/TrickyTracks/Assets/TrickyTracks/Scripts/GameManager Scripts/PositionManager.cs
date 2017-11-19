using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Written by Angus Secomb
//Last edit made 14/11/17.
public class PositionManager : MonoBehaviour {

    private PlayerSelectActor psActor;

    private PlayerActor kart1, kart2, kart3, kart4;
    private GameObject go_kart1, go_kart2, go_kart3, go_kart4;

    private Image playerOnePosition, playerTwoPosition, playerThreePosition, playerFourPosition;
    private Image playerOneLap, playerTwoLap, playerThreeLap, playerFourLap;

    private Image playerOnePos, playerTwoPos, playerThreePos, playerFourPos;
    private Image playerOneLp, playerTwoLp, playerThreeLp, playerFourLp;

    public Sprite positionOne, positionTwo, positionThree, positionFour;
    public Sprite lapOne, lapTwo, lapThree;


    // Use this for initialization
    void Start() {
        psActor = GetComponent<PlayerSelectActor>();

        switch (psActor.playerCount)
        {
            case 2:
                go_kart1 = GameObject.Find("PlayerCharacter_001");
                kart1 = go_kart1.GetComponent<PlayerActor>();
                go_kart2 = GameObject.Find("PlayerCharacter_002");
                kart2 = go_kart2.GetComponent<PlayerActor>();
                playerOnePosition = GameObject.Find("PlayerOnePosition").GetComponent<Image>();
                playerTwoPosition = GameObject.Find("PlayerThreePosition").GetComponent<Image>();
                GameObject.Find("PlayerTwoPosition").SetActive(false);
                GameObject.Find("PlayerFourPosition").SetActive(false);

                playerOneLap = GameObject.Find("PlayerTwoLaps").GetComponent<Image>();
                playerTwoLap = GameObject.Find("PlayerFourLaps").GetComponent<Image>();

                GameObject.Find("PlayerThreeLaps").SetActive(false);
                GameObject.Find("PlayerOneLaps").SetActive(false);            

                break;
            case 3:
                go_kart1 = GameObject.Find("PlayerCharacter_001");
                kart1 = go_kart1.GetComponent<PlayerActor>();
                go_kart2 = GameObject.Find("PlayerCharacter_002");
                kart2 = go_kart2.GetComponent<PlayerActor>();
                go_kart3 = GameObject.Find("PlayerCharacter_003");
                kart3 = go_kart3.GetComponent<PlayerActor>();
                playerOnePosition = GameObject.Find("PlayerOnePosition").GetComponent<Image>();
                playerTwoPosition = GameObject.Find("PlayerTwoPosition").GetComponent<Image>();
                playerThreePosition = GameObject.Find("PlayerThreePosition").GetComponent<Image>();
                GameObject.Find("PlayerFourPosition").SetActive(false);

                playerOneLap = GameObject.Find("PlayerOneLaps").GetComponent<Image>();
                playerTwoLap = GameObject.Find("PlayerTwoLaps").GetComponent<Image>();
                playerThreeLap = GameObject.Find("PlayerThreeLaps").GetComponent<Image>();

                GameObject.Find("PlayerFourLaps").SetActive(false);              

                break;
            case 4:
                go_kart1 = GameObject.Find("PlayerCharacter_001");
                kart1 = go_kart1.GetComponent<PlayerActor>();
                go_kart2 = GameObject.Find("PlayerCharacter_002");
                kart2 = go_kart2.GetComponent<PlayerActor>();
                go_kart3 = GameObject.Find("PlayerCharacter_003");
                kart3 = go_kart3.GetComponent<PlayerActor>();
                go_kart4 = GameObject.Find("PlayerCharacter_004");
                kart4 = go_kart4.GetComponent<PlayerActor>();
                playerOnePosition = GameObject.Find("PlayerOnePosition").GetComponent<Image>();
                playerTwoPosition = GameObject.Find("PlayerTwoPosition").GetComponent<Image>();
                playerThreePosition = GameObject.Find("PlayerThreePosition").GetComponent<Image>();
                playerFourPosition = GameObject.Find("PlayerFourPosition").GetComponent<Image>();

                playerOneLap = GameObject.Find("PlayerOneLaps").GetComponent<Image>();
                playerTwoLap = GameObject.Find("PlayerTwoLaps").GetComponent<Image>();
                playerThreeLap = GameObject.Find("PlayerThreeLaps").GetComponent<Image>();
                playerFourLap = GameObject.Find("PlayerFourLaps").GetComponent<Image>();
                break;
            default:
                break;
        }

    }

    // Update is called once per frame
    void Update() {

        switch(psActor.playerCount)
        {
            case 2:
                positionCalculator(kart1, go_kart1, kart2, go_kart2);
                if(kart1.kartPosition == 1 && kart2.kartPosition == 2)
                {
                    playerOnePosition.sprite = positionOne;
                    playerTwoPosition.sprite = positionTwo;

                }
                else
                {
                    playerOnePosition.sprite = positionTwo;
                    playerTwoPosition.sprite = positionOne;
                }

                if(kart1.lapNumber == 1)
                {
                    playerOneLap.sprite = lapOne;
                }
                else if(kart1.lapNumber == 2)
                {
                    playerOneLap.sprite = lapTwo;
                }
                else if(kart1.lapNumber == 3)
                {
                    playerOneLap.sprite = lapThree;
                }

                if(kart2.lapNumber ==1 )
                {
                    playerTwoLap.sprite = lapOne;
                }
                else if(kart2.lapNumber == 2)
                {
                    playerTwoLap.sprite = lapTwo;
                }
                else if(kart2.lapNumber == 3)
                {
                    playerTwoLap.sprite = lapThree;

                                   }
                //if (kart1.lapNumber < 3)
                //{
                //    playerOneLap.text = (kart1.lapNumber + 1) + "/3";
                //}
                //if (kart2.lapNumber < 3)
                //{
                //    playerTwoLap.text = (kart2.lapNumber + 1) + "/3";
                //}
                break;
            case 3:
                positionCalculator(kart1, go_kart1, kart2, go_kart2, kart3, go_kart3);
                if (kart1.kartPosition == 1 && kart2.kartPosition == 2 && kart3.kartPosition == 3)
                {
                    playerOnePosition.sprite = positionOne;
                    playerTwoPosition.sprite = positionTwo;
                    playerThreePosition.sprite = positionThree;
                }
                else if(kart1.kartPosition == 1 && kart2.kartPosition == 3 && kart3.kartPosition == 2)
                {
                    playerOnePosition.sprite = positionOne;
                    playerTwoPosition.sprite = positionThree;
                    playerThreePosition.sprite = positionTwo;
                }
                else if(kart1.kartPosition == 2 && kart2.kartPosition == 3 && kart3.kartPosition == 1)
                {
                    playerOnePosition.sprite = positionTwo;
                    playerTwoPosition.sprite = positionThree;
                    playerThreePosition.sprite = positionOne;
                }
                else if(kart1.kartPosition == 2 && kart2.kartPosition == 1 && kart3.kartPosition == 3)
                {
                    playerOnePosition.sprite = positionTwo;
                    playerTwoPosition.sprite = positionOne;
                    playerThreePosition.sprite = positionThree;

                }
                else if(kart1.kartPosition == 3 && kart2.kartPosition == 1 && kart3.kartPosition == 2)
                {
                    playerOnePosition.sprite = positionThree;
                    playerTwoPosition.sprite = positionOne;
                    playerThreePosition.sprite = positionTwo;
                }
                else if (kart1.kartPosition == 3 && kart2.kartPosition == 2 && kart3.kartPosition == 1)
                {
                    playerOnePosition.sprite = positionThree;
                    playerTwoPosition.sprite = positionTwo;
                    playerThreePosition.sprite = positionOne;
                }

                if (kart1.lapNumber == 1)
                {
                    playerOneLap.sprite = lapOne;
                }
                else if (kart1.lapNumber == 2)
                {
                    playerOneLap.sprite = lapTwo;
                }
                else if (kart1.lapNumber == 3)
                {
                    playerOneLap.sprite = lapThree;
                }

                if (kart2.lapNumber == 1)
                {
                    playerTwoLap.sprite = lapOne;
                }
                else if (kart2.lapNumber == 2)
                {
                    playerTwoLap.sprite = lapTwo;
                }
                else if (kart2.lapNumber == 3)
                {
                    playerTwoLap.sprite = lapThree;

                }
                if(kart3.lapNumber == 1)
                {
                    playerThreeLap.sprite = lapOne;
                }
                else if(kart3.lapNumber == 2)
                {
                    playerThreeLap.sprite = lapTwo;
                }
                else if(kart3.lapNumber == 3)
                {
                    playerThreeLap.sprite = lapThree;
                }
                    break;
            case 4:
                positionCalculator(kart1, go_kart1, kart2, go_kart2, kart3, go_kart3, kart4, go_kart4);
                //1st
                if (kart1.kartPosition == 1 && kart2.kartPosition == 2 && kart3.kartPosition == 3 && kart4.kartPosition == 4)
                {
                    playerOnePosition.sprite = positionOne;
                    playerTwoPosition.sprite = positionTwo;
                    playerThreePosition.sprite = positionThree;
                    playerFourPosition.sprite = positionFour;
                }
                else if(kart1.kartPosition == 1 && kart2.kartPosition == 2 && kart3.kartPosition == 4 && kart4.kartPosition == 3)
                {
                    playerOnePosition.sprite = positionOne;
                    playerTwoPosition.sprite = positionTwo;
                    playerThreePosition.sprite = positionFour;
                    playerFourPosition.sprite = positionThree;
                }
                else if (kart1.kartPosition == 1 && kart2.kartPosition == 3 && kart3.kartPosition == 2 && kart4.kartPosition == 4)
                {
                    playerOnePosition.sprite = positionOne;
                    playerTwoPosition.sprite = positionThree;
                    playerThreePosition.sprite = positionTwo;
                    playerFourPosition.sprite = positionFour;
                }
                else if (kart1.kartPosition == 1 && kart2.kartPosition == 3 && kart3.kartPosition == 4 && kart4.kartPosition == 2)
                {
                    playerOnePosition.sprite = positionOne;
                    playerTwoPosition.sprite = positionThree;
                    playerThreePosition.sprite = positionFour;
                    playerFourPosition.sprite = positionTwo;
                }
                else if (kart1.kartPosition == 1 && kart2.kartPosition == 4 && kart3.kartPosition == 2 && kart4.kartPosition == 3)
                {
                    playerOnePosition.sprite = positionOne;
                    playerTwoPosition.sprite = positionFour;
                    playerThreePosition.sprite = positionTwo;
                    playerFourPosition.sprite = positionThree;
                }
                else if (kart1.kartPosition == 1 && kart2.kartPosition == 4 && kart3.kartPosition == 3 && kart4.kartPosition == 2)
                {
                    playerOnePosition.sprite = positionOne;
                    playerTwoPosition.sprite = positionFour;
                    playerThreePosition.sprite = positionThree;
                    playerFourPosition.sprite = positionTwo;
                }
                //2md
                else if (kart1.kartPosition == 2 && kart2.kartPosition == 1 && kart3.kartPosition == 3 && kart4.kartPosition == 4)
                {
                    playerOnePosition.sprite = positionTwo;
                    playerTwoPosition.sprite = positionOne;
                    playerThreePosition.sprite = positionThree;
                    playerFourPosition.sprite = positionFour;
                }
                else if (kart1.kartPosition == 2 && kart2.kartPosition == 1 && kart3.kartPosition == 4 && kart4.kartPosition == 3)
                {
                    playerOnePosition.sprite = positionTwo;
                    playerTwoPosition.sprite = positionOne;
                    playerThreePosition.sprite = positionFour;
                    playerFourPosition.sprite = positionThree;
                }
                else if (kart1.kartPosition == 2 && kart2.kartPosition == 3 && kart3.kartPosition == 4 && kart4.kartPosition == 1)
                {
                    playerOnePosition.sprite = positionTwo;
                    playerTwoPosition.sprite = positionThree;
                    playerThreePosition.sprite = positionFour;
                    playerFourPosition.sprite = positionOne;
                }
                else if (kart1.kartPosition == 2 && kart2.kartPosition == 3 && kart3.kartPosition == 1 && kart4.kartPosition == 4)
                {
                    playerOnePosition.sprite = positionTwo;
                    playerTwoPosition.sprite = positionThree;
                    playerThreePosition.sprite = positionOne;
                    playerFourPosition.sprite = positionFour;
                }
                else if (kart1.kartPosition == 2 && kart2.kartPosition == 4 && kart3.kartPosition == 1 && kart4.kartPosition == 3)
                {
                    playerOnePosition.sprite = positionTwo;
                    playerTwoPosition.sprite = positionFour;
                    playerThreePosition.sprite = positionOne;
                    playerFourPosition.sprite = positionThree;
                }
                else if (kart1.kartPosition == 2 && kart2.kartPosition == 4 && kart3.kartPosition == 3 && kart4.kartPosition == 1)
                {
                    playerOnePosition.sprite = positionTwo;
                    playerTwoPosition.sprite = positionFour;
                    playerThreePosition.sprite = positionThree;
                    playerFourPosition.sprite = positionOne;
                }
                //3rd
                else if (kart1.kartPosition == 3 && kart2.kartPosition == 1 && kart3.kartPosition == 2 && kart4.kartPosition == 4)
                {
                    playerOnePosition.sprite = positionThree;
                    playerTwoPosition.sprite = positionOne;
                    playerThreePosition.sprite = positionTwo;
                    playerFourPosition.sprite = positionFour;
                }
                else if (kart1.kartPosition == 3 && kart2.kartPosition == 1 && kart3.kartPosition == 4 && kart4.kartPosition == 2)
                {
                    playerOnePosition.sprite = positionThree;
                    playerTwoPosition.sprite = positionOne;
                    playerThreePosition.sprite = positionFour;
                    playerFourPosition.sprite = positionTwo;
                }
                else if (kart1.kartPosition == 3 && kart2.kartPosition == 2 && kart3.kartPosition == 1 && kart4.kartPosition == 4)
                {
                    playerOnePosition.sprite = positionThree;
                    playerTwoPosition.sprite = positionTwo;
                    playerThreePosition.sprite = positionOne;
                    playerFourPosition.sprite = positionFour;
                }
                else if (kart1.kartPosition == 3 && kart2.kartPosition == 2 && kart3.kartPosition == 4 && kart4.kartPosition == 1)
                {
                    playerOnePosition.sprite = positionThree;
                    playerTwoPosition.sprite = positionTwo;
                    playerThreePosition.sprite = positionFour;
                    playerFourPosition.sprite = positionOne;
                }
                else if (kart1.kartPosition == 3 && kart2.kartPosition == 4 && kart3.kartPosition == 1 && kart4.kartPosition == 2)
                {
                    playerOnePosition.sprite = positionThree;
                    playerTwoPosition.sprite = positionFour;
                    playerThreePosition.sprite = positionOne;
                    playerFourPosition.sprite = positionTwo;
                }
                else if (kart1.kartPosition == 3 && kart2.kartPosition == 4 && kart3.kartPosition == 2 && kart4.kartPosition == 1)
                {
                    playerOnePosition.sprite = positionThree;
                    playerTwoPosition.sprite = positionFour;
                    playerThreePosition.sprite = positionTwo;
                    playerFourPosition.sprite = positionOne;
                }
                //4th
                else if (kart1.kartPosition == 4 && kart2.kartPosition == 1 && kart3.kartPosition == 2 && kart4.kartPosition == 3)
                {
                    playerOnePosition.sprite = positionFour;
                    playerTwoPosition.sprite = positionOne;
                    playerThreePosition.sprite = positionTwo;
                    playerFourPosition.sprite = positionThree;
                }
                else if (kart1.kartPosition == 4 && kart2.kartPosition == 1 && kart3.kartPosition == 3 && kart4.kartPosition == 2)
                {
                    playerOnePosition.sprite = positionFour;
                    playerTwoPosition.sprite = positionOne;
                    playerThreePosition.sprite = positionThree;
                    playerFourPosition.sprite = positionTwo;
                }
                else if (kart1.kartPosition == 4 && kart2.kartPosition == 2 && kart3.kartPosition == 3 && kart4.kartPosition == 1)
                {
                    playerOnePosition.sprite = positionFour;
                    playerTwoPosition.sprite = positionTwo;
                    playerThreePosition.sprite = positionThree;
                    playerFourPosition.sprite = positionOne;
                }
                else if (kart1.kartPosition == 4 && kart2.kartPosition == 2 && kart3.kartPosition == 1 && kart4.kartPosition == 3)
                {
                    playerOnePosition.sprite = positionFour;
                    playerTwoPosition.sprite = positionTwo;
                    playerThreePosition.sprite = positionOne;
                    playerFourPosition.sprite = positionThree;
                }
                else if (kart1.kartPosition == 4 && kart2.kartPosition == 3 && kart3.kartPosition == 1 && kart4.kartPosition == 2)
                {
                    playerOnePosition.sprite = positionFour;
                    playerTwoPosition.sprite = positionThree;
                    playerThreePosition.sprite = positionOne;
                    playerFourPosition.sprite = positionTwo;
                }
                else if (kart1.kartPosition == 4 && kart2.kartPosition == 3 && kart3.kartPosition == 2 && kart4.kartPosition == 1)
                {
                    playerOnePosition.sprite = positionFour;
                    playerTwoPosition.sprite = positionThree;
                    playerThreePosition.sprite = positionTwo;
                    playerFourPosition.sprite = positionOne;
                }

                if (kart1.lapNumber == 1)
                {
                    playerOneLap.sprite = lapOne;
                }
                else if (kart1.lapNumber == 2)
                {
                    playerOneLap.sprite = lapTwo;
                }
                else if (kart1.lapNumber == 3)
                {
                    playerOneLap.sprite = lapThree;
                }

                if (kart2.lapNumber == 1)
                {
                    playerTwoLap.sprite = lapOne;
                }
                else if (kart2.lapNumber == 2)
                {
                    playerTwoLap.sprite = lapTwo;
                }
                else if (kart2.lapNumber == 3)
                {
                    playerTwoLap.sprite = lapThree;

                }
                if (kart3.lapNumber == 1)
                {
                    playerThreeLap.sprite = lapOne;
                }
                else if (kart3.lapNumber == 2)
                {
                    playerThreeLap.sprite = lapTwo;
                }
                else if (kart3.lapNumber == 3)
                {
                    playerThreeLap.sprite = lapThree;
                }

                if(kart4.lapNumber == 1)
                {
                    playerFourLap.sprite = lapOne;
                }
                else if(kart4.lapNumber == 2)
                {
                    playerFourLap.sprite = lapTwo;
                }
                else if(kart4.lapNumber == 3)
                {
                    playerFourLap.sprite = lapThree;
                }
                break;
        }
    }

    void positionCalculator(PlayerActor kart, GameObject go_kart, PlayerActor karttwo, GameObject go_karttwo)
    {
        if (kart.lapNumber > karttwo.lapNumber)
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 2;
        }
        else if (kart.lapNumber < karttwo.lapNumber)
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 1;
        }
        else if (kart.lapNumber == karttwo.lapNumber)
        {
            if (kart.checkPointCounter > karttwo.checkPointCounter)
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
            }
            else if (kart.checkPointCounter < karttwo.checkPointCounter)
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 1;
            }
            else if (kart.checkPointCounter == karttwo.checkPointCounter)
            {
                float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
                float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position);

                if (dist1 < dist2)
                {
                    kart.kartPosition = 1;
                    karttwo.kartPosition = 2;
                }
                else if (dist1 > dist2)
                {
                    kart.kartPosition = 2;
                    karttwo.kartPosition = 1;
                }
            }

        }
    }

    //Checks first kart against others.
    bool kartLapCheck(PlayerActor kart, PlayerActor karttwo, PlayerActor kartthree)
    {
        if ((kart.lapNumber > karttwo.lapNumber) && (karttwo.lapNumber > kartthree.lapNumber))
        {
            return true;
        }
        return false;
    }

    bool kartCheckpointCheck(PlayerActor kart, PlayerActor karttwo, PlayerActor kartthree)
    {
        if((kart.checkPointCounter > karttwo.checkPointCounter) && (karttwo.checkPointCounter > kartthree.checkPointCounter))
        {
            return true;
        }
        return false;
    }

    bool kartLapCheck(PlayerActor kart, PlayerActor karttwo, PlayerActor kartthree, PlayerActor kartfour)
    {
        if ((kart.lapNumber > karttwo.lapNumber) && (karttwo.lapNumber > kartthree.lapNumber)
            && (kartthree.lapNumber > kartfour.lapNumber))
        {
            return true;
        }
        return false;
    }



    bool kartCheckpointCheck(PlayerActor kart, PlayerActor karttwo, PlayerActor kartthree, PlayerActor kartfour)
    {
        if ((kart.checkPointCounter > karttwo.checkPointCounter) && (karttwo.checkPointCounter > kartthree.checkPointCounter)
            && (kartthree.checkPointCounter > kartfour.checkPointCounter))
        {
            return true;
        }
        return false;
    }

    void positionCalculator(PlayerActor kart, GameObject go_kart, PlayerActor karttwo, GameObject go_karttwo,
                            PlayerActor kartthree, GameObject go_kartthree)
    {

        if (kartCheckpointCheck(kart, karttwo, kartthree))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 3;
        }
        else if (kartCheckpointCheck(kart, kartthree, karttwo))
        {
            kart.kartPosition = 1;
            kartthree.kartPosition = 2;
            karttwo.kartPosition = 3;
        }
        else if (kartCheckpointCheck(karttwo, kart, kartthree))
        {
            karttwo.kartPosition = 1;
            kart.kartPosition = 2;
            kartthree.kartPosition = 3;
        }
        else if (kartCheckpointCheck(karttwo, kartthree, kart))
        {
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 2;
            kart.kartPosition = 3;
        }
        else if (kartCheckpointCheck(kartthree, kart, karttwo))
        {
            kartthree.kartPosition = 1;
            kart.kartPosition = 2;
            karttwo.kartPosition = 3;
        }
        else if (kartCheckpointCheck(kartthree, karttwo, kart))
        {
            kartthree.kartPosition = 1;
            karttwo.kartPosition = 2;
            kart.kartPosition = 3;
        }
        else if ((kart.checkPointCounter == karttwo.checkPointCounter) && (kartthree.checkPointCounter > karttwo.checkPointCounter))
        {

            kartthree.kartPosition = 1;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position);

            if ((dist1 > dist2))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 2;
            }
            else if ((dist1 < dist2))
            {
                karttwo.kartPosition = 3;
                kart.kartPosition = 2;
            }

        }
        else if ((kart.checkPointCounter == karttwo.checkPointCounter) && (kartthree.checkPointCounter < karttwo.checkPointCounter))
        {
            kartthree.kartPosition = 3;

            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position);

            if ((dist1 > dist2))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 1;
            }
            else if ((dist1 < dist2))
            {
                karttwo.kartPosition = 2;
                kart.kartPosition = 1;
            }
        }
        else if ((kart.checkPointCounter == kartthree.checkPointCounter) && (karttwo.checkPointCounter > kart.checkPointCounter))
        {
            karttwo.kartPosition = 1;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist1 > dist3)
            {
                kart.kartPosition = 3;
                kartthree.kartPosition = 2;
            }
            else if (dist1 < dist3)
            {
                kart.kartPosition = 2;
                kartthree.kartPosition = 3;
            }
        }
        else if ((kart.checkPointCounter == kartthree.checkPointCounter) && (karttwo.checkPointCounter < kart.checkPointCounter))
        {
            karttwo.kartPosition = 3;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist1 > dist3)
            {
                kart.kartPosition = 2;
                kartthree.kartPosition = 1;
            }
            else if (dist1 < dist3)
            {
                kart.kartPosition = 1;
                kartthree.kartPosition = 2;
            }
        }
        else if (karttwo.checkPointCounter == kartthree.checkPointCounter && (kart.checkPointCounter > karttwo.checkPointCounter))
        {
            kart.kartPosition = 1;
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist2 > dist3)
            {
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 2;
            }
            else if (dist2 < dist3)
            {
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 3;
            }

        }
        else if (karttwo.checkPointCounter == kartthree.checkPointCounter && (kart.checkPointCounter < karttwo.checkPointCounter))
        {
            kart.kartPosition = 3;
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist2 > dist3)
            {
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 1;
            }
            else if (dist2 < dist3)
            {
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 2;
            }

        }
        else if ((kart.checkPointCounter == karttwo.checkPointCounter) && (karttwo.checkPointCounter == kartthree.checkPointCounter))
        {
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position);

            if ((dist1 < dist2) && (dist2 < dist3))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 3;
            }
            else if ((dist1 < dist3) && (dist3 < dist2))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 2;
            }
            else if ((dist2 < dist1) && (dist1 < dist3))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 3;
            }
            else if ((dist2 < dist3) && (dist3 < dist1))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 2;
            }
            else if ((dist3 < dist1) && (dist1 < dist2))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 1;
            }
            else if ((dist3 < dist2) && (dist2 < dist1))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 1;
            }
        }


            ////Kart One
            //if (kartLapCheck(kart, karttwo, kartthree))
            //{
            //    kart.kartPosition = 1;
            //    karttwo.kartPosition = 2;
            //    kartthree.kartPosition = 3;
            //}
            //else if (kartLapCheck(kart, kartthree, karttwo))
            //{
            //    kart.kartPosition = 1;
            //    kartthree.kartPosition = 2;
            //    karttwo.kartPosition = 3;
            //}
            ////Kart Two.
            //else if (kartLapCheck(karttwo, kart, kartthree))
            //{
            //    karttwo.kartPosition = 1;
            //    kart.kartPosition = 2;
            //    kartthree.kartPosition = 3;
            //}
            //else if (kartLapCheck(karttwo, kartthree, kart))
            //{
            //    karttwo.kartPosition = 1;
            //    kartthree.kartPosition = 2;
            //    kart.kartPosition = 3;
            //}
            ////kart three
            //else if (kartLapCheck(kartthree, kart, karttwo))
            //{
            //    kartthree.kartPosition = 1;
            //    kart.kartPosition = 2;
            //    karttwo.kartPosition = 3;
            //}
            //else if (kartLapCheck(kartthree, karttwo, kart))
            //{
            //    kartthree.kartPosition = 1;
            //    karttwo.kartPosition = 2;
            //    kart.kartPosition = 3;
            //}
            //else if ((kart.lapNumber == karttwo.lapNumber) && (kart.lapNumber == kartthree.lapNumber) && (karttwo.lapNumber == kartthree.lapNumber))
            //{
            //    if(kartCheckpointCheck(kart, karttwo, kartthree))
            //    {
            //        kart.kartPosition = 1;
            //        karttwo.kartPosition = 2;
            //        kartthree.kartPosition = 3;
            //    }
            //    else if(kartCheckpointCheck(kart, kartthree, karttwo))
            //    {
            //        kart.kartPosition = 1;
            //        kartthree.kartPosition = 2;
            //        karttwo.kartPosition = 3;
            //    }
            //    else if(kartCheckpointCheck(karttwo, kart, kartthree))
            //    {
            //        karttwo.kartPosition = 1;
            //        kart.kartPosition = 2;
            //        kartthree.kartPosition = 3;
            //    }
            //    else if(kartCheckpointCheck(karttwo, kartthree, kart))
            //    {
            //        karttwo.kartPosition = 1;
            //        kartthree.kartPosition = 2;
            //        kart.kartPosition = 3;
            //    }
            //    else if(kartCheckpointCheck(kartthree, kart, karttwo))
            //    {
            //        kartthree.kartPosition = 1;
            //        kart.kartPosition = 2;
            //        karttwo.kartPosition = 3;
            //    }
            //    else if(kartCheckpointCheck(kartthree, karttwo, kart))
            //    {
            //        kartthree.kartPosition = 1;
            //        karttwo.kartPosition = 2;
            //        kart.kartPosition = 3;
            //    }
            //    else if ((kart.checkPointCounter == karttwo.checkPointCounter) && (karttwo.checkPointCounter == kartthree.checkPointCounter))
            //    {
            //        float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            //        float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position);
            //        float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position);

            //        if ((dist1 < dist2) && (dist2 < dist3))
            //        {
            //            kart.kartPosition = 1;
            //            karttwo.kartPosition = 2;
            //            kartthree.kartPosition = 3;
            //        }
            //        else if ((dist1 < dist3) && (dist3 < dist2))
            //        {
            //            kart.kartPosition = 1;
            //            karttwo.kartPosition = 3;
            //            kartthree.kartPosition = 2;
            //        }
            //        else if ((dist2 < dist1) && (dist1 < dist3))
            //        {
            //            kart.kartPosition = 2;
            //            karttwo.kartPosition = 1;
            //            kartthree.kartPosition = 3;
            //        }
            //        else if ((dist2 < dist3) && (dist3 < dist1))
            //        {
            //            kart.kartPosition = 3;
            //            karttwo.kartPosition = 1;
            //            kartthree.kartPosition = 2;
            //        }
            //        else if ((dist3 < dist1) && (dist1 < dist2))
            //        {
            //            kart.kartPosition = 2;
            //            karttwo.kartPosition = 3;
            //            kartthree.kartPosition = 1;
            //        }
            //        else if ((dist3 < dist2) && (dist2 < dist1))
            //        {
            //            kart.kartPosition = 3;
            //            karttwo.kartPosition = 2;
            //            kartthree.kartPosition = 1;
            //        }
            //    }
            //}
    }

    //Position calculator calculates each karts lap counter
    //
    //So I am aware this vv is terrible when i started it i was like "hey i don't need an array for 4 karts."
    //my bad.. but hey it works :D
    void positionCalculator(PlayerActor kart, GameObject go_kart, PlayerActor karttwo, GameObject go_karttwo,
                            PlayerActor kartthree, GameObject go_kartthree, PlayerActor kartfour, GameObject go_kartfour)
    {
        ////First player winning
        //if (kartLapCheck(kart, karttwo, kartthree, kartfour))
        //{
        //    kart.kartPosition = 1;
        //    karttwo.kartPosition = 2;
        //    kartthree.kartPosition = 3;
        //    kartfour.kartPosition = 4;
        //}
        //else if (kartLapCheck(kart, karttwo, kartfour, kartthree))
        //{
        //    kart.kartPosition = 1;
        //    karttwo.kartPosition = 2;
        //    kartthree.kartPosition = 4;
        //    kartfour.kartPosition = 3;
        //}
        //else if (kartLapCheck(kart, kartthree, karttwo, kartfour))
        //{
        //    kart.kartPosition = 1;
        //    karttwo.kartPosition = 3;
        //    kartthree.kartPosition = 2;
        //    kartfour.kartPosition = 4;
        //}
        //else if (kartLapCheck(kart, kartthree, kartfour, karttwo))
        //{
        //    kart.kartPosition = 1;
        //    karttwo.kartPosition = 4;
        //    kartthree.kartPosition = 2;
        //    kartfour.kartPosition = 3;
        //}
        //else if (kartLapCheck(kart, kartfour, kartthree, karttwo))
        //{
        //    kart.kartPosition = 1;
        //    karttwo.kartPosition = 4;
        //    kartthree.kartPosition = 3;
        //    kartfour.kartPosition = 2;
        //}
        //else if (kartLapCheck(kart, kartfour, karttwo, kartthree))
        //{
        //    kart.kartPosition = 1;
        //    karttwo.kartPosition = 3;
        //    kartthree.kartPosition = 4;
        //    kartfour.kartPosition = 2;
        //}
        ////Kart two
        //else if (kartLapCheck(karttwo, kart, kartthree, kartfour))
        //{
        //    kart.kartPosition = 2;
        //    karttwo.kartPosition = 1;
        //    kartthree.kartPosition = 3;
        //    kartfour.kartPosition = 4;
        //}
        //else if (kartLapCheck(karttwo, kart, kartfour, kartthree))
        //{
        //    kart.kartPosition = 2;
        //    karttwo.kartPosition = 1;
        //    kartthree.kartPosition = 4;
        //    kartfour.kartPosition = 3;
        //}
        //else if (kartLapCheck(karttwo, kartthree, kart, kartfour))
        //{
        //    kart.kartPosition = 3;
        //    karttwo.kartPosition = 1;
        //    kartthree.kartPosition = 2;
        //    kartfour.kartPosition = 4;
        //}
        //else if (kartLapCheck(karttwo, kartthree, kartfour, kart))
        //{
        //    kart.kartPosition = 4;
        //    karttwo.kartPosition = 1;
        //    kartthree.kartPosition = 2;
        //    kartfour.kartPosition = 3;
        //}
        //else if (kartLapCheck(karttwo, kartfour, kart, kartthree))
        //{
        //    kart.kartPosition = 3;
        //    karttwo.kartPosition = 1;
        //    kartthree.kartPosition = 4;
        //    kartfour.kartPosition = 2;
        //}
        //else if (kartLapCheck(karttwo, kartfour, kartthree, kart))
        //{
        //    kart.kartPosition = 4;
        //    karttwo.kartPosition = 1;
        //    kartthree.kartPosition = 3;
        //    kartfour.kartPosition = 2;
        //}
        ////Kart three
        //else if (kartLapCheck(kartthree, kart, karttwo, kartfour))
        //{
        //    kart.kartPosition = 2;
        //    karttwo.kartPosition = 3;
        //    kartthree.kartPosition = 1;
        //    kartfour.kartPosition = 4;
        //}
        //else if (kartLapCheck(kartthree, kart, kartfour, karttwo))
        //{
        //    kart.kartPosition = 2;
        //    karttwo.kartPosition = 4;
        //    kartthree.kartPosition = 1;
        //    kartfour.kartPosition = 3;
        //}
        //else if (kartLapCheck(kartthree, karttwo, kart, kartfour))
        //{
        //    kart.kartPosition = 3;
        //    karttwo.kartPosition = 2;
        //    kartthree.kartPosition = 1;
        //    kartfour.kartPosition = 4;
        //}
        //else if (kartLapCheck(kartthree, karttwo, kartfour, kart))
        //{
        //    kart.kartPosition = 4;
        //    karttwo.kartPosition = 2;
        //    kartthree.kartPosition = 1;
        //    kartfour.kartPosition = 3;
        //}
        //else if (kartLapCheck(kartthree, kartfour, kart, karttwo))
        //{
        //    kart.kartPosition = 3;
        //    karttwo.kartPosition = 4;
        //    kartthree.kartPosition = 1;
        //    kartfour.kartPosition = 2;
        //}
        //else if (kartLapCheck(kartthree, kartfour, karttwo, kart))
        //{
        //    kart.kartPosition = 4;
        //    karttwo.kartPosition = 3;
        //    kartthree.kartPosition = 1;
        //    kartfour.kartPosition = 2;
        //}
        ////Kart four
        //else if (kartLapCheck(kartfour, kart, karttwo, kartthree))
        //{
        //    kart.kartPosition = 2;
        //    karttwo.kartPosition = 3;
        //    kartthree.kartPosition = 4;
        //    kartfour.kartPosition = 1;
        //}
        //else if (kartLapCheck(kartfour, kart, kartthree, karttwo))
        //{
        //    kart.kartPosition = 2;
        //    karttwo.kartPosition = 4;
        //    kartthree.kartPosition = 3;
        //    kartfour.kartPosition = 1;
        //}
        //else if (kartLapCheck(kartfour, karttwo, kartthree, kart))
        //{
        //    kart.kartPosition = 4;
        //    karttwo.kartPosition = 2;
        //    kartthree.kartPosition = 3;
        //    kartfour.kartPosition = 1;
        //}
        //else if (kartLapCheck(kartfour, karttwo, kart, kartthree))
        //{
        //    kart.kartPosition = 3;
        //    karttwo.kartPosition = 2;
        //    kartthree.kartPosition = 4;
        //    kartfour.kartPosition = 1;
        //}
        //else if (kartLapCheck(kartfour, kartthree, kart, karttwo))
        //{
        //    kart.kartPosition = 3;
        //    karttwo.kartPosition = 4;
        //    kartthree.kartPosition = 2;
        //    kartfour.kartPosition = 1;
        //}
        //else if (kartLapCheck(kartfour, kartthree, karttwo, kart))
        //{
        //    kart.kartPosition = 4;
        //    karttwo.kartPosition = 3;
        //    kartthree.kartPosition = 2;
        //    kartfour.kartPosition = 1;
        //}

        //else if ((kart.lapNumber == karttwo.lapNumber) && (karttwo.lapNumber == kartthree.lapNumber) && (kartthree.lapNumber == kartfour.lapNumber))
        //{

        //First player winning
        if (kartCheckpointCheck(kart, karttwo, kartthree, kartfour))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 4;
        }
        else if (kartCheckpointCheck(kart, karttwo, kartfour, kartthree))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 3;
        }
        else if (kartCheckpointCheck(kart, kartthree, karttwo, kartfour))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 4;
        }
        else if (kartCheckpointCheck(kart, kartthree, kartfour, karttwo))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 3;
        }
        else if (kartCheckpointCheck(kart, kartfour, kartthree, karttwo))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 2;
        }
        else if (kartCheckpointCheck(kart, kartfour, karttwo, kartthree)) // 1 2 3 4
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 2;
        }
        //Kart two
        else if (kartCheckpointCheck(karttwo, kart, kartthree, kartfour))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 4;
        }
        else if (kartCheckpointCheck(karttwo, kart, kartfour, kartthree))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 3;
        }
        else if (kartCheckpointCheck(karttwo, kartthree, kart, kartfour))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 4;
        }
        else if (kartCheckpointCheck(karttwo, kartthree, kartfour, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 3;
        }
        else if (kartCheckpointCheck(karttwo, kartfour, kart, kartthree))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 2;
        }
        else if (kartCheckpointCheck(karttwo, kartfour, kartthree, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 2;
        }
        //Kart three
        else if (kartCheckpointCheck(kartthree, kart, karttwo, kartfour))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 4;
        }
        else if (kartCheckpointCheck(kartthree, kart, kartfour, karttwo))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 3;
        }
        else if (kartCheckpointCheck(kartthree, karttwo, kart, kartfour))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 4;
        }
        else if (kartCheckpointCheck(kartthree, karttwo, kartfour, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 3;
        }
        else if (kartCheckpointCheck(kartthree, kartfour, kart, karttwo))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 2;
        }
        else if (kartCheckpointCheck(kartthree, kartfour, karttwo, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 2;
        }
        //Kart four
        else if (kartCheckpointCheck(kartfour, kart, karttwo, kartthree))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 1;
        }
        else if (kartCheckpointCheck(kartfour, kart, kartthree, karttwo))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 1;
        }
        else if (kartCheckpointCheck(kartfour, karttwo, kartthree, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 1;
        }
        else if (kartCheckpointCheck(kartfour, karttwo, kart, kartthree))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 1;
        }
        else if (kartCheckpointCheck(kartfour, kartthree, kart, karttwo))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 1;
        }
        else if (kartCheckpointCheck(kartfour, kartthree, karttwo, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 1;
        }
        //if kart 1 and 2 are on the same checkpoint
        else if((kart.checkPointCounter == karttwo.checkPointCounter) && (kartthree.checkPointCounter > kartfour.checkPointCounter) &&
                (kartfour.checkPointCounter > karttwo.checkPointCounter))
        {
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 2;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);

            if(dist1 > dist2)
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 3;
            }
            else
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 4;
            }

        }
        //if kart 1 and 2 are on the same checkpoint.
        else if((kart.checkPointCounter == karttwo.checkPointCounter) && (kartthree.checkPointCounter < kartfour.checkPointCounter) &&
            (kartthree.checkPointCounter > karttwo.checkPointCounter))
        {
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 1;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);

            if (dist1 > dist2)
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 3;
            }
            else
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 4;
            }
        }   
        //if kart 1 and 3 are on the same checkpoint.
        else if((kart.checkPointCounter == kartthree.checkPointCounter) && (karttwo.checkPointCounter > kartfour.checkPointCounter) &&
            (kartfour.checkPointCounter > kartthree.checkPointCounter))
        {
            karttwo.kartPosition = 1;
            kartfour.kartPosition = 2;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist1 > dist3)
            {
                kart.kartPosition = 4;
                kartthree.kartPosition = 3;
            }
            else
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 4;
            }
        }
        //if kart 1 and 3 are on the same checkpoint.
        else if ((kart.checkPointCounter == kartthree.checkPointCounter) && (karttwo.checkPointCounter < kartfour.checkPointCounter) &&
          (karttwo.checkPointCounter > kartthree.checkPointCounter))
        {
            karttwo.kartPosition = 2;
            kartfour.kartPosition = 1;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist1 > dist3)
            {
                kart.kartPosition = 4;
                kartthree.kartPosition = 3;
            }
            else
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 4;
            }
        }
        //if kart 1 and 4 are on the same checkpoint
        else if ((kart.checkPointCounter == kartfour.checkPointCounter) && (karttwo.checkPointCounter > kartthree.checkPointCounter) &&
                 (kartthree.checkPointCounter > kartfour.checkPointCounter))
        {
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 2;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist1 > dist4)
            {
                kart.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else
            {
                kart.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
        }
        //if kart 1 and 4 are on the same checkpoint
        else if ((kart.checkPointCounter == kartfour.checkPointCounter) && (karttwo.checkPointCounter < kartthree.checkPointCounter) &&
                 (karttwo.checkPointCounter > kartfour.checkPointCounter))
        {
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 1;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist1 > dist4)
            {
                kart.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else
            {
                kart.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
        }
        //if kart 2 and 3 are on the same checkpoint
        else if ((karttwo.checkPointCounter == kartthree.checkPointCounter) && (kart.checkPointCounter > kartfour.checkPointCounter) &&
             (kartfour.checkPointCounter > kartthree.checkPointCounter))
        {
            kart.kartPosition = 1;
            kartfour.kartPosition = 2;
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist2 > dist3)
            {
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 3;
            }
            else
            {
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 4;
            }
        }
        //if kart 2 and 3 are on the same checkpoint
        else if ((karttwo.checkPointCounter == kartthree.checkPointCounter) && (kart.checkPointCounter < kartfour.checkPointCounter) &&
             (kart.checkPointCounter > kartthree.checkPointCounter))
        {
            kart.kartPosition = 2;
            kartfour.kartPosition = 1;
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist2 > dist3)
            {
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 3;
            }
            else
            {
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 4;
            }
        }
        //if kart 2 and 4 are on the same checkpoint
        else if ((karttwo.checkPointCounter == kartfour.checkPointCounter) && (kart.checkPointCounter > kartthree.checkPointCounter) &&
             (kartthree.checkPointCounter > kartfour.checkPointCounter))
        {
            kart.kartPosition = 1;
            kartthree.kartPosition = 2;
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist2 > dist4)
            {
                karttwo.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else
            {
                karttwo.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
        }
        //if kart 2 and 4 are on the same checkpoint
        else if ((karttwo.checkPointCounter == kartfour.checkPointCounter) && (kart.checkPointCounter < kartthree.checkPointCounter) &&
             (kart.checkPointCounter > kartfour.checkPointCounter))
        {
            kart.kartPosition = 2;
            kartthree.kartPosition = 1;
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist2 > dist4)
            {
                karttwo.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else
            {
                karttwo.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
        }
        //if kart 3 and 4 are on the same checkpoint
        else if ((kartthree.checkPointCounter == kartfour.checkPointCounter) && (kart.checkPointCounter > karttwo.checkPointCounter) &&
           (karttwo.checkPointCounter > kartfour.checkPointCounter))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 2;
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist3 > dist4)
            {
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else
            {
                karttwo.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
        }
        //if kart 3 and 4 are on the same checkpoint
        else if ((kartthree.checkPointCounter == kartfour.checkPointCounter) && (kart.checkPointCounter < karttwo.checkPointCounter) &&
           (kart.checkPointCounter > kartfour.checkPointCounter))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 1;
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist3 > dist4)
            {
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else
            {
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
        }
        /// SECOND LOT HAHAHA i wanna die.
         //if kart 1 and 2 are on the same checkpoint
        else if ((kart.checkPointCounter == karttwo.checkPointCounter) && (kartthree.checkPointCounter > kartfour.checkPointCounter) &&
                (kartfour.checkPointCounter < karttwo.checkPointCounter))
        {
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 4;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);

            if (dist1 > dist2)
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 1;
            }
            else
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
            }

        }
        //if kart 1 and 2 are on the same checkpoint.
        else if ((kart.checkPointCounter == karttwo.checkPointCounter) && (kartthree.checkPointCounter < kartfour.checkPointCounter) &&
            (kartthree.checkPointCounter < karttwo.checkPointCounter))
        {
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 3;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);

            if (dist1 > dist2)
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 1;
            }
            else
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
            }
        }
        //if kart 1 and 3 are on the same checkpoint.
        else if ((kart.checkPointCounter == kartthree.checkPointCounter) && (karttwo.checkPointCounter > kartfour.checkPointCounter) &&
            (kartfour.checkPointCounter < kartthree.checkPointCounter))
        {
            karttwo.kartPosition = 3;
            kartfour.kartPosition = 4;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist1 > dist3)
            {
                kart.kartPosition = 2;
                kartthree.kartPosition = 1;
            }
            else
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
            }
        }
        //if kart 1 and 3 are on the same checkpoint.
        else if ((kart.checkPointCounter == kartthree.checkPointCounter) && (karttwo.checkPointCounter < kartfour.checkPointCounter) &&
          (karttwo.checkPointCounter < kartthree.checkPointCounter))
        {
            karttwo.kartPosition = 4;
            kartfour.kartPosition = 3;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist1 > dist3)
            {
                kart.kartPosition = 2;
                kartthree.kartPosition = 1;
            }
            else
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
            }
        }
        //if kart 1 and 4 are on the same checkpoint
        else if ((kart.checkPointCounter == kartfour.checkPointCounter) && (karttwo.checkPointCounter > kartthree.checkPointCounter) &&
                 (kartthree.checkPointCounter < kartfour.checkPointCounter))
        {
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 4;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist1 > dist4)
            {
                kart.kartPosition = 2;
                kartfour.kartPosition = 1;
            }
            else
            {
                kart.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
        }
        //if kart 1 and 4 are on the same checkpoint
        else if ((kart.checkPointCounter == kartfour.checkPointCounter) && (karttwo.checkPointCounter < kartthree.checkPointCounter) &&
                 (karttwo.checkPointCounter < kartfour.checkPointCounter))
        {
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 3;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist1 > dist4)
            {
                kart.kartPosition = 2;
                kartfour.kartPosition = 1;
            }
            else
            {
                kart.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
        }
        //if kart 2 and 3 are on the same checkpoint
        else if ((karttwo.checkPointCounter == kartthree.checkPointCounter) && (kart.checkPointCounter > kartfour.checkPointCounter) &&
             (kartfour.checkPointCounter < kartthree.checkPointCounter))
        {
            kart.kartPosition = 3;
            kartfour.kartPosition = 4;
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist2 > dist3)
            {
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 1;
            }
            else
            {
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 2;
            }
        }
        //if kart 2 and 3 are on the same checkpoint
        else if ((karttwo.checkPointCounter == kartthree.checkPointCounter) && (kart.checkPointCounter < kartfour.checkPointCounter) &&
             (kart.checkPointCounter < kartthree.checkPointCounter))
        {
            kart.kartPosition = 4;
            kartfour.kartPosition = 3;
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);

            if (dist2 > dist3)
            {
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 1;
            }
            else
            {
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 2;
            }
        }
        //if kart 2 and 4 are on the same checkpoint
        else if ((karttwo.checkPointCounter == kartfour.checkPointCounter) && (kart.checkPointCounter > kartthree.checkPointCounter) &&
             (kartthree.checkPointCounter < kartfour.checkPointCounter))
        {
            kart.kartPosition = 3;
            kartthree.kartPosition = 4;
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist2 > dist4)
            {
                karttwo.kartPosition = 2;
                kartfour.kartPosition = 1;
            }
            else
            {
                karttwo.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
        }
        //if kart 2 and 4 are on the same checkpoint
        else if ((karttwo.checkPointCounter == kartfour.checkPointCounter) && (kart.checkPointCounter < kartthree.checkPointCounter) &&
             (kart.checkPointCounter < kartfour.checkPointCounter))
        {
            kart.kartPosition = 4;
            kartthree.kartPosition = 3;
            float dist2 = Vector3.Distance(go_kart2.transform.position, karttwo.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist2 > dist4)
            {
                karttwo.kartPosition = 2;
                kartfour.kartPosition = 1;
            }
            else
            {
                karttwo.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
        }
        //if kart 3 and 4 are on the same checkpoint
        else if ((kartthree.checkPointCounter == kartfour.checkPointCounter) && (kart.checkPointCounter > karttwo.checkPointCounter) &&
           (karttwo.checkPointCounter < kartfour.checkPointCounter))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 4;
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist3 > dist4)
            {
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 1;
            }
            else
            {
                karttwo.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
        }
        //if kart 3 and 4 are on the same checkpoint
        else if ((kartthree.checkPointCounter == kartfour.checkPointCounter) && (kart.checkPointCounter < karttwo.checkPointCounter) &&
           (kart.checkPointCounter < kartfour.checkPointCounter))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 3;
            float dist3 = Vector3.Distance(go_kart3.transform.position, kartthree.nextCheckPoint.transform.position);
            float dist4 = Vector3.Distance(go_kart4.transform.position, kartfour.nextCheckPoint.transform.position);

            if (dist3 > dist4)
            {
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 1;
            }
            else
            {
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
        }
        //if kart4 is greater than all the other karts and checkpoints are the same.
        else if ((kart.checkPointCounter == karttwo.checkPointCounter) && (karttwo.checkPointCounter == kartthree.checkPointCounter) &&
                (kartfour.checkPointCounter > kart.checkPointCounter))
        {
            kartfour.kartPosition = 1;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position); //Player 1
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position); // Player 2
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position); // Player 3

            if ((dist1 < dist2) && (dist2 < dist3))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 4;
            }
            else if ((dist1 < dist3) && (dist3 < dist2))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 3;
            }
            else if ((dist2 < dist1) && (dist1 < dist3))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 4;
            }
            else if ((dist2 < dist3) && (dist3 < dist1))
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 3;
            }
            else if ((dist3 < dist1) && (dist1 < dist2))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 2;
            }
            else if ((dist3 < dist2) && (dist2 < dist1))
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 2;
            }
        }
        //if kart 4 is less then all the other karts and all the other kart checkpoints are the same
        else if ((kart.checkPointCounter == karttwo.checkPointCounter) && (karttwo.checkPointCounter == kartthree.checkPointCounter) &&
             (kartfour.checkPointCounter < kart.checkPointCounter))
        {
            kartfour.kartPosition = 4;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position); //Player 1
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position); // Player 2
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position); // Player 3

            if ((dist1 < dist2) && (dist2 < dist3))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 3;
            }
            else if ((dist1 < dist3) && (dist3 < dist2))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 2;
            }
            else if ((dist2 < dist1) && (dist1 < dist3))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 3;
            }
            else if ((dist2 < dist3) && (dist3 < dist1))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 2;
            }
            else if ((dist3 < dist1) && (dist1 < dist2))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 1;
            }
            else if ((dist3 < dist2) && (dist2 < dist1))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 1;
            }
        }
        //If kart one is greater then all the other karts and all the other karts are on the same checkpoint counter.
        else if ((kart.checkPointCounter > karttwo.checkPointCounter) && (karttwo.checkPointCounter == kartthree.checkPointCounter) &&
                (kartthree.checkPointCounter == kartfour.checkPointCounter))
        {
            kart.kartPosition = 1;
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position); // Player 2
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position); // Player 3
            float dist4 = Vector3.Distance(go_kartfour.transform.position, kartfour.nextCheckPoint.transform.position); // Player 4

            if ((dist2 < dist3) && (dist3 < dist4))
            {
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
            else if ((dist2 < dist4) && (dist4 < dist3))
            {
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else if ((dist3 < dist2) && (dist2 < dist4))
            {
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 4;
            }
            else if ((dist3 < dist4) && (dist4 < dist2))
            {
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 3;
            }
            else if ((dist4 < dist2) && (dist2 < dist3))
            {
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 2;
            }
            else if ((dist4 < dist3) && (dist3 < dist2))
            {
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 2;
            }
        }
        //if kart one is less then all the other karts and all the other karts are on the same checkpoint counter
        else if ((kart.checkPointCounter < karttwo.checkPointCounter) && (karttwo.checkPointCounter == kartthree.checkPointCounter) &&
               (kartthree.checkPointCounter == kartfour.checkPointCounter))
        {
            kart.kartPosition = 4;
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position); // Player 2
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position); // Player 3
            float dist4 = Vector3.Distance(go_kartfour.transform.position, kartfour.nextCheckPoint.transform.position); // Player 4

            if ((dist2 < dist3) && (dist3 < dist4))
            {
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 3;
            }
            else if ((dist2 < dist4) && (dist4 < dist3))
            {
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 2;
            }
            else if ((dist3 < dist2) && (dist2 < dist4))
            {
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 3;
            }
            else if ((dist3 < dist4) && (dist4 < dist2))
            {
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
            else if ((dist4 < dist2) && (dist2 < dist3))
            {
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 1;
            }
            else if ((dist4 < dist3) && (dist3 < dist2))
            {
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 1;
            }
        }
        //Kart two is less then all the other karts and all other karts are on the same checkpoint.
        else if ((karttwo.checkPointCounter < kart.checkPointCounter) && (kart.checkPointCounter == kartthree.checkPointCounter) &&
              (kartthree.checkPointCounter == kartfour.checkPointCounter))
        {
            karttwo.kartPosition = 4;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position); // Player 2
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position); // Player 3
            float dist4 = Vector3.Distance(go_kartfour.transform.position, kartfour.nextCheckPoint.transform.position); // Player 4

            if ((dist1 < dist3) && (dist3 < dist4))
            {
                kart.kartPosition = 1;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 3;
            }
            else if ((dist1 < dist4) && (dist4 < dist3))
            {
                kart.kartPosition = 1;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 2;
            }
            else if ((dist3 < dist1) && (dist1 < dist4))
            {
                kart.kartPosition = 2;
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 3;
            }
            else if ((dist3 < dist4) && (dist4 < dist1))
            {
                kart.kartPosition = 3;
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
            else if ((dist4 < dist1) && (dist1 < dist3))
            {
                kart.kartPosition = 2;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 1;
            }
            else if ((dist4 < dist3) && (dist3 < dist1))
            {
                kart.kartPosition = 3;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 1;
            }
        }
        //if kart two is greater then all the other karts and all other karts are on the same checkpoint
        else if ((karttwo.checkPointCounter > kart.checkPointCounter) && (kart.checkPointCounter == kartthree.checkPointCounter) &&
             (kartthree.checkPointCounter == kartfour.checkPointCounter))
        {
            karttwo.kartPosition = 1;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position); // Player 2
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position); // Player 3
            float dist4 = Vector3.Distance(go_kartfour.transform.position, kartfour.nextCheckPoint.transform.position); // Player 4

            if ((dist1 < dist3) && (dist3 < dist4))
            {
                kart.kartPosition = 2;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
            else if ((dist1 < dist4) && (dist4 < dist3))
            {
                kart.kartPosition = 2;
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else if ((dist3 < dist1) && (dist1 < dist4))
            {
                kart.kartPosition = 3;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 4;
            }
            else if ((dist3 < dist4) && (dist4 < dist1))
            {
                kart.kartPosition = 4;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 3;
            }
            else if ((dist4 < dist1) && (dist1 < dist3))
            {
                kart.kartPosition = 3;
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 2;
            }
            else if ((dist4 < dist3) && (dist3 < dist1))
            {
                kart.kartPosition = 4;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 2;
            }
        }
        //Kart three is less then all the other karts and all other karts are on the same checkpoint.
        else if ((kartthree.checkPointCounter < kart.checkPointCounter) && (kart.checkPointCounter == karttwo.checkPointCounter) &&
              (karttwo.checkPointCounter == kartfour.checkPointCounter))
        {
            kartthree.kartPosition = 4;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position); // Player 2
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position); // Player 3
            float dist4 = Vector3.Distance(go_kartfour.transform.position, kartfour.nextCheckPoint.transform.position); // Player 4

            if ((dist1 < dist2) && (dist2 < dist4))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
                kartfour.kartPosition = 3;
            }
            else if ((dist1 < dist4) && (dist4 < dist2))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 3;
                kartfour.kartPosition = 2;
            }
            else if ((dist2 < dist1) && (dist1 < dist4))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 1;
                kartfour.kartPosition = 3;
            }
            else if ((dist2 < dist4) && (dist4 < dist1))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
            else if ((dist4 < dist1) && (dist1 < dist2))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 3;
                kartfour.kartPosition = 1;
            }
            else if ((dist4 < dist2) && (dist2 < dist1))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 2;
                kartfour.kartPosition = 1;
            }
        }
        //Kart three is greater then all the other karts and all other karts are on the same checkpoint.
        else if ((kartthree.checkPointCounter > kart.checkPointCounter) && (kart.checkPointCounter == karttwo.checkPointCounter) &&
              (karttwo.checkPointCounter == kartfour.checkPointCounter))
        {
            kartthree.kartPosition = 1;
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position); // Player 2
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position); // Player 3
            float dist4 = Vector3.Distance(go_kartfour.transform.position, kartfour.nextCheckPoint.transform.position); // Player 4

            if ((dist1 < dist2) && (dist2 < dist4))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
            else if ((dist1 < dist4) && (dist4 < dist2))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else if ((dist2 < dist1) && (dist1 < dist4))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 2;
                kartfour.kartPosition = 4;
            }
            else if ((dist2 < dist4) && (dist4 < dist1))
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 2;
                kartfour.kartPosition = 3;
            }
            else if ((dist4 < dist1) && (dist1 < dist2))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 4;
                kartfour.kartPosition = 2;
            }
            else if ((dist4 < dist2) && (dist2 < dist1))
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 3;
                kartfour.kartPosition = 2;
            }
        }
        //If all karts are on the same checkpoint.
        else if ((kart.checkPointCounter == karttwo.checkPointCounter) && (karttwo.checkPointCounter == kartthree.checkPointCounter)
                    && (kartthree.checkPointCounter == kartfour.checkPointCounter))
        {
            float dist1 = Vector3.Distance(go_kart.transform.position, kart.nextCheckPoint.transform.position); //Player 1
            float dist2 = Vector3.Distance(go_karttwo.transform.position, karttwo.nextCheckPoint.transform.position); // Player 2
            float dist3 = Vector3.Distance(go_kartthree.transform.position, kartthree.nextCheckPoint.transform.position); // Player 3
            float dist4 = Vector3.Distance(go_kartfour.transform.position, kartfour.nextCheckPoint.transform.position); // Player 4

            // First            second        third        fourth
            if ((dist1 < dist2) && (dist2 < dist3) && (dist3 < dist4))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
            else if ((dist1 < dist3) && (dist3 < dist2) && (dist2 < dist4))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 4;
            }
            else if ((dist1 < dist2) && (dist2 < dist4) && (dist4 < dist3))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else if ((dist1 < dist3) && (dist3 < dist4) && (dist4 < dist2))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 3;
            }
            else if ((dist1 < dist4) && (dist4 < dist2) && (dist2 < dist3))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 2;
            }
            else if ((dist1 < dist4) && (dist4 < dist3) && (dist3 < dist2))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 2;
            }
            //Kart2
            else if ((dist2 < dist1) && (dist1 < dist3) && (dist3 < dist4))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 4;
            }
            else if ((dist2 < dist1) && (dist1 < dist4) && (dist4 < dist3))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 3;
            }
            else if ((dist2 < dist3) && (dist3 < dist1) && (dist1 < dist4))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 4;
            }
            else if ((dist2 < dist3) && (dist3 < dist4) && (dist4 < dist1))
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 3;
            }
            else if ((dist2 < dist4) && (dist4 < dist1) && (dist1 < dist3))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 2;
            }
            else if ((dist2 < dist4) && (dist4 < dist3) && (dist3 < dist1))
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 2;
            }
            //Kart3
            else if ((dist3 < dist1) && (dist1 < dist2) && (dist2 < dist4))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 4;
            }
            else if ((dist3 < dist1) && (dist1 < dist4) && (dist4 < dist2))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 3;
            }
            else if ((dist3 < dist2) && (dist2 < dist1) && (dist1 < dist4))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 4;
            }
            else if ((dist3 < dist2) && (dist2 < dist4) && (dist4 < dist1))
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 3;
            }
            else if ((dist3 < dist4) && (dist4 < dist2) && (dist2 < dist1))
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
            else if ((dist3 < dist4) && (dist4 < dist1) && (dist1 < dist2))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 1;
                kartfour.kartPosition = 2;
            }
            //Kart4
            else if ((dist4 < dist1) && (dist1 < dist2) && (dist2 < dist3))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 1;
            }
            else if ((dist4 < dist1) && (dist1 < dist3) && (dist3 < dist2))
            {
                kart.kartPosition = 2;
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 1;
            }
            else if ((dist4 < dist2) && (dist2 < dist1) && (dist1 < dist3))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 4;
                kartfour.kartPosition = 1;
            }
            else if ((dist4 < dist2) && (dist2 < dist3) && (dist3 < dist1))
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 3;
                kartfour.kartPosition = 1;
            }
            else if ((dist4 < dist3) && (dist3 < dist2) && (dist2 < dist1))
            {
                kart.kartPosition = 4;
                karttwo.kartPosition = 3;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 1;
            }
            else if ((dist4 < dist3) && (dist3 < dist1) && (dist1 < dist2))
            {
                kart.kartPosition = 3;
                karttwo.kartPosition = 4;
                kartthree.kartPosition = 2;
                kartfour.kartPosition = 1;
            }

        }

        }
    //So I am aware this ^^ is terrible when i started it i was like "hey i don't need an array for 4 karts."
    //my bad.. but hey it works :D
}
