using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundChange : MonoBehaviour
{
    public float stepRate = 0.5f;
    public float stepCoolDown;
    public AudioClip footStepCon;
    public AudioClip footStepGrass;
    int soundchanger = 1;
    
    public ChairBehaviour behaviour;
    
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
        if (behaviour.isSitting)
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grass"))
        {
            soundchanger = 0;
            Debug.Log("i am triggering grass");
        }
        if (other.gameObject.CompareTag("Chair"))
        {
            behaviour = other.GetComponent<ChairBehaviour>();
        }
        if (other.gameObject.CompareTag("Lounge"))
        {
            FindObjectOfType<AudioManager>().Stop("bgmusic1");
            FindObjectOfType<AudioManager>().Play("Lounge");
        }
        if (other.gameObject.CompareTag("Garden"))
        {
            FindObjectOfType<AudioManager>().Stop("bgmusic1");
            FindObjectOfType<AudioManager>().Play("Garden");
        }
    }

   void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Grass"))
        {
            soundchanger = 1;
        }
        if (other.gameObject.CompareTag("Chair"))
        {
            behaviour = null;
        }
        if (other.gameObject.CompareTag("Lounge"))
        {
            FindObjectOfType<AudioManager>().Play("bgmusic1");
            FindObjectOfType<AudioManager>().Stop("Lounge");
        }
        if (other.gameObject.CompareTag("Garden"))
        {
            FindObjectOfType<AudioManager>().Play("bgmusic1");
            FindObjectOfType<AudioManager>().Stop("Garden");
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
