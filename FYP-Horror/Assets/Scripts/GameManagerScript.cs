using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject killer;
    public GameObject killerMovePosition;
    public GameObject player;
    private GameObject wardrobeWithPlayer;
    private KillerScript killerScript;
    // Start is called before the first frame update
    void Start()
    {
        killerScript = killer.GetComponent<KillerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playerEnteredWardrobe()
    {
        bool inVision = killer.GetComponent<FieldOfView>().getIsPlayerInVision();
        killerScript.willBearTrapBePlacedAtDestination = true;
        wardrobeWithPlayer = player.GetComponent<PlayerScript>().wardrobe;
        wardrobeWithPlayer.GetComponentInParent<WardrobeScript>().setIsPlayerInside(true);
        if (inVision == true)
        {
            killerMovePosition.transform.position = wardrobeWithPlayer.transform.Find("PlayerLeavePosition").gameObject.transform.position;
        }
    }
}
