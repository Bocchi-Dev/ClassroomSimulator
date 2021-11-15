using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        //if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        //{
        //    StartButtons();
        //}
        //else
        //{
        //    StatusLabels();

        //    SubmitNewPosition();
        //}

        StatusLabels();

        GUILayout.EndArea();
    }

    //static void StartButtons()
    //{
    //    if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
    //    if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
    //    if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    //}

    public static void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public static void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        //NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public static void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
        "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
        NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    //static void SubmitNewPosition()
    //{  
    //    if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
    //    {
    //        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
    //        var player = playerObject.GetComponent<Controls>();
    //        player.Move();
    //    }
    //}
}

