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
    private const int MAX_AMMO = 6;
    private int ammo = MAX_AMMO;
    public Camera weaponCamera;
    private ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveGunOnScreen());
        particleSystem = this.GetComponent<ParticleSystem>();
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

        aimAtMiddleOfScreen();
        shootGun();

        if (Input.GetButtonDown("R"))
        {
            StartCoroutine(reloadGun());
        }
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

    private void aimAtMiddleOfScreen()
    {
        Ray ray = weaponCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
        }

        Debug.DrawRay(this.transform.position, hit.point, Color.green, 1);
    }

    private void shootGun()
    {
        if (Input.GetMouseButtonDown(0) && ammo > 0)
        {
            ammo--;
            particleSystem.Play();
            Debug.Log(ammo);
        }
    }
    IEnumerator reloadGun()
    {
        yield return new WaitForSeconds(3);
        ammo = MAX_AMMO;
    }
}
