using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightDirection : MonoBehaviour
{
    public Camera camera;
    public GameObject lookObject;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        Ray ray = camera.ScreenPointToRay(mousePos);       
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            lookObject.transform.position = hit.point;
            transform.LookAt(hit.point);
        }

    }
}
