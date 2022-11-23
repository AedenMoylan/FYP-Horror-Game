using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    private RoomTemplates roomTemplates;
    private int rand;
    private bool isSpawned;
    // Start is called before the first frame update
    void Start()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        rand = 0;
        isSpawned = false;
        Invoke("Spawn", 0.1f);
    }

    // Update is called once per frame
    void Spawn()
    {
        if (isSpawned == false)
        {
            if (openingDirection == 1)
            {
                rand = Random.Range(0, roomTemplates.topRooms.Length);
                Instantiate(roomTemplates.topRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 2)
            {
                rand = Random.Range(0, roomTemplates.rightRooms.Length);
                Instantiate(roomTemplates.rightRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, roomTemplates.bottomRooms.Length);
                Instantiate(roomTemplates.bottomRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, roomTemplates.leftRooms.Length);
                Instantiate(roomTemplates.leftRooms[rand], transform.position, Quaternion.identity);
            }
        }
        isSpawned = true;
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint") && other.GetComponent<RoomSpawner>().isSpawned == true)
        {
            Destroy(gameObject);
        }
    }
}
