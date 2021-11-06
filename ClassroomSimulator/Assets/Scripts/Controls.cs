using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if(Physics.Raycast(ray, out _hit, distanceOfRaycast))
        {
            if(Input.GetButtonDown("Interact") && _hit.transform.CompareTag("Rotateable"))
            {
                _hit.transform.gameObject.GetComponent<RotateObject>().ChangeSpin();
            }
        }
        playerMovement();
    }

    void playerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
        Vector3 velocity = direction * moveSpeed;
        velocity = Camera.main.transform.TransformDirection(velocity);
        velocity.y = 0; //to prevent character from flying when moving forward while looking up

        controller.Move(velocity * Time.deltaTime);
    }

    void interact()
    {
        if (Input.GetButton("Interact"))
        {

        }
    }
}
