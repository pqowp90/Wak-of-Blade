using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips;
    private AudioSource audioSource;
    private Animator myAnimator;
    private CharacterController characterController;
    void Start(){
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponentInParent<CharacterController>();

    }
    public void Tak(int hihi){
        if(!characterController.isGrounded)return;
        audioSource.clip = audioClips[Random.Range(0,audioClips.Length-1)];
        audioSource.Play();

    }

}