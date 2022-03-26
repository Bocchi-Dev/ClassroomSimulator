using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class TestConnectServerDirectlyHardocoded : MonoBehaviour
{
    NetworkManager manager;

    void Awake()
    {
        manager = FindObjectOfType<NetworkManager>();
    }

    //This is a temporary function for testing purposes
    public void connectToServer()
    {
        //manager.networkAddress = "sea.playflow.cloud.app";
        manager.StartClient();
        manager.StartHost();
    }
}
