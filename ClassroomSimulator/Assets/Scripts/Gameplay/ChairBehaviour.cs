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


            prefabPlayer = GameObject.FindGameObjectWithTag("Player");
            charac = prefabPlayer.GetComponent<CharacterController>();
            player = prefabPlayer.GetComponent<Player>();
            if (isStanding)
            {
                if (isHitting && Input.GetKeyDown(KeyCode.Space))
                {
                    isStanding = false;
                    isSitting = true;
                    charac.enabled = false;
                    prefabPlayer.transform.position = new Vector3(sittingPos.position.x, sittingPos.position.y, sittingPos.position.z);
                    player.moveSpeed = 0f;
                    Debug.Log("i sat");
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    charac.enabled = true;
                }
            }
            if (isSitting && !isStanding)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isSitting = false;
                    isStanding = true;
                    charac.enabled = false;
                    prefabPlayer.transform.position = new Vector3(exitPos.position.x, exitPos.position.y, exitPos.position.z);
                    player.moveSpeed = 10f;
                    Debug.Log("i stand");
                }
                if (Input.GetKeyUp(KeyCode.E))
                {
                    charac.enabled = true;
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                isHitting = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                isHitting = false;
            }
        }


    }
}
