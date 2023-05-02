using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    public float xSensitivity;
    public float ySensitivity;

    private float xRotation;

    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // used for mouse and camera movement
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * xSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * ySensitivity;

        xRotation -= mouseY;


        playerTransform.Rotate(Vector3.up * mouseX);
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
