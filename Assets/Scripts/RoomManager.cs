using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*
* When you join a room : JoinRandomRoom()
* 1-It doesn't exist: OnJoinRandomFailed() and creates a new room CreateAndJoinRoom(); -> creates a new room
*   -The room is created with: OnCreatedRoom()
*   -Player enters the room: OnPlayerEnteredRoom()
* 2-It exists: OnJoinedRoom() -> enters an existing room
*   -Player enters the room: OnPlayerEnteredRoom()
*
* When you click to enter a specific map
* 1-OnEnterButtonClickedOutdoor - PhotonNetwork.JoinRandomRoom -> try to join Outdoor room
* 2-OnEnterButtonClickedSchool -  PhotonNetwork.JoinRandomRoom -> try to join School room
*
*/

public class RoomManager : MonoBehaviourPunCallbacks
{
    private string mapType;

    void Start()
    {
        //Syncs scene to each player
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region UI Callback Methods
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //When player clicks in ENTER UI Button below the Outdoor Map
    public void OnEnterButtonClickedOutdoor()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable(){ {MultiplayerVRConstants.MAP_TYPE_KEY, mapType }};
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    //When player clicks in ENTER UI Button below the School Map
    public void OnEnterButtonClickedSchool()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable(){ {MultiplayerVRConstants.MAP_TYPE_KEY, mapType }};
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }
    #endregion

    #region Photon Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("There's no room yet");
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
                if((string) mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL)
                {
                    //Load school
                    PhotonNetwork.LoadLevel("World_School");
                }
                else if((string) mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR )
                {
                    //Load outdoor
                    PhotonNetwork.LoadLevel("World_Outdoor");
                }
            }
        }
    
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Alerts when a new player entered the room
        print(newPlayer.NickName +" joined the room. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    #endregion

    #region Private Methods
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
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable(){ {MultiplayerVRConstants.MAP_TYPE_KEY, mapType}};

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;

        PhotonNetwork.CreateRoom(randomName, roomOptions);
    }
    #endregion


}
