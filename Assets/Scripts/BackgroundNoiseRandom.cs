using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNoiseRandom : MonoBehaviour
{

    public AudioSource audiosource;
    public float minimumDelay = 1f;
    public float maximumDelay = 5f;
    public float waitTime = -1f;


    // Start is called before the first frame update
    void Start()
    {
        audiosource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audiosource.isPlaying)
        {
            if(waitTime<0f)
            {
                audiosource.Play();
                waitTime=Random.Range(minimumDelay, maximumDelay);
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
