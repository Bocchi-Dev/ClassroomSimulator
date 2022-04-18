using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundChange : MonoBehaviour
{
    public float stepRate = 0.5f;
    public float stepCoolDown;
    public AudioClip footStep;

    public CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stepCoolDown -= Time.deltaTime;
        if (cc.velocity.magnitude > 2f && stepCoolDown < 0f && cc.isGrounded == true)
        {
            GetComponent<AudioSource>().pitch = 1f + Random.Range(-0.2f, 0.2f);
            GetComponent<AudioSource>().PlayOneShot(footStep);
            stepCoolDown = stepRate;
        }
    }

}
