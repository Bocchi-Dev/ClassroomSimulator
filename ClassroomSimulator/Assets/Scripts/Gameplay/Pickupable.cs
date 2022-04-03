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

        // Start is called before the first frame update
        void Start()
        {
            NetworkServer.Spawn(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (Parent != null)
            {
                transform.position = Parent.transform.position;
                transform.rotation = Parent.transform.rotation;
            }
        }
    }

}
