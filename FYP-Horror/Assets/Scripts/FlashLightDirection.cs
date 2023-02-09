using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightDirection : MonoBehaviour
{
    public Camera camera;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        Ray ray = camera.ScreenPointToRay(mousePos);       
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(hit.point);
        }

    }
}
