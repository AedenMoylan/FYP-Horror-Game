using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float Range;
    public float Angle;
    public LayerMask ObstacleLayer;
    public int NumOfTriangles = 120;
    Mesh VisionConeMesh;
    MeshFilter meshFilter;
    public Material VisionConeMaterial;
    private Transform hitTarget;
    public GameObject player;
    public GameObject killerMoveObject;
    void Start()
    {
        transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        meshFilter = transform.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        Angle *= Mathf.Deg2Rad;
    }


    void Update()
    {
        visionCone();
    }

    void visionCone()
    {
        int[] triangles = new int[(NumOfTriangles - 1) * 3];
        Vector3[] Vertices = new Vector3[NumOfTriangles + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -Angle / 2;
        float angleIcrement = Angle / (NumOfTriangles - 1);

        for (int i = 0; i < NumOfTriangles; i++)
        {
            float sin = Mathf.Sin(Currentangle);
            float cos = Mathf.Cos(Currentangle);
            Vector3 direction = (transform.forward * cos) + (transform.right * sin);
            Vector3 VertForward = (Vector3.forward * cos) + (Vector3.right * sin);
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Range, ObstacleLayer))
            {
                //hitTarget = hit.transform;
                checkifPlayer(hit.transform);
                Vertices[i + 1] = (VertForward / 2) * hit.distance;
            }
            else
            {
                Vertices[i + 1] = VertForward * Range / 2;
            }


            Currentangle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;
        meshFilter.mesh = VisionConeMesh;
    }

    void checkifPlayer(Transform hitTransform)
    {
        if (hitTransform.IsChildOf(player.transform) == true)
        {
            KillerScript killer = this.GetComponentInParent<KillerScript>()/*.setToHunt()*/;
            killer.setToHunt();
            killerMoveObject.transform.position = hitTransform.position;
        }
    }
}