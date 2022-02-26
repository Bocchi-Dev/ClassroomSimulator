using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
using Telepathy;

namespace ClassroomSimulator
{
    public class SceneScript : NetworkBehaviour
    {
        public SceneReference sceneReference;
        public Text canvasStatusText;
        public Text canvasAutoTraffic;
        public Text canvasPlayerCount;
        public GameObject[] playersArray;
        public PlayerMovement playerScript;

        [SyncVar(hook = nameof(OnStatusTextChanged))]
        public string statusText;
        void OnStatusTextChanged(string _Old, string _New)
        {
            //called from sync var hook, to update info on screen for all players
            canvasStatusText.text = statusText;
        }

        public void ButtonSendMessage()
        {
            if (playerScript) { playerScript.CmdSendPlayerMessage(); }
        }

        public void ButtonSetupAutoTraffic()
        {
            if (playerScript)
            {
                staticC.traffic += 1; if (staticC.traffic > 4) { staticC.traffic = 0; }
                playerScript.SetupAutoTraffic();
                canvasAutoTraffic.text = "Traffic: " + playerScript.trafficType;
            }
        }

        public void ButtonFindPlayers()
        {
            playersArray = GameObject.FindGameObjectsWithTag("Player");
            canvasPlayerCount.text = "Players: " + playersArray.Length;
        }
    }
}


