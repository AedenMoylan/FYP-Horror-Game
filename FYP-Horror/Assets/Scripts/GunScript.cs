using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    private float gunRaiseAmount = 1.17f;
    private float gunRaiseSpeed = 1f;
    private float gunAimSpeed = 3f;
    private bool isGunRaising = true;
    private double moveCounter = 0;
    public Camera weaponCamera;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveGunOnScreen());
    }

    // Update is called once per frame
    void Update()
    {
        if (isGunRaising == true)
        {
            //this.transform.position = new Vector3(transform.position.x, transform.position.y * gunRaiseSpeed * Time.deltaTime, transform.position.z);

            transform.position = transform.position + weaponCamera.transform.up * gunRaiseSpeed * Time.deltaTime;
        }

        //aimGun();

    }

    IEnumerator moveGunOnScreen()
    {
        yield return new WaitForSeconds(gunRaiseAmount);
        isGunRaising=false;
    }

    public void aimGun()
    {
        if (moveCounter !< 0.27)
        {
            transform.position = transform.position - weaponCamera.transform.right * gunAimSpeed * Time.deltaTime;
            moveCounter = moveCounter + Time.deltaTime;
            Debug.Log(moveCounter);
        }

    }

    public void resetGun()
    {
        if (moveCounter > 0)
        {
            transform.position = transform.position + weaponCamera.transform.right * gunAimSpeed * Time.deltaTime;
            moveCounter = moveCounter - Time.deltaTime;
        }
    }
}
