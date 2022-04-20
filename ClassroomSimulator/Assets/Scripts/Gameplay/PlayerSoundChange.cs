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
        if (other.gameObject.name == "ChangeSong")
        {
            FindObjectOfType<AudioManager>().Stop("bgmusic1");
            FindObjectOfType<AudioManager>().Play("bgmusic2");
            Debug.Log("Music Off");
        }
        if (other.gameObject.CompareTag("Chair"))
        {
            behaviour = other.GetComponent<ChairBehaviour>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ChangeSong")
        {
            FindObjectOfType<AudioManager>().Play("bgmusic1");
            FindObjectOfType<AudioManager>().Stop("bgmusic2");
            Debug.Log("Music On");
        }
        if (other.gameObject.CompareTag("Chair"))
        {
            behaviour = null;
        }
    }
}
