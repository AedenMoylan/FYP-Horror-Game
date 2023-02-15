using System.Collections;
using System.Collections.Generic;
using Tobii.Research.Unity;
using UnityEngine;

public class FlashLightDirection : MonoBehaviour
{
    public Camera camera;
    public RaycastHit testRay;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        Ray ray = camera.ScreenPointToRay(mousePos);       
        RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    transform.LookAt(hit.point);
        //}

        transform.LookAt(testRay.point);
    }
}