using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObjectScript : MonoBehaviour
{
    AudioSource aud;
    public AudioClip clip;
    public float pitch = 1;
    public float volume = 1;
    public Transform follow;
    bool following;
    void Start()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.clip = clip;
        aud.pitch = pitch;
        aud.volume = volume;
        aud.Play();
        following = follow != null;
    }

    // Update is called once per frame
    void Update()
    {
        if (follow != null) transform.position = new Vector3(follow.position.x,follow.position.y,-5);
        if (following && follow == null) Destroy(gameObject);
        if (!aud.isPlaying) Destroy(gameObject);
    }
}
