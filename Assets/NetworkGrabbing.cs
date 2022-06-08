using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    PhotonView m_photonView;
    Rigidbody rb;

    bool isBeingHeld = false;

    // Start is called before the first frame update
    void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isBeingHeld)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }

    private void TransferOwnership()
    {
        m_photonView.RequestOwnership();
    }

    public void OnSelectEntered()
    {
        print("Grabbed");

        //Broadcast the function
        m_photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);

        if(m_photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            print("It's already mine");
        }
        else
        {
            TransferOwnership();
        }
        
    }

    public void OnSelectExited()
    {
        print("Released");
        m_photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
    }

    //Requested
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        //only caled to grab object 
        if(targetView != m_photonView)
        {
            return;
        }

        print("Owndership Requested for:" + targetView.name + " from " + requestingPlayer.NickName);
        m_photonView.TransferOwnership(requestingPlayer);
    }

    //Completed
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        print("Owndership transfered to:" + targetView.name + " from " + previousOwner.NickName);
    }

    //Failed
    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }

    [PunRPC]
    public void StartNetworkGrabbing()
    {
        //Disable gravity
        isBeingHeld = true;

        //InHand Layer
        gameObject.layer = 11;
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
       //Reenables gravity
       isBeingHeld = false;  

       //Interactable Layer
       gameObject.layer = 9;
    }
}
