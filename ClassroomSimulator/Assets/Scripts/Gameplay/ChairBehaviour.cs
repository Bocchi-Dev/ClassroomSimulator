using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ClassroomSimulator;

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
        public Collider chairCollider;
        float setPlayerSpeed;
       

        void Start()
        {
            chairCollider = GetComponent<Collider>();
            isSitting = false;
            isStanding = true;
        }

        // Update is called once per frame
        void Update()
        {     
            if (isStanding)
            {
                if (isHitting && Input.GetButtonDown("Sit"))
                {
                    isStanding = false;
                    isSitting = true;
                    charac.enabled = false;
                    prefabPlayer.transform.position = new Vector3(sittingPos.position.x, sittingPos.position.y, sittingPos.position.z);
                    prefabPlayer.transform.rotation =Quaternion.Euler(sittingPos.rotation.x, sittingPos.rotation.y, sittingPos.rotation.z);
                    setPlayerSpeed = player.moveSpeed;
                    player.moveSpeed = 0f;
                    player.anime.SetBool("isSitting", true);
                    Debug.Log("i sat");
                    chairCollider.enabled = false;
            }
                
            }
            if (Input.GetButtonDown("Sit"))
            {
                charac.enabled = true;
            }
            if (isSitting)
            {
                if (Input.GetButtonDown("Stand"))
                {
                    isSitting = false;
                    isStanding = true;
                    charac.enabled = false;
                    prefabPlayer.transform.position = new Vector3(exitPos.position.x, exitPos.position.y, exitPos.position.z);
                    prefabPlayer.transform.rotation = Quaternion.Euler(sittingPos.rotation.x, sittingPos.rotation.y, sittingPos.rotation.z);
                    player.moveSpeed = setPlayerSpeed;
                    player.anime.SetBool("isSitting", false);
                    Debug.Log("i stand");
                    chairCollider.enabled = true;
                }
               
            }
            if (Input.GetButtonDown("Stand"))
            {
                charac.enabled = true;
            }

        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                prefabPlayer = other.gameObject;

                charac = prefabPlayer.GetComponent<CharacterController>();
                player = prefabPlayer.GetComponent<Player>();
                isHitting = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                prefabPlayer = null;

                charac = null;
                player = null;
                isHitting = false;
            }
        }


    }

