using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using Unity.Netcode;

public class PasswordNetworkManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private GameObject passwordEntryUI;
    [SerializeField] private GameObject leaveButton;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void OnDestroy()
    {
        if(NetworkManager.Singleton == null) { return; }

        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }

    public void Host()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    public void Client()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passwordInputField.text);
        NetworkManager.Singleton.StartClient();
    }

    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }

        passwordEntryUI.SetActive(true);
        leaveButton.SetActive(false);
    }

    private void HandleServerStarted()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HandleClientConnected(NetworkManager.Singleton.LocalClientId);
        }
    }
    private void HandleClientConnected(ulong clientId)
    {
        if(clientId == NetworkManager.Singleton.LocalClientId)
        {
            passwordEntryUI.SetActive(false);
            leaveButton.SetActive(true);
        }
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            passwordEntryUI.SetActive(true);
            leaveButton.SetActive(false);
        }
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        string password = Encoding.ASCII.GetString(connectionData);

        bool approvedConnection = password == passwordInputField.text;

        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        
        callback(true, null, approvedConnection, spawnPos, spawnRot);
    }

}
