using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightSource : MonoBehaviour
{
    public int reflections = 5;
    public float maxLenght = 15;

    public LightColorManager colorManager;
    public Material lineColor;
    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit hit;

    public DoorManager door;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {

        ShowLine();
    }

    public void ShowLine()
    {
        ray = new Ray(transform.position, transform.forward);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remaningLength = maxLenght;

        for (int i = 0; i < reflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remaningLength))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remaningLength -= Vector3.Distance(ray.origin, hit.point);

                if (hit.collider.tag == "Mirror")
                {
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                }

                if (hit.collider.tag == "Key")
                {
                    door.isOnKey = true;
                    door.CheckForEquality();
                    break;
                }
                else if (hit.collider.tag != "Key")
                {
                    door.isOnKey = false;
                    door.CheckForEquality();
                }
                else if (hit.collider.tag != "Mirror")
                {
                    break;
                }
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remaningLength);
            }
        }

        lineRenderer.material = lineColor;
        lineColor = colorManager.outColor;
    }
}
