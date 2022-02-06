using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class MenuClientButton : MonoBehaviour
{
    NetworkManager manager;

    public TMP_InputField serverIP;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void startClient()
    {
        Debug.Log(serverIP.text);
        manager.networkAddress = serverIP.text;
        manager.StartClient();      
    }
}
