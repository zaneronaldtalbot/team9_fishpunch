﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour {

    private PlayerSelectActor psActor;

    private KartActor2 kart1, kart2, kart3, kart4;
    private GameObject go_kart1, go_kart2, go_kart3, go_kart4;

    // Use this for initialization
    void Start() {
        psActor = GetComponent<PlayerSelectActor>();

        switch (psActor.playerCount)
        {
            case 2:
                go_kart1 = GameObject.Find("PlayerCharacter_001");
                kart1 = go_kart1.GetComponent<KartActor2>();
                go_kart2 = GameObject.Find("PlayerCharacter_002");
                kart2 = go_kart2.GetComponent<KartActor2>();
                break;
            case 3:
                go_kart1 = GameObject.Find("PlayerCharacter_001");
                kart1 = go_kart1.GetComponent<KartActor2>();
                go_kart2 = GameObject.Find("PlayerCharacter_002");
                kart2 = go_kart2.GetComponent<KartActor2>();
                go_kart3 = GameObject.Find("PlayerCharacter_003");
                kart3 = go_kart3.GetComponent<KartActor2>();

                break;
            case 4:
                go_kart1 = GameObject.Find("PlayerCharacter_001");
                kart1 = go_kart1.GetComponent<KartActor2>();
                go_kart2 = GameObject.Find("PlayerCharacter_002");
                kart2 = go_kart2.GetComponent<KartActor2>();
                go_kart3 = GameObject.Find("PlayerCharacter_003");
                kart3 = go_kart3.GetComponent<KartActor2>();
                go_kart4 = GameObject.Find("PlayerCharacter_004");
                kart4 = go_kart4.GetComponent<KartActor2>();
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
                break;
            case 3:
                positionCalculator(kart1, go_kart1, kart2, go_kart2, kart3, go_kart3);
                break;
            case 4:
                positionCalculator(kart1, go_kart1, kart2, go_kart2, kart3, go_kart3, kart4, go_kart4);
                break;
        }
  

    }

    void positionCalculator(KartActor2 kart, GameObject go_kart, KartActor2 karttwo, GameObject go_karttwo)
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
    bool kartLapCheck(KartActor2 kart, KartActor2 karttwo, KartActor2 kartthree)
    {
        if ((kart.lapNumber > karttwo.lapNumber) && (karttwo.lapNumber > kartthree.lapNumber))
        {
            return true;
        }
        return false;
    }

    bool kartCheckpointCheck(KartActor2 kart, KartActor2 karttwo, KartActor2 kartthree)
    {
        if((kart.checkPointCounter > karttwo.checkPointCounter) && (karttwo.checkPointCounter > kartthree.checkPointCounter))
        {
            return true;
        }
        return false;
    }

    bool kartLapCheck(KartActor2 kart, KartActor2 karttwo, KartActor2 kartthree, KartActor2 kartfour)
    {
        if ((kart.lapNumber > karttwo.lapNumber) && (karttwo.lapNumber > kartthree.lapNumber)
            && (kartthree.lapNumber > kartfour.lapNumber))
        {
            return true;
        }
        return false;
    }



    bool kartCheckpointCheck(KartActor2 kart, KartActor2 karttwo, KartActor2 kartthree, KartActor2 kartfour)
    {
        if ((kart.checkPointCounter > karttwo.checkPointCounter) && (karttwo.checkPointCounter > kartthree.checkPointCounter)
            && (kartthree.checkPointCounter > kartfour.checkPointCounter))
        {
            return true;
        }
        return false;
    }

    void positionCalculator(KartActor2 kart, GameObject go_kart, KartActor2 karttwo, GameObject go_karttwo,
                            KartActor2 kartthree, GameObject go_kartthree)
    {

        if (kartLapCheck(kart, karttwo, kartthree))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 3;
        }
        else if (kartLapCheck(kart, kartthree, karttwo))
        {
            kart.kartPosition = 1;
            kartthree.kartPosition = 2;
            karttwo.kartPosition = 3;
        }
        else if (kartLapCheck(karttwo, kart, kartthree))
        {
            karttwo.kartPosition = 1;
            kart.kartPosition = 2;
            kartthree.kartPosition = 3;
        }
        else if (kartLapCheck(karttwo, kartthree, kart))
        {
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 2;
            kart.kartPosition = 3;
        }
        else if (kartLapCheck(kartthree, kart, karttwo))
        {
            kartthree.kartPosition = 1;
            kart.kartPosition = 2;
            karttwo.kartPosition = 3;
        }
        else if (kartLapCheck(kartthree, karttwo, kart))
        {
            kartthree.kartPosition = 1;
            karttwo.kartPosition = 2;
            kart.kartPosition = 3;
        }
        else if ((kart.lapNumber == karttwo.lapNumber) && (kart.lapNumber == kartthree.lapNumber) && (karttwo.lapNumber == kartthree.lapNumber))
        {
            if(kartCheckpointCheck(kart, karttwo, kartthree))
            {
                kart.kartPosition = 1;
                karttwo.kartPosition = 2;
                kartthree.kartPosition = 3;
            }
            else if(kartCheckpointCheck(kart, kartthree, karttwo))
            {
                kart.kartPosition = 1;
                kartthree.kartPosition = 2;
                karttwo.kartPosition = 3;
            }
            else if(kartCheckpointCheck(karttwo, kart, kartthree))
            {
                karttwo.kartPosition = 1;
                kart.kartPosition = 2;
                kartthree.kartPosition = 3;
            }
            else if(kartCheckpointCheck(karttwo, kartthree, kart))
            {
                karttwo.kartPosition = 1;
                kartthree.kartPosition = 2;
                kart.kartPosition = 3;
            }
            else if(kartCheckpointCheck(kartthree, kart, karttwo))
            {
                kartthree.kartPosition = 1;
                kart.kartPosition = 2;
                karttwo.kartPosition = 3;
            }
            else if(kartCheckpointCheck(kartthree, karttwo, kart))
            {
                kartthree.kartPosition = 1;
                karttwo.kartPosition = 2;
                kart.kartPosition = 3;
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

        }


    }

    //Position calculator calculates each karts lap counter
    //
    void positionCalculator(KartActor2 kart, GameObject go_kart, KartActor2 karttwo, GameObject go_karttwo,
                            KartActor2 kartthree, GameObject go_kartthree, KartActor2 kartfour, GameObject go_kartfour)
    {
        //First player winning
        if (kartLapCheck(kart, karttwo, kartthree, kartfour))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 4;
        }
        else if (kartLapCheck(kart, karttwo, kartfour, kartthree))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 3;
        }
        else if (kartLapCheck(kart, kartthree, karttwo, kartfour))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 4;
        }
        else if (kartLapCheck(kart, kartthree, kartfour, karttwo))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 3;
        }
        else if (kartLapCheck(kart, kartfour, kartthree, karttwo))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 2;
        }
        else if (kartLapCheck(kart, kartfour, karttwo, kartthree))
        {
            kart.kartPosition = 1;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 2;
        }
        //Kart two
        else if (kartLapCheck(karttwo, kart, kartthree, kartfour))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 4;
        }
        else if (kartLapCheck(karttwo, kart, kartfour, kartthree))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 3;
        }
        else if (kartLapCheck(karttwo, kartthree, kart, kartfour))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 4;
        }
        else if (kartLapCheck(karttwo, kartthree, kartfour, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 3;
        }
        else if (kartLapCheck(karttwo, kartfour, kart, kartthree))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 2;
        }
        else if (kartLapCheck(karttwo, kartfour, kartthree, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 1;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 2;
        }
        //Kart three
        else if (kartLapCheck(kartthree, kart, karttwo, kartfour))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 4;
        }
        else if (kartLapCheck(kartthree, kart, kartfour, karttwo))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 3;
        }
        else if (kartLapCheck(kartthree, karttwo, kart, kartfour))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 4;
        }
        else if (kartLapCheck(kartthree, karttwo, kartfour, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 3;
        }
        else if (kartLapCheck(kartthree, kartfour, kart, karttwo))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 2;
        }
        else if (kartLapCheck(kartthree, kartfour, karttwo, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 1;
            kartfour.kartPosition = 2;
        }
        //Kart four
        else if (kartLapCheck(kartfour, kart, karttwo, kartthree))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 1;
        }
        else if (kartLapCheck(kartfour, kart, kartthree, karttwo))
        {
            kart.kartPosition = 2;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 1;
        }
        else if (kartLapCheck(kartfour, karttwo, kartthree, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 3;
            kartfour.kartPosition = 1;
        }
        else if (kartLapCheck(kartfour, karttwo, kart, kartthree))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 2;
            kartthree.kartPosition = 4;
            kartfour.kartPosition = 1;
        }
        else if (kartLapCheck(kartfour, kartthree, kart, karttwo))
        {
            kart.kartPosition = 3;
            karttwo.kartPosition = 4;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 1;
        }
        else if (kartLapCheck(kartfour, kartthree, karttwo, kart))
        {
            kart.kartPosition = 4;
            karttwo.kartPosition = 3;
            kartthree.kartPosition = 2;
            kartfour.kartPosition = 1;
        }
        else if ((kart.lapNumber == karttwo.lapNumber) && (karttwo.lapNumber == kartthree.lapNumber) && (kartthree.lapNumber == kartfour.lapNumber))
        {

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
            else if (kartCheckpointCheck(kart, kartfour, karttwo, kartthree))
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
                if ((dist2 < dist1) && (dist1 < dist3) && (dist3 < dist4))
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
                if ((dist3 < dist1) && (dist1 < dist2) && (dist2 < dist4))
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
                if ((dist4 < dist1) && (dist1 < dist2) && (dist2 < dist3))
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
    }
}
