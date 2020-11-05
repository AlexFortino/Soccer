using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        if (homePlayer != null){
            PhotonNetwork.Instantiate(homePlayer.name, new Vector3(0, 5, 0), Quaternion.identity, 0);
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

        Destroy(player);
        Destroy(ai);
        Destroy(ball);

        player = Instantiate(homePlayer, homeStart.position, Quaternion.identity);
        ai = Instantiate(awayPlayer, awayStart.position, Quaternion.identity);
        ball = Instantiate(soccerBall, ballStart.position, Quaternion.identity);

        playerController.player = player.GetComponentInChildren<Pawn>();
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
