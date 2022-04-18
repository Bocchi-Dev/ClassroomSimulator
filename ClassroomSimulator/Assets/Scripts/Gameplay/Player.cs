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

        //movement Stuff
        Rigidbody rb;
        public float moveSpeed = 10f;
        [Range(1,10)]
        public float jumpVelocity;
        public CharacterController controller;

        //Jumping Stuff
        public float gravity = -9.8f;
        public float jumpSpeed = 1f;
        private float tempDirectionY;

        [SyncVar(hook = nameof(OnNameChanged))]
        public string playerName;

     
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

        void Awake()
        {
            //allow all players to run this
            sceneScript = GameObject.Find("SceneReference").GetComponent<SceneReference>().sceneScript;
        }

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            rb = GetComponent<Rigidbody>();
#if PLATFORM_ANDROID
            Camera.main.GetComponent<MouseLook>().enabled = false;
            Input.gyro.enabled = true;
#endif
#if UNITY_EDITOR
            Camera.main.GetComponent<MouseLook>().enabled = true;
#endif
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

            Debug.Log("Player is grounded: " + controller.isGrounded);

           

            Vector3 velocity = direction * moveSpeed;
            velocity = Camera.main.transform.TransformDirection(velocity);

            if (controller.isGrounded)
            {
                velocity.y = 0;

                if (Input.GetButtonDown("Jump"))
                {
                    Debug.Log("Jumped!");
                    tempDirectionY = jumpSpeed;
                }
            }

            if (horizontalInput != 0 || verticalInput != 0)
            {
                FindObjectOfType<AudioManager>().Play("Walking");
                Debug.Log("I am Walking");
            }
            else
            {
                FindObjectOfType<AudioManager>().Stop("Walking");
                Debug.Log("I am not Walking");
            }
            tempDirectionY -= gravity * Time.deltaTime;
            velocity.y = tempDirectionY;



#if PLATFORM_ANDROID
            transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y / 2, 0);
#endif

            controller.Move(velocity * Time.deltaTime);
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
