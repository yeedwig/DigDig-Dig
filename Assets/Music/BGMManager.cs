using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgms;
    private AudioSource audioSource;
    public int index = 0;
    public float volume;
    public bool playLoop;

    public bool canPlayMusic;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgms[index];
        
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(playLoop)
        {
            audioSource.loop = true;
        }
        else
        {
            audioSource.loop = false;
        }
        /*
        if(!canPlayMusic)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }*/

        audioSource.volume = volume;
        audioSource.clip = bgms[index];
    }
}
