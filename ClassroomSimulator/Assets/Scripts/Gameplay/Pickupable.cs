using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace ClassroomSimulator
{
    public class Pickupable : NetworkBehaviour
    {
        [SyncVar]
        public GameObject Parent;

        public override void OnStartServer()
        {
            NetworkServer.Spawn(gameObject);
            base.OnStartServer();
        }

        public override void OnStartClient()
        {
            Debug.Log("Network Client Ready OBJECTS: " + NetworkClient.ready);
        }

        // Update is called once per frame
        void Update()
        {
            if (Parent != null)
            {
                transform.position = Parent.transform.GetChild(0).position;
                transform.rotation = Parent.transform.GetChild(0).rotation;
            }
        }
    }

}
