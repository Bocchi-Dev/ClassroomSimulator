using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirdMovement : MonoBehaviour
{

    //public Transform endPos;
    //Vector3 endPos2;
    //Rigidbody rb;
    //public float speed = 5f;

    public float HorizontalSpeed = 0.02f;
    public float VerticalSpeed = 1f;
    public float otherSpeed = 0.02f;
    public float Amplitude = 2f;
    

    float newZSpeed;
    float newHSpeed;
    float newAmp;


    Vector3 tempPosition;

    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        // endPos2 = endPos.transform.position;
      
        tempPosition = transform.position;
        InvokeRepeating("NewVal", 0.01f, 20);

    }

    void Update()
    {
        
       

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // transform.position = Vector3.MoveTowards(transform.position, endPos2, Time.deltaTime * speed);
        
        tempPosition.x += HorizontalSpeed;
        tempPosition.y = 45f + Mathf.Sin(Time.realtimeSinceStartup * VerticalSpeed) * Amplitude;
        tempPosition.z += otherSpeed;
        transform.position = tempPosition;
    }

    void NewVal()
    {
        newZSpeed = Random.Range(-0.07f, 0.07f);
        newHSpeed = Random.Range(-0.07f, 0.07f);
        newAmp = Random.Range(1f, 2f);
        HorizontalSpeed = newHSpeed;
        otherSpeed = newZSpeed;
        Amplitude = newAmp;
    }

  
}
