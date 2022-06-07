using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoginManager))]

public class LoginManagerEditor : Editor
{
    //Everytime the inspector is rendered
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("Responsible for connecting to Photon Servers", MessageType.Info);

        LoginManager loginManager = (LoginManager) target;

        if(GUILayout.Button("Connect Anonymously"))
        {
            loginManager.ConnectAnonymously();
        }
    }
}
