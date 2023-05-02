using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeScript : MonoBehaviour
{
    bool isPlayerInside = false;


    public void setIsPlayerInside(bool _inside)
    {
        isPlayerInside = _inside;
    }

    public bool getIsPlayerInside()
    {
        return isPlayerInside;
    }
}
