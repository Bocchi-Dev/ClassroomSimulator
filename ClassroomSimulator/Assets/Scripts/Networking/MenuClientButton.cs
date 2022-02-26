using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class MenuClientButton : MonoBehaviour
{
    NetworkManager manager;

    public TMP_InputField playerNameInput;

    public string playerName;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void startClient()
    {
        playerName = playerNameInput.text;
        manager.StartClient();      
    }

    public void hostServer()
    {
        playerName = playerNameInput.text;
        manager.StartHost();
    }

}
