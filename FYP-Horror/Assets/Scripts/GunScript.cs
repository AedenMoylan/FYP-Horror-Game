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
    public GameObject killer;
    private GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveGunOnScreen());
        particleSystem = this.GetComponent<ParticleSystem>();
        gameManager = GameObject.Find("Game Manager");
    }

    // Update is called once per frame
    void Update()
    {
        // makes sure that gun soesn't move too far
        if (isGunRaising == true)
        {
            //this.transform.position = new Vector3(transform.position.x, transform.position.y * gunRaiseSpeed * Time.deltaTime, transform.position.z);

            transform.position = transform.position + weaponCamera.transform.up * gunRaiseSpeed * Time.deltaTime;
        }

        //aimGun();

        aimAtMiddleOfScreen();
        shootGun();
        // reloads ammo
        if (Input.GetKeyDown(KeyCode.R))
        {
            ammo = 6;
        }
    }

    IEnumerator moveGunOnScreen()
    {
        yield return new WaitForSeconds(gunRaiseAmount);
        isGunRaising=false;
    }
    /// <summary>
    /// bring gun to aim position
    /// </summary>
    public void aimGun()
    {
        if (moveCounter !< 0.27)
        {
            transform.position = transform.position - weaponCamera.transform.right * gunAimSpeed * Time.deltaTime;
            moveCounter = moveCounter + Time.deltaTime;
            Debug.Log(moveCounter);
        }

    }
    /// <summary>
    /// brings gun back to non aiming position
    /// </summary>
    public void resetGun()
    {
        if (moveCounter > 0)
        {
            transform.position = transform.position + weaponCamera.transform.right * gunAimSpeed * Time.deltaTime;
            moveCounter = moveCounter - Time.deltaTime;
        }
    }

    /// <summary>
    /// shoots a ray out from the middle of the screen. returns object ray is hitting
    /// </summary>
    /// <returns></returns>
    private Transform aimAtMiddleOfScreen()
    {
        Ray ray = weaponCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Transform hitTarget = null;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hitTarget = hit.transform;
        }

        Debug.DrawRay(this.transform.position, hit.point, Color.green, 1);

        return hitTarget;
    }

    /// <summary>
    /// shoots gun if left click is pressed. Also kills killer if caught in the ray of the bullet
    /// </summary>
    private void shootGun()
    {
        if (Input.GetMouseButtonDown(0) && ammo > 0)
        {
            ammo--;
            particleSystem.Play();

            if (aimAtMiddleOfScreen().IsChildOf(killer.transform) == true)
            {
                killer.GetComponent<KillerScript>().setToDie();
                StartCoroutine(gameManager.GetComponent<GameManagerScript>().changeToMenu());
            }
        }
    }
}
