using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ChangeSong")
        {
            FindObjectOfType<AudioManager>().Stop("bgmusic1");
            FindObjectOfType<AudioManager>().Play("bgmusic2");
            Debug.Log("Music Off");
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
    }
}
