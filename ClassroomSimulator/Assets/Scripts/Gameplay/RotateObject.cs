using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float spinForce = 45f;

    private bool isSpinning = false;

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(0, spinForce * Time.deltaTime, 0);
        }
        else if (!isSpinning)
        {
            transform.Rotate(0, 0, 0);
        }
    }

    public void ChangeSpin()
    {
        isSpinning = !isSpinning;
    }
}
