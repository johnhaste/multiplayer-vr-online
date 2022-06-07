using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VirtualWorldManager : MonoBehaviourPunCallbacks
{
    #region Photon Callback Methods

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Alerts when a new player entered the room
        print(newPlayer.NickName +" joined the room. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    
    #endregion
}
