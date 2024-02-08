using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip codycollision;
    public AudioClip dooropen;
    public AudioClip codystep;
    public AudioClip crystalset;
    public static SoundManager Instance;
    public SoundManager()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip Sound)
    {
        audiosource.PlayOneShot(Sound);
    }

    public void PlaySound(AudioClip Sound,float duration)
    {
        audiosource.PlayOneShot(Sound, duration);
    }

}
