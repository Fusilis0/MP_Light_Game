using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public Material keyMaterial;
    public Material colorMaterial;
    public KeyManager keyManager;
    public LightColorManager lightColorManager;
    PhotonView PV;

    public Transform open;
    public Transform close;

    public bool isOnKey;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        keyMaterial = keyManager.mesh.material;
    }

    public void CheckForEquality()
    {
        if (lightColorManager.colorNumber == keyManager.colorNumber && isOnKey)
        {
            PV.RPC("OpenDoor", RpcTarget.All);
        }
        else if (lightColorManager.colorNumber != keyManager.colorNumber || !isOnKey)
        {
            PV.RPC("CloseDoor", RpcTarget.All);
        }
    }

    [PunRPC]
    public void OpenDoor()
    {
       transform.position = open.position;
    }

    [PunRPC]
    public void CloseDoor()
    {
        transform.position = close.position;
    }
}
