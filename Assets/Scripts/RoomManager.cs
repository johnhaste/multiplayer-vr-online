using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/*
* When app starts: checks if it's connected and ready
*   -If it is not, connect and then JoinLobby()
*   -If it is connected already, just JoinLobby()
*
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
    public TextMeshProUGUI OccupancyRateText_ForSchool;
    public TextMeshProUGUI OccupancyRateText_ForOutdoor;

    void Start()
    {
        //Syncs scene to each player
        PhotonNetwork.AutomaticallySyncScene = true;

        //Check if it's connected
        if(!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
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

    public override void OnConnectedToMaster()
    {
        print("Connected to servers again");
        PhotonNetwork.JoinLobby();
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

    //It's called when a room is created or modified
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(roomList.Count == 0)
        {
            OccupancyRateText_ForSchool.text = 0 + " / " + 20;
            OccupancyRateText_ForOutdoor.text = 0 + " / " + 20;
        }

        //Goes through each room created
        foreach(RoomInfo room in roomList)
        {
            print(room.Name);
            if(room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR))
            {
                print("Room: Outdoor. Player count:" + room.PlayerCount);
                OccupancyRateText_ForOutdoor.text = room.PlayerCount + "/" + 20;
            }
            else if(room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL))
            {
                print("Room: School. Player count:" + room.PlayerCount);
                OccupancyRateText_ForSchool.text = room.PlayerCount + "/" + 20;
            }
        }
    }

    public override void OnJoinedLobby()
    {
        print("Joined the Lobby");
    }
    #endregion

    #region Private Methods
    private void CreateAndJoinRoom()
    {
        string randomName = "Room_" + mapType + Random.Range(0,10000);
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
