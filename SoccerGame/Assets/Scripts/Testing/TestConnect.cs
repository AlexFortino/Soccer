using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class TestConnect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting To Server");
        PhotonNetwork.GameVersion = "1.0.0";
      //  PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Sever");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Player Left:" + cause.ToString());

    }


}
