using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    private byte maxPlayersPerRoom = 4;

    string gameVersion = "1.0.0";

    bool isConnecting;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;    
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected == true)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    //ADD extra functions!

    public override void OnJoinRandomFailed(short code, string msg) {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()    {
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting) {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected Due To: " + cause);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
