using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace ClassroomSimulator
{
    public class ChairBehaviour : NetworkBehaviour
    {
        public GameObject prefabPlayer;
        public Transform sittingPos;
        public Transform exitPos;
        public bool isHitting;
        public float speed = 0.2f;
        public bool isSitting;
        public bool isStanding;
        Player player;
        CharacterController charac;
        Vector3 newPos;

        void Start()
        {
            isSitting = false;
            isStanding = true;
        }

        // Update is called once per frame
        void Update()
        {


            
           
            if (isStanding)
            {
                if (isHitting && Input.GetKeyDown(KeyCode.Space))
                {
                    isStanding = false;
                    isSitting = true;
                    charac.enabled = false;
                    prefabPlayer.transform.position = new Vector3(sittingPos.position.x, sittingPos.position.y, sittingPos.position.z);
                    prefabPlayer.transform.rotation =Quaternion.Euler(sittingPos.rotation.x, sittingPos.rotation.y, sittingPos.rotation.z);
                    player.moveSpeed = 0f;
                    Debug.Log("i sat");
                }
                
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                charac.enabled = true;
               
            }
            if (isSitting)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isSitting = false;
                    isStanding = true;
                    charac.enabled = false;
                    prefabPlayer.transform.position = new Vector3(exitPos.position.x, exitPos.position.y, exitPos.position.z);
                    prefabPlayer.transform.rotation = Quaternion.Euler(sittingPos.rotation.x, sittingPos.rotation.y, sittingPos.rotation.z);
                    player.moveSpeed = 10f;
                    Debug.Log("i stand");
                }
               
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                charac.enabled = true;
            }

        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                isHitting = true;
                prefabPlayer = other.gameObject;
                charac = prefabPlayer.GetComponent<CharacterController>();
                player = prefabPlayer.GetComponent<Player>();
            }
        }
        private void OnTriggerExit(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                isHitting = false;
                prefabPlayer = null;
            }
        }   


    }
}
