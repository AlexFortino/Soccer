﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    [Header("Scoreboard Values")]	
    public float gameTimer = 300.0f;
    public GameClock gameClock;
    public Score awayScore;
    public Score homeScore;
    public int numberOfPlayers = 0;
    [HideInInspector]
    int aScore = 0;
    [HideInInspector]
    int hScore = 0;    

    Goal awayGoal;
    Goal homeGoal;

    [Header("Field Positions")]
    public Transform awayStart;
    public Transform homeStart;
    public Transform ballStart;

    [Header("Prefabs")]
    public PlayerController playerController;
    public GameObject soccerBall;
    public GameObject homePlayer;
    public GameObject awayPlayer;

    public List<GameObject> players = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (homePlayer != null){
            // PhotonNetwork.Instantiate(homePlayer.name, new Vector3(0, 5, 0), Quaternion.identity, 0);
            CheckPlayers();
            Debug.Log(numberOfPlayers);

            SpawnPlayers();
        }

        awayGoal = GameObject.Find("/Field/Field Lines/Away Goal/Trigger").GetComponent<Goal>();
        homeGoal = GameObject.Find("/Field/Field Lines/Home Goal/Trigger").GetComponent<Goal>();


        awayGoal.GoalScored += Goal;
        homeGoal.GoalScored += Goal;
    }
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void CheckPlayers()
    {
        numberOfPlayers = PhotonNetwork.CountOfPlayers;
        // 
    }
    //PhotonNetwork.Instantiate(homePlayer.name, homeStart.position, Quaternion.identity, 0);
    public void SpawnPlayers()
    {
        switch (numberOfPlayers)
        {
            case 1:
                players.Add((GameObject)PhotonNetwork.Instantiate(homePlayer.name, homeStart.position, Quaternion.identity, 0));
                //PhotonNetwork.Instantiate(homePlayer.name, homeStart.position, Quaternion.identity, 0);
                //numberOfPlayers = 2;
                Debug.Log(players[0].name);
                break;
            case 2:
                players.Add((GameObject)PhotonNetwork.Instantiate(homePlayer.name, awayStart.position, Quaternion.identity, 0));
                //PhotonNetwork.Instantiate(homePlayer.name, homeStart.position, Quaternion.identity, 0);
              //  numberOfPlayers = 3;
                break;
            case 3:
                PhotonNetwork.Instantiate(homePlayer.name, homeStart.position, Quaternion.identity, 0);
                PhotonNetwork.Instantiate(homePlayer.name, awayStart.position, Quaternion.identity, 0);
                PhotonNetwork.Instantiate(homePlayer.name, homeStart.position, Quaternion.identity, 0);
                numberOfPlayers = 4;
                break;
            case 4:
                PhotonNetwork.Instantiate(homePlayer.name, homeStart.position, Quaternion.identity, 0);
                PhotonNetwork.Instantiate(homePlayer.name, awayStart.position, Quaternion.identity, 0);
                PhotonNetwork.Instantiate(homePlayer.name, homeStart.position, Quaternion.identity, 0);
                PhotonNetwork.Instantiate(homePlayer.name, awayStart.position, Quaternion.identity, 0);
                numberOfPlayers = 1;
                break;
            default:
                Debug.Log("UHH TOO MANY PLAYERS");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer -= Time.deltaTime;
        gameClock.DisplayTime(gameTimer);

        if(gameTimer <= 0.0f)
        {
            GameOver();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }
    
    public void Scored()
    {

    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient) {
            Debug.LogError("Maste client not found");
        }

        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
           // LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
               if (PhotonNetwork.IsMasterClient)
        {
            LoadArena();
        } 
    }

    void Goal(string team)
    {
        UpdateScore(team);
        ResetField();
    }

    void UpdateScore(string team)
    {
        if(team == "Away")
        {
            hScore++;
            homeScore.DisplayScore(hScore);
        }

        if (team == "Home")
        {
            aScore++;
            awayScore.DisplayScore(aScore);
        }
    }

    void ResetField()
    {
        GameObject player, ai, ball;
        player = GameObject.FindGameObjectWithTag("Player");
        ai = GameObject.FindGameObjectWithTag("AI");
        ball = GameObject.FindGameObjectWithTag("Ball");



        players[0].transform.position = homeStart.position;
        players[0].GetComponentInChildren<PlayerController>().StopKick();

        if (players.Count == 2)
        {
            players[1].transform.position = awayStart.position;
            players[1].GetComponentInChildren<PlayerController>().StopKick();
        }
        if (players.Count == 3)

        {
            players[2].transform.position = homeStart.position;
            players[2].GetComponentInChildren<PlayerController>().StopKick();
        }
        if (players.Count == 4)
        {
            players[3].transform.position = awayStart.position;
            players[3].GetComponentInChildren<PlayerController>().StopKick();
        }

            ball.transform.position = ballStart.position;

        /*
        Destroy(player);
        Destroy(ai);
        Destroy(ball);

        player = Instantiate(homePlayer, homeStart.position, Quaternion.identity);
        ai = Instantiate(awayPlayer, awayStart.position, Quaternion.identity);
        ball = Instantiate(soccerBall, ballStart.position, Quaternion.identity);
        
        playerController.player = player.GetComponentInChildren<Pawn>();
        */

    }

    void GameOver()
    {
        if (aScore > hScore)
        {
            SceneManager.LoadScene(2);
        }
        else if (hScore > aScore)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
    }
}
