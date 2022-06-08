using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{

    public GameObject LocalXRRigGameobject;
    public GameObject AvatarHeadGameObject;
    public GameObject AvatarBodyGameObject;

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            //If the player is local
            LocalXRRigGameobject.SetActive(true);

            //Change their layers so the local player won't see their own body
            SetLayerRecursively(AvatarHeadGameObject, 6 );
            SetLayerRecursively(AvatarHeadGameObject, 7 );

            //Checks for Teleportation areas
            TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<TeleportationArea>();
            if(teleportationAreas.Length > 0)
            {
                print("Found " + teleportationAreas.Length + " teleportation area.");
                foreach(var item in teleportationAreas)
                {
                    //Adds the teleportation area to the local XR Rig Provider
                    item.teleportationProvider = LocalXRRigGameobject.GetComponent<TeleportationProvider>();
                }
            }
            
        }else
        {
            //The player is remote (disable XR Rig)
            LocalXRRigGameobject.SetActive(false);

            //Change their layers so the local player can see other people's bodies
            SetLayerRecursively(AvatarBodyGameObject, 0 );
            SetLayerRecursively(AvatarHeadGameObject, 0 );
        }
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
