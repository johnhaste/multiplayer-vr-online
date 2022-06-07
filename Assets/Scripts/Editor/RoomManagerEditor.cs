using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomManager))]

public class RoomManagerEditor : Editor
{
    //Everytime the inspector is rendered
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("Responsible for connecting to Room Management", MessageType.Info);

        RoomManager roomManager = (RoomManager) target;

        /*if(GUILayout.Button("Join Random Room"))
        {
            roomManager.JoinRandomRoom();
        }*/

        if(GUILayout.Button("Join School Room"))
        {
            roomManager.OnEnterButtonClickedSchool();
        }

        if(GUILayout.Button("Join Outdoor Room"))
        {
            roomManager.OnEnterButtonClickedOutdoor();
        }
    }
}
