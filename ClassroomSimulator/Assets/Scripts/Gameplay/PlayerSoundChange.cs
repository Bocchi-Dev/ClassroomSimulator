using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundChange : MonoBehaviour
{
    public float stepRate = 0.5f;
    public float stepCoolDown;
    public AudioClip footStepCon;
    public AudioClip footStepGrass;
    public int soundchanger = 1;

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
            if (soundchanger == 1)
            {
                walkInConcrete();
            }
            else
            {
                walkInGrass();
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grass"))
        {
            soundchanger = 0;
        }
    }

   void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Grass"))
        {
            soundchanger = 1;
        }
    }

    void walkInConcrete()
    {
        GetComponent<AudioSource>().pitch = 1f + Random.Range(-0.2f, 0.2f);
        GetComponent<AudioSource>().PlayOneShot(footStepCon);
        stepCoolDown = stepRate;
    }

    void walkInGrass()
    {
        GetComponent<AudioSource>().pitch = 1f + Random.Range(-0.2f, 0.2f);
        GetComponent<AudioSource>().PlayOneShot(footStepGrass);
        stepCoolDown = stepRate;
    }
}
