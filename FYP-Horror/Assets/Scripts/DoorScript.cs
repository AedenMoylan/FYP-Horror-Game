using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isDoorOpen = false;
    public bool isDoorOpening = false;
    public float doorOpenSpeed;
    public float doorOpenDuration;
    private float currentOpenDuration = 0;

    public Transform rotationPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        openDoor();
    }

    private void openDoor()
    {
        if (isDoorOpen == true)
        {
            if (currentOpenDuration < doorOpenDuration)
            {
                transform.RotateAround(rotationPoint.position, new Vector3(0, -1, 0), doorOpenSpeed * Time.deltaTime);
                currentOpenDuration += 1 * Time.deltaTime;
            }
        }
    }
}
