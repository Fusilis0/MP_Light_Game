using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RGB_Button : MonoBehaviour
{
    public bool isOn;
    public MeshRenderer MeshRenderer;
    public Material grey;
    public Material ownColor;
    
    PhotonView PV;

    // Start is called before the first frame update
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        PV.RPC("RPC_ButtonOnOff", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_ButtonOnOff()
    {

        if (isOn)
        {
            MeshRenderer.material = ownColor;
        }
        if (!isOn)
        {
            MeshRenderer.material = grey;
        }
    }

    public void OnButton()
    {
        PV.RPC("RPC_OnButton", RpcTarget.All);
    }

    public void OffButton()
    {
        PV.RPC("RPC_OffButton", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_OnButton()
    {
        isOn = true;
    }
    [PunRPC]
    public void RPC_OffButton()
    {
        isOn = false;
    }

}
