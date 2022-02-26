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
    public TMP_InputField playerNameInput;

    public string playerName;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void startClient()
    {
        Debug.Log(serverIP.text);
        manager.networkAddress = serverIP.text;
        playerName = playerNameInput.text;
        manager.StartClient();      
    }

    public void hostServer()
    {
        playerName = playerNameInput.text;
        manager.StartHost();
    }

   
}
