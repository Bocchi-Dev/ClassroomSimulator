using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Controls : NetworkBehaviour
{
    //Movement Jazz
    public CharacterController controller;

    public float moveSpeed = 10f;

    public float gravity = 10f;

    //Interaction Jazz
    public int distanceOfRaycast;
    private RaycastHit _hit;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        interact();
        PlayerMove();
    }

    void PlayerMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime);
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * moveSpeed;
        velocity = Camera.main.transform.TransformDirection(velocity);
        velocity.y -= gravity;

        //Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;

        controller.Move(velocity * Time.deltaTime);
        Move();
    }
    public void Move()
    {
      
    }

    void interact()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out _hit, distanceOfRaycast))
        {
            if (_hit.transform.CompareTag("Rotateable") && Input.GetButtonDown("Interact"))
            {
                _hit.transform.gameObject.GetComponent<RotateObject>().ChangeSpin();
            }
        }
    }

    
}
