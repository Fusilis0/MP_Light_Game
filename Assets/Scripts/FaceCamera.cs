using UnityEngine;
using System.Collections;
using Photon.Pun;

public class FaceCamera : MonoBehaviour
{
    public Camera cam;
 
    //Orient the camera after all movement is completed this frame to avoid jittering
    void Update()
    {
        if(cam == null)
        {
            FindLocalCam();
        }
        else
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
            cam.transform.rotation * Vector3.up);
        }
        
    }

    void FindLocalCam()
    {
        Camera [] cameras = FindObjectsOfType<Camera>();
        foreach(Camera c in cameras)
        {
            if(c.GetComponentInParent<PhotonView>().IsMine)
            {
                cam = c;
            }
        }
    }
}