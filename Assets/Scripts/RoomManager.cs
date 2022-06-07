using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Callback Methods
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion

    #region Photon Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        CreateAndJoinRoom();
    }

    public override void OnCreatedRoom()
    {
        print("A room is created with the name" + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
       //Only appears for the first player
       print("The local player" + PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + ". Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    
       //Checks for the current room properties
        if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
        {
            object mapType;
            //Prints the room the player is at
            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out mapType))
            {
                print("Joined the room with the map:" + (string) mapType);
            }
        }
    
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print(newPlayer.NickName +" joined the room. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    #endregion

    #region Private Methords
    private void CreateAndJoinRoom()
    {
        string randomName = "Room_" + Random.Range(0,10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        string[] roomPropsInLobby = { "map" };

        //We have 2 different maps
        //1. Outdoor = "outdoor"
        //2. School = "school"

        //Configure the room properties
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable(){ {MultiplayerVRConstants.MAP_TYPE_KEY, MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL}};

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(randomName, roomOptions);
    }
    #endregion


}
