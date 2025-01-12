using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [HideInInspector] public PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
}
