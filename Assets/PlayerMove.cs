using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController characterController;
    public float speed;
    public float jumpPow;
    public float RotateDemp;
    private float gravity;
    private float moveY;
    private Vector3 MoveDir;
    [SerializeField]
    private Animator animator;
    private void Start(){
        speed = 5f;
        gravity = 10f;
        MoveDir = Vector3.zero;
        moveY = 0f;
        jumpPow = 5f;
    }
    private void Update() {
        moveY = MoveDir.y;

        Transform CameraTransform = Camera.main.transform;
        MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        MoveDir = CameraTransform.TransformDirection(MoveDir);
        MoveDir.y = 0f;
        MoveDir.Normalize();
        
        if(MoveDir != Vector3.zero){
            animator.SetBool("Runing", true);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(MoveDir), RotateDemp * Time.deltaTime);
        }
        else{
            animator.SetBool("Runing", false);
        }
        MoveDir *= speed;
        MoveDir.y = moveY;
        if(characterController.isGrounded){
            if(Input.GetButton("Jump"))MoveDir.y = jumpPow;
        }
        else{
            MoveDir.y -= gravity * Time.deltaTime;
        }
        characterController.Move(MoveDir * Time.deltaTime);
    }
}