using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioClip[] clips;
    public GameObject audioObject;
    public static AudioScript instance;

    void Awake () {
        instance = this;
    }

    public void PlaySound(Vector3 position, int index, float pitch = 1, float volume = 1, Transform follower = null) {
        AudioObjectScript aud = GameObject.Instantiate(audioObject, new Vector3(position.x,position.y,-5), Quaternion.identity).GetComponent<AudioObjectScript>();
        aud.clip = clips[index];
        aud.pitch = pitch;
        aud.volume = volume;
        aud.follow = follower;
    }
}
