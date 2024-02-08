using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class DoorSoundEffect : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip Audioclip;
    public GameObject Door1;
    public GameObject Door2;
    public GameObject Door3;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Door1.activeSelf==false)
        {
            audiosource.PlayOneShot(Audioclip, 0.5f);
        }

        if (Door2.activeSelf == false)
        {
            audiosource.PlayOneShot(Audioclip, 0.5f);
        }

        if (Door3.activeSelf == false)
        {
            audiosource.PlayOneShot(Audioclip, 0.5f);
        }


    }
}


