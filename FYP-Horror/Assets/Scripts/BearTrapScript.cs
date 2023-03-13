using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearTrapScript : MonoBehaviour
{
    public GameObject TrapHalf;
    public GameObject TrapHalf2;
    private Vector3 BearTrapSnapRotation;
    private bool isPlayerTrapped = false;
    private Vector3 endClampRotation;
    public float maxTrapResetDuration;
    private float trapResetProgress;
    private bool trapDestroyed = false;
    private GameObject killer;
    private GameObject killerMovePosition;
    private PlayerMovementScript playerMovementScript;


    // Start is called before the first frame update
    void Start()
    {
        killer = GameObject.Find("Killer");
        killerMovePosition = GameObject.Find("KillerMovePosition");
        endClampRotation = new Vector3(60,0,0);
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerTrapped == true)
        {
            trapPlayer();
            disarmTrap();
        }      
    }

    public void trapPlayer()
    {
        BearTrapSnapRotation = Vector3.Lerp(new Vector3(0,0,0), endClampRotation, 1);
        TrapHalf.transform.rotation = Quaternion.Euler(BearTrapSnapRotation);
        TrapHalf2.transform.rotation = Quaternion.Euler(-BearTrapSnapRotation);
        killerMovePosition.transform.position = new Vector3(this.transform.position.x, killerMovePosition.transform.position.y, this.transform.position.z);
        killer.GetComponent<KillerScript>().setToHunt();
    }

    public void setTrapBoolToTrue()
    {
        isPlayerTrapped = true;
    }

    public void disarmTrap()
    {
        trapResetProgress += Time.deltaTime;

        if (trapResetProgress >= maxTrapResetDuration)
        {
            destroyTrap();
        }
    }

    private void destroyTrap()
    {
        playerMovementScript.changeCanPlayerMove(true);
        Destroy(this.gameObject);
    }
}
