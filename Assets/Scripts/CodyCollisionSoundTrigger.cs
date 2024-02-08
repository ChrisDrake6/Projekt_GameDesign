using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodyCollisionSoundTrigger : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip Audioclip;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        audiosource.PlayOneShot(Audioclip, 1f);
    }
}
