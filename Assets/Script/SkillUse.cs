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
    private bool Uppercuting = false;
    [SerializeField]
    private float chargeSpeed;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Vector3 forwardCenter1, forwardCenter2, forwardCenter3;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float timeDemp;
    private float tralTime=0f;
    [SerializeField]
    private LayerMask layermask;
    [SerializeField]
    private Transform ChargeEffectPos;
    [SerializeField]
    private Transform UppercutEffectPos;
    private Coroutine UppercutCoroutine;
    private bool canUseCharge;
    private bool canUseUppercut;
    public void UnlockSkillType(UnlockSkillType unlockSkillType){
        switch(unlockSkillType){
            case global::UnlockSkillType.charge:
                canUseCharge = true;
                break;
            case global::UnlockSkillType.uppercut:
                canUseUppercut = true;
                break;
        }
    }
    private void Start() {
        playerMove = GetComponent<PlayerMove>();
        cameraMove = FindObjectOfType<CameraMove>();
        characterController = GetComponent<CharacterController>();
        
    }
    private void Charge(){
        if(UppercutCoroutine!=null){
            StopCoroutine(UppercutCoroutine);
            Uppercuting = false;
            TrailsOn(false, true);
            playerMove.SetUsingSkill(false);
        }
        

        playerMove.SetUsingSkill(true);
        TrailsOn(true, false);
        animator.SetTrigger("ChargeStart");
        cameraMove.SlowMouse(true);
        Charging = true;
        
    }
    private IEnumerator Uppercut(){
        playerMove.SetUsingSkill(true);
        Uppercuting  = true;
        if(UppercutCoroutine!=null)
            StopCoroutine(UppercutCoroutine);
        animator.ResetTrigger("UppercutOff");
        animator.SetTrigger("Uppercut");
        yield return new WaitForSeconds(0.25f);
        playerMove.addForceGo(transform.rotation*new Vector3(0f, 9f, 5f));
        cameraMove.ShakeCamera(0.15f, Vector3.up, 40, 90);
        playerMove.SetUsingSkill(false);
        TrailsOn(true, false);

        Transform effectTransform = PoolManager.GetItem<Effect>("UppercutEffect").transform;
        effectTransform.position = UppercutEffectPos.position;
        effectTransform.rotation = UppercutEffectPos.rotation;
        Collider[] colliders = Physics.OverlapSphere(UppercutEffectPos.position + Vector3.up, radius*2f, layermask);
        AttackByCollider(colliders);
        effectTransform.SetParent(null);


        yield return new WaitForSeconds(0.2f);
        animator.SetTrigger("UppercutOff");
        Uppercuting  = false;
        yield return new WaitForSeconds(0.1f);
        TrailsOn(false, true);
    }
    private void ChargeOff(){

        Transform effectTransform = PoolManager.GetItem<Effect>("ChargeEffect").transform;
        effectTransform.position = ChargeEffectPos.position;
        effectTransform.rotation = ChargeEffectPos.rotation;
        effectTransform.SetParent(null);


        cameraMove.ShakeCamera(0.4f, 1, 20, 90);
        playerMove.addForceGo(transform.rotation*new Vector3(0f, 3f, -13f));
        playerMove.SetUsingSkill(false);
        TrailsOn(false, false);
        animator.SetTrigger("ChargeEnd");
        cameraMove.SlowMouse(false);
        Charging = false;
    }
    private void TrailsOn(bool active, bool now){
        tralTime = active?0.4f:0f;
        if(now){
            foreach(TrailRenderer trail in trails){
                trail.time = tralTime;
            }
        }
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            if(!Uppercuting&&!Charging&&canUseCharge)
                Charge();
        }
        if(Input.GetMouseButtonDown(1)&&canUseUppercut){
            if(!Uppercuting&&!Charging)
                UppercutCoroutine = StartCoroutine(Uppercut());
        }
        if(Charging){
            

            
            Quaternion cameraQ = Quaternion.Euler(0f, cameraMove.transform.localEulerAngles.y + Input.GetAxisRaw("Horizontal") * 10f, 0f);
            
            //haracterController.Move(cameraQ * Vector3.forward * chargeSpeed * Time.deltaTime);
            playerMove.SetVelocity(cameraQ * Vector3.forward * chargeSpeed);
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
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.rotation*forwardCenter1, radius, layermask);
        Collider[] colliders2 = Physics.OverlapSphere(transform.position + transform.rotation*forwardCenter2, radius, layermask);
        Collider[] colliders3 = Physics.OverlapSphere(transform.position + transform.rotation*forwardCenter3, radius, layermask);
        if(colliders.Length>0){
            
            ChargeOff();
            AttackByCollider(colliders);
        }
        else if(colliders2.Length>0){
            ChargeOff();
            AttackByCollider(colliders2);
        }
        else if(colliders3.Length>0){
            ChargeOff();
            AttackByCollider(colliders3);
        }
    }
    private void AttackByCollider(Collider[] colliders){
        foreach(var enemy in colliders){
            if(enemy.CompareTag("Enemy")){
                enemy.GetComponent<Enemy>().TakeDamage(50);
                break;
            }
        }
    }


    private void OnDrawGizmos(){
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position + transform.rotation*forwardCenter1, radius);
        Gizmos.DrawSphere(transform.position + transform.rotation*forwardCenter2, radius);
        Gizmos.DrawSphere(transform.position + transform.rotation*forwardCenter3, radius);
    }
}
