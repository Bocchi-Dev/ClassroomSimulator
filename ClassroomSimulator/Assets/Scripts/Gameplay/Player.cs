using UnityEngine;
using Mirror;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace ClassroomSimulator
{
    public class Player : NetworkBehaviour
    {
        private SceneScript sceneScript;
        private GameObject sceneScriptObj;

        public TextMesh playerNameText;
        public GameObject floatingInfo;
        private Material playerMaterialClone;
        private float weaponCooldownTime;

        private float autoTurnAmount = 0.0f;
        private float autoMoveAmount = 0.0f;
        public string trafficType = "none";

        //Movement
        public float moveSpeed = 10f;
        public float gravity = -9.8f;
        public float jumpHeight = 1f;
        public CharacterController controller;

        [SyncVar(hook = nameof(OnNameChanged))]
        public string playerName;

        //Object interaction
        public GameObject mainCamera;
        bool carrying;
        GameObject carriedObject;
        public float distance;
        public float smooth;
        public float throwForce = 10;
        public int lastChildIndex;

        void OnNameChanged(string _Old, string _New)
        {
            playerNameText.text = playerName;
        }

        [SyncVar(hook = nameof(OnColorChanged))]
        public Color playerColor = Color.white;

        void OnColorChanged(Color _Old, Color _New)
        {
            //Debug.Log(gameObject.name + " HOOK OnColorChanged");
            playerNameText.color = _New;
            playerMaterialClone = new Material(this.GetComponent<Renderer>().material);
            playerMaterialClone.color = _New;
            this.GetComponent<Renderer>().material = playerMaterialClone;
        }

        public override void OnStartLocalPlayer()
        {
            sceneScript.playerScript = this;

            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0.60f, 0);
            #if UNITY_EDITOR
                Camera.main.GetComponent<MouseLook>().playerBody = transform;
            #endif

            floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
            floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            string name = FindObjectOfType<MenuClientButton>().playerName;
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
 
            CmdSetupPlayer(name, color);

            SetupAutoTraffic();
        }

        public override void OnStartClient()
        {
            Debug.Log("Network Client Ready: " + NetworkClient.ready);
        }

        void Awake()
        {
            //allow all players to run this
            sceneScript = GameObject.Find("SceneReference").GetComponent<SceneReference>().sceneScript;        
        }

        private void Start()
        {
            controller = GetComponent<CharacterController>();
#if PLATFORM_ANDROID
            Camera.main.GetComponent<MouseLook>().enabled = false;
            Input.gyro.enabled = true;
#endif
#if UNITY_EDITOR
            Camera.main.GetComponent<MouseLook>().enabled = true;
#endif

            lastChildIndex = gameObject.gameObject.transform.childCount - 1;
            mainCamera = gameObject.transform.GetChild(lastChildIndex).gameObject;
        }

        [Command]
        public void CmdSendPlayerMessage()
        {
            if (sceneScript) { sceneScript.statusText = playerName + " says hello " + UnityEngine.Random.Range(10, 99); }
        }

        [Command]
        public void CmdSetupPlayer(string _name, Color _col)
        {
            //player info sent to server, then server updates sync vars which handles it on all clients
            playerNameText.text = _name;
            playerName = _name;
            playerColor = _col;
        }

        void Update()
        {
            //FindSceneScript();
            //allow all players to run this
            if (!isLocalPlayer)
            {
                // make non-local players run this
                floatingInfo.transform.LookAt(Camera.main.transform);
                return;
            }

            //insert movement here
            PlayerMovement();

            //object interaction here
            if (carrying)
            {
                carry(carriedObject);
                checkDrop();
                checkThrow();
            }
            else
            {
                pickUp();
            }

            if (staticC.traffic > 1)
            {
                if (autoTurnAmount > 0) { transform.Rotate(0, autoTurnAmount, 0); }
                if (autoMoveAmount > 0) { transform.Translate(0, 0, autoMoveAmount); }
            }
        }

       
        void PlayerMovement()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime);
            Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
            Vector3 velocity = direction * moveSpeed;
            velocity = Camera.main.transform.TransformDirection(velocity);

            velocity.y += gravity;

            if (controller.isGrounded)
            {
                velocity.y = 0;
            }

            if (Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                //float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
                velocity.y += jumpHeight;
            }


            
#if PLATFORM_ANDROID
            transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y / 2, 0);
#endif

            controller.Move(velocity * Time.deltaTime);
        }

        [Command]
        void carry(GameObject o)
        {
            o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
        }

        [Command]
        void pickUp()
        {
            if (Input.GetButtonDown("Interact"))
            {
                int x = Screen.width / 2;
                int y = Screen.height / 2;

                Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Pickupable p = hit.collider.GetComponent<Pickupable>();
                    if (p != null)
                    {
                        hit.collider.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
                        //Debug.Log("transfereredauth");
                        carrying = true;
                        carriedObject = p.gameObject;
                        p.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
            }
        }

        void checkDrop()
        {
            if (Input.GetButtonDown("Interact"))
            {
                dropObject();
            }

        }

        [Command]
        void dropObject()
        {
            carrying = false;
            carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject = null;
        }

        void checkThrow()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ThrowObject();
                Debug.Log("I am Throwing");
            }
        }

        [Command]
        void ThrowObject()
        {
            carrying = false;
            carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            carriedObject.GetComponent<Rigidbody>().AddForce(mainCamera.transform.forward * throwForce);
            carriedObject = null;
        }

        void AutoRepeatingMessage()
        {
            CmdSendPlayerMessage();
        }

        public void SetupAutoTraffic()
        {

            //Debug.Log("Traffic 0=none   1=light (card game)  2=active (social game)  3=heavy (mmo)   4=fruequent (fps)");
            CancelInvoke("AutoRepeatingMessage");
            CancelInvoke("AutoRepeatingShoot");

            if (staticC.traffic == 0)
            {
                trafficType = "None";
            }
            else if (staticC.traffic == 1)
            {
                trafficType = "Cards";
                InvokeRepeating(nameof(AutoRepeatingMessage), 1, 3f);
            }
            else if (staticC.traffic == 2)
            {
                trafficType = "Social";
                autoTurnAmount = UnityEngine.Random.Range(0.1f, 1.5f);
                InvokeRepeating(nameof(AutoRepeatingMessage), 1, 2f);
            }
            else if (staticC.traffic == 3)
            {
                trafficType = "MMO";
                autoTurnAmount = UnityEngine.Random.Range(0.1f, 1.5f);
                autoMoveAmount = UnityEngine.Random.Range(0.05f, 0.1f);
                InvokeRepeating(nameof(AutoRepeatingMessage), 1, 1.0f);
            }
            else if (staticC.traffic == 4)
            {
                trafficType = "FPS";
                autoTurnAmount = UnityEngine.Random.Range(0.1f, 1.5f);
                autoMoveAmount = UnityEngine.Random.Range(0.1f, 0.2f);
                InvokeRepeating(nameof(AutoRepeatingMessage), 1, 0.75f);
            }
        }
    }
}
