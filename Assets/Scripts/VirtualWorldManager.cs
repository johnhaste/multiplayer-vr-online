using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VirtualWorldManager : MonoBehaviourPunCallbacks
{

    public static VirtualWorldManager instance;

    private void Awake()
    {
        if(instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LeaveRoomAndLoadHomeScene()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("HomeScene");
    }

    #region Photon Callback Methods

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Alerts when a new player entered the room
        print(newPlayer.NickName +" joined the room. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    
    #endregion
}
