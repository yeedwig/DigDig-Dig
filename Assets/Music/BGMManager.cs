using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

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
        audioSource.volume = volume;

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

        audioSource.volume = volume;



    }

    void changeVolumeByDepth()
    {
        if(player.transform.position.y < -10)
        {
            volume -= 0.05f;
        }
    }


    public void changeBGM(int _index, float _volume)
    {
        audioSource.Stop();
        index = _index;
        audioSource.clip = bgms[index];
        audioSource.volume = volume;

        audioSource.Play();
    }
}
