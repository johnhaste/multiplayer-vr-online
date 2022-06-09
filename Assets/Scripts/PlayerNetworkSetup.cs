using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{

    public GameObject LocalXRRigGameobject;
    public GameObject MainAvatarGameObject;


    public GameObject AvatarHeadGameObject;
    public GameObject AvatarBodyGameObject;

    public GameObject[] AvatarModelPrefabs;

    public TextMeshProUGUI PlayerNameText;

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            //If the player is local
            LocalXRRigGameobject.SetActive(true);

            //Loading correct avatar model
            object avatarSelectionNumber;
            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_SELECTION_NUMBER, out avatarSelectionNumber))
            {
                print("Avatar selection number:" + (int) avatarSelectionNumber);
                photonView.RPC("InitializeSelectedAvatarModel", RpcTarget.AllBuffered, (int) avatarSelectionNumber);
            }

            //AvatarSelectionManager.Instance

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
            
            MainAvatarGameObject.AddComponent<AudioListener>();

        }else
        {
            //The player is remote (disable XR Rig)
            LocalXRRigGameobject.SetActive(false);

            //Change their layers so the local player can see other people's bodies
            SetLayerRecursively(AvatarBodyGameObject, 0 );
            SetLayerRecursively(AvatarHeadGameObject, 0 );
        }

        if(PlayerNameText != null)
        {
            PlayerNameText.text = photonView.Owner.NickName;
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

    //PunRPC -> Is Executed for all players
    [PunRPC]
    public void InitializeSelectedAvatarModel(int avatarSelectionNumber)
    {
        GameObject selectedAvatarGameobject = Instantiate(AvatarModelPrefabs[avatarSelectionNumber],LocalXRRigGameobject.transform);

        AvatarInputConverter avatarInputConverter = LocalXRRigGameobject.GetComponent<AvatarInputConverter>();
        AvatarHolder avatarHolder = selectedAvatarGameobject.GetComponent<AvatarHolder>();
        SetUpAvatarGameobject(avatarHolder.HeadTransform,avatarInputConverter.AvatarHead);
        SetUpAvatarGameobject(avatarHolder.BodyTransform,avatarInputConverter.AvatarBody);
        SetUpAvatarGameobject(avatarHolder.HandLeftTransform, avatarInputConverter.AvatarHand_Left);
        SetUpAvatarGameobject(avatarHolder.HandRightTransform, avatarInputConverter.AvatarHand_Right);
    }

    void SetUpAvatarGameobject(Transform avatarModelTransform, Transform mainAvatarTransform)
    {
        avatarModelTransform.SetParent(mainAvatarTransform);
        avatarModelTransform.localPosition = Vector3.zero;
        avatarModelTransform.localRotation = Quaternion.identity;
    }
}
