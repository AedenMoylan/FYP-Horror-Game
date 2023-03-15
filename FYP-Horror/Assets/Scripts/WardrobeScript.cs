using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeScript : MonoBehaviour
{
    bool isPlayerInside = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setIsPlayerInside(bool _inside)
    {
        isPlayerInside = _inside;
    }

    public bool getIsPlayerInside()
    {
        return isPlayerInside;
    }
}
