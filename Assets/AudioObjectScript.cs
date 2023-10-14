using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObjectScript : MonoBehaviour
{
    AudioSource aud;
    public AudioClip clip;
    public float pitch = 1;
    public float volume = 1;
    void Start()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.clip = clip;
        aud.pitch = pitch;
        aud.volume = volume;
        aud.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!aud.isPlaying) Destroy(gameObject);
    }
}
