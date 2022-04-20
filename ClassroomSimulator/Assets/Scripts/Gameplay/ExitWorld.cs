using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class ExitWorld : MonoBehaviour
{
    public void exitWorld()
    {
        FindObjectOfType<CustomNetworkManager>().StopClient();
        FindObjectOfType<CustomNetworkManager>().StopHost();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu"); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            exitWorld();
        }
    }
}
