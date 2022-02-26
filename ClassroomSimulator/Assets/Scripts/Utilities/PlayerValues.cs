using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public string playerName;
    //add other values later

    public void initPlayerValues(string name)
    {
        playerName = name;
    }
}
