using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> audioClips = new List<AudioClip>();
    private AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public void Play(){
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count-1)]);
    }
}
