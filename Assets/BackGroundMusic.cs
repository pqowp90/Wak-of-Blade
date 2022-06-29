using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip nowAudioClip;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Music")){
            nowAudioClip = other.GetComponent<Music>().music;
            audioSource.clip = nowAudioClip;
            audioSource.time = 0f;
            audioSource.Play();
        }
    }
}
