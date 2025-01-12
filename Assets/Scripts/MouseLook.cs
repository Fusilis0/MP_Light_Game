using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{
    public PhotonView PV;
    // Start is called before the first frame update

    public Transform playerBody;
    float xRotation = 0f;
    public float mouseSens = 20;
    public Camera cam;
    // Update is called once per frame
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (!PV.IsMine)
        {
            cam.enabled = false;
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSens;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
       
        Vector3 ScreenCentreCoordinates = new Vector3(0.5f, 0.5f, 0f);    
        //Ray ray = cam.ViewportPointToRay(ScreenCentreCoordinates);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 shootPos;

        // if the raycast collides with an object, then make that our projectile target
        if (Physics.Raycast(ray, out hit))
        {
            shootPos = hit.point;
            if (hit.collider.tag == "Player")
            {
                Debug.Log("Hit a player");
                if (!hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    hit.collider.gameObject.GetComponent<PlayerController>().TakeDamage(2, 
                    hit.collider.gameObject.GetComponent<PlayerController>().PV.Owner);
                }
            }
            else if(hit.collider.tag == "Ground")
            {
                Debug.Log("Hit the ground");
            }
            else if (hit.collider.tag == "RGB_Button")
            {
                RGB_Button button = hit.collider.gameObject.GetComponent<RGB_Button>();
                Debug.Log("Hit the button");

                if (hit.collider.gameObject.GetComponent<RGB_Button>().isOn && hit.distance < 2f)
                {
                    button.OffButton();
                }
                else if (!hit.collider.gameObject.GetComponent<RGB_Button>().isOn && hit.distance < 2f)
                {
                    button.OnButton();
                }
                
            }
        }
        // if it doesn't hit anything, make our projectile target 1000 away from us (adjust this accordingly)
        else
        {
            shootPos = ray.GetPoint(1000f);
            Debug.Log("No hits");
        }
        
        
        Debug.DrawRay(cam.transform.position,shootPos,Color.red,1f);
    }

}
