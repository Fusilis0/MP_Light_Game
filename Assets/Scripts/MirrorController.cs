using Photon.Pun;
using UnityEngine;

public class MirrorController : MonoBehaviour
{
    public PhotonView PV;
    public Transform mirror;
    public float y;

    private bool isPlayerCloseLocal = false;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        y = mirror.transform.rotation.y;
    }

    private void Update()
    {
        RotateObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            isPlayerCloseLocal = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            isPlayerCloseLocal = false;
        }
    }

    private void RotateObject()
    {      
        if (isPlayerCloseLocal)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                PV.RPC("RPC_YPlus", RpcTarget.All);
            }

            if (Input.GetKey(KeyCode.E))
            {
                PV.RPC("RPC_YMinus", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void RPC_YPlus()
    {
        y += 0.2f;
        mirror.transform.rotation = Quaternion.Euler(mirror.transform.rotation.x, y, mirror.transform.rotation.z);
    }

    [PunRPC]
    public void RPC_YMinus()
    {
        y -= 0.2f;
        mirror.transform.rotation = Quaternion.Euler(mirror.transform.rotation.x, y, mirror.transform.rotation.z);
    }
}
