using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUse : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer[] trails = new TrailRenderer[4];
    private PlayerMove playerMove;
    private CameraMove cameraMove;
    private CharacterController characterController;
    private bool Charging = false;
    [SerializeField]
    private float chargeSpeed;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Vector3 forwardCenter;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float timeDemp;
    private float tralTime=0f;
    private void Start() {
        playerMove = GetComponent<PlayerMove>();
        cameraMove = FindObjectOfType<CameraMove>();
        characterController = GetComponent<CharacterController>();
    }
    private void Charge(){
        playerMove.SetUsingSkill(true);
        TrailsOn(true);
        animator.SetTrigger("ChargeStart");
        cameraMove.SlowMouse(true);
        Charging = true;
    }
    private void ChargeOff(){
        playerMove.SetUsingSkill(false);
        TrailsOn(false);
        animator.SetTrigger("ChargeEnd");
        cameraMove.SlowMouse(false);
        Charging = false;
    }
    private void TrailsOn(bool active){
        tralTime = active?0.4f:0f;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            Charge();
        }
        if(Charging){
            Quaternion cameraQ = Quaternion.Euler(0f, cameraMove.transform.localEulerAngles.y, 0f);
            
            characterController.Move(cameraQ * Vector3.forward * chargeSpeed * Time.deltaTime);
            transform.rotation = cameraQ;
        }
        ChackForward();
        foreach(TrailRenderer trail in trails){
            trail.time = Mathf.Lerp(trail.time, tralTime, timeDemp * Time.deltaTime);
        }
    }
    [SerializeField]
    private float radius;
    private void ChackForward(){
        if(!Charging)return;
        if(Physics.OverlapSphere(transform.position + transform.rotation*forwardCenter, radius, 1<<LayerMask.NameToLayer("WorldObject")).Length>0){
            ChargeOff();
        }
    }
    private void OnDrawGizmos(){
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position + transform.rotation*forwardCenter, radius);
    }
}
