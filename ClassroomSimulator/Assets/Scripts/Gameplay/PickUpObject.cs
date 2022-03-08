using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    GameObject mainCamera;
    bool carrying;
    GameObject carriedObject;
    public float distance;
    public float smooth;
    public float throwForce = 10;

    private void Start()
    {
        mainCamera = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
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
    }

    void carry(GameObject o)
    {
        o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
    }

    void pickUp()
    {
        if (Input.GetMouseButtonDown(1))
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
                    carrying = true;
                    carriedObject = p.gameObject;
                    p.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
    void checkDrop()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dropObject();
        }

    }
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
    void ThrowObject()
    {
        carrying = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;

        carriedObject.GetComponent<Rigidbody>().AddForce(mainCamera.transform.forward * throwForce);
        carriedObject = null;
    }
}
