using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KillerSpawnScript : MonoBehaviour
{
    public List<GameObject> appropriateSpawnCells;
    public GameObject killer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addSpawnCell(GameObject _cell)
    {
        appropriateSpawnCells.Add(_cell);
    }

    public void spawnKiller()
    {
        NavMeshAgent navAgent = this.GetComponent<NavMeshAgent>();
        Vector3 warpPosition = appropriateSpawnCells[1].transform.position;
        warpPosition = new Vector3(warpPosition.x - 5, warpPosition.y, warpPosition.z + 5);
        navAgent.Warp(warpPosition);
    }
}
