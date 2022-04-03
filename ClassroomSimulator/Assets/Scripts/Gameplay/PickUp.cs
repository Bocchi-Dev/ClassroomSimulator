using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace ClassroomSimulator
{
    public class PickUp : NetworkBehaviour
    {
        public GameObject PickUpObject;
        public GameObject Hand;
        public GameObject mainCamera;
        public int lastChildIndex;
        public bool isCarrying = false;

        //private void OnTriggerEnter(Collider other)
        //{
        //    Debug.Log("touch");
        //    if (!other.CompareTag("PickUp")) return;

        //    CmdPickUp(other.gameObject);
        //}

        private void Start()
        {
            lastChildIndex = gameObject.gameObject.transform.childCount - 1;
            mainCamera = gameObject.transform.GetChild(lastChildIndex).gameObject;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Interact"))
            {
                Debug.Log("Pressed interact;");
                if (!isCarrying)
                {
                    int x = Screen.width / 2;
                    int y = Screen.height / 2;

                    Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log("raycats hit");
                        if (hit.collider.CompareTag("PickUp"))
                        {
                            isCarrying = true;
                            CmdPickUp(hit.collider.gameObject);
                            Debug.Log("pickup run");
                        }
                    }
                }
                else
                {
                    isCarrying = false;
                    CmdDropOff();
                    Debug.Log("dropped");
                }
            }
        }

        [Command]
        public void CmdPickUp(GameObject pickObject)
        {
            Debug.Log(pickObject.name);
            PickUpObject = pickObject;
            PickUpObject.GetComponent<NetworkIdentity>().RemoveClientAuthority();
            PickUpObject.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            PickUpObject.GetComponent<Pickupable>().Parent = Hand;
        }

        [Command]
        public void CmdDropOff()
        {
            PickUpObject.GetComponent<Pickupable>().Parent = null;
        }
    }

}

