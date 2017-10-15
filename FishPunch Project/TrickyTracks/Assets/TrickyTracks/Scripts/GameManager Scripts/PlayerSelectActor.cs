﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerSelectActor : MonoBehaviour
{
    private GamePadManager gpManager;
    private GameObject manager;

    [HideInInspector]
    public int playerCount = 0;

    public GameObject player1Ready, player2Ready, player3Ready, player4Ready;

    public GameObject go_timer;

    bool playerReady1 = false;
    bool playerReady2 = false;
    bool playerReady3 = false;
    bool playerReady4 = false;

    public float timer = 60.0f;

    private Text textTimer;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("Manager");
        gpManager = manager.GetComponent<GamePadManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0.0f)
        {
            SceneManager.LoadScene("MainLevel001");
        }

        switch (gpManager.ConnectedTotal())
        {
            case 1:
                if (playerReady1)
                {
                    SceneManager.LoadScene("MainLevel001");
                    this.enabled = false;
                }
                break;
            case 2:
                if (playerReady1 && playerReady2)
                {
                    SceneManager.LoadScene("MainLevel001");
                }
                break;
            case 3:
                if (playerReady1 && playerReady2 && playerReady3)
                {
                    SceneManager.LoadScene("MainLevel001");
                }
                break;
            case 4:
                if (playerReady1 && playerReady2 && playerReady3 && playerReady4)
                {
                    SceneManager.LoadScene("MainLevel001");
                }
                break;
            default:
                break;
        }

        timer -= Time.deltaTime;

         textTimer =  go_timer.GetComponent<Text>();

        int intTimer = (int)timer;
        textTimer.text = "Time Left: " + intTimer.ToString();
    
        for (int i = 1; i <= 4; ++i)
        {
            startPressed(i);
        }

     
      
    }

    void startPressed(int index)
    {
        xbox_gamepad gamepad;
        gamepad = gpManager.GetGamePad(index);


            if (gamepad.GetButtonDown("Start"))
            {


            switch (index)
            {
                case 0:
                    break;
                case 1:
                    if (!playerReady1)
                    {
                        Destroy(GameObject.Find("Player1JoinText"));
                        player1Ready.SetActive(true);
                        playerCount++;
                        playerReady1 = true;
                    }
                    break;
                case 2:
                    if (!playerReady2)
                    {
                        Destroy(GameObject.Find("Player2JoinText"));
                        player2Ready.SetActive(true);
                        playerCount++;
                        playerReady2 = true;
                    }

                    break;
                case 3:
                    if (!playerReady3)
                    {
                        Destroy(GameObject.Find("Player3JoinText"));
                        player3Ready.SetActive(true);
                        playerCount++;
                        playerReady3 = true;
                    }

                    break;
                case 4:
                    if (!playerReady4)
                    {
                        Destroy(GameObject.Find("Player4JoinText"));
                        player4Ready.SetActive(true);
                        playerCount++;
                        playerReady4 = true;
                    }

                    break;
                    default:
                        break;     
            }
        }
    }
}
