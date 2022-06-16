using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
    [SerializeField]
    private float attackSpeed=0;
    [SerializeField]
    private float nowAttackSpeed;
    [SerializeField]
    private float startAttackSpeed;
    [SerializeField]
    private float attackingTime = 0;
    private Tween attackTween;
    [SerializeField]
    private float atkPower;
    [SerializeField]
    private Collider waponCollider;
    [SerializeField]
    private Ease speedUpTweenType = Ease.InOutCubic;
    private List<GameObject> attackedEnemy = new List<GameObject>();
    private CameraMove cameraMove;
    
    private void Start(){
        attackedEnemy.Clear();
        speed = 5f;
        gravity = 10f;
        MoveDir = Vector3.zero;
        moveY = 0f;
        jumpPow = 5f;
        nowAttackSpeed = startAttackSpeed;
        cameraMove = Camera.main.transform.GetComponentInParent<CameraMove>();
    }
    private void Move(){
        moveY = MoveDir.y;

        Transform CameraTransform = Camera.main.transform;
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        if((!animator.GetBool("Attacking"))||(!characterController.isGrounded && animator.GetBool("Attacking"))){
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");
            MoveDir = horizontal * right + vertical * forward;
        }else{
            MoveDir = Vector3.zero;
        }

        MoveDir.y = 0f;
        MoveDir.Normalize();
        
        if(MoveDir != Vector3.zero){
            animator.SetBool("Runing", true);
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(MoveDir), RotateDemp * Time.deltaTime);
            }
        }
        else{
            animator.SetBool("Runing", false);
        }
        MoveDir *= speed;
        MoveDir.y = moveY;
        animator.SetFloat("VelocityY", moveY);
        animator.SetBool("IsGround", characterController.isGrounded);
        if(characterController.isGrounded){
            if(Input.GetButton("Jump")){
                MoveDir.y = jumpPow;
                animator.SetTrigger("Jump");
            }
        }
        else{
            MoveDir.y -= gravity * Time.deltaTime;
        }

        characterController.Move(MoveDir * Time.deltaTime);
    }
    private void Attack(){
        if(Input.GetMouseButtonDown(0)){
            animator.SetBool("Attacking", true);
            
        }
        if(Input.GetMouseButtonUp(0)){
            animator.SetBool("Attacking", false);
        }
    }
    public void AttackStart(){
        attackedEnemy.Clear();
        SetAttackSpeed();
        waponCollider.enabled = true;
        //attackTween = DOTween.To(()=> nowAttackSpeed, x=> nowAttackSpeed = x, attackSpeed, attackingTime).SetEase(speedUpTweenType);
    }
    public void AttackEnd(){
        waponCollider.enabled = false;
        nowAttackSpeed = startAttackSpeed;
        //attackTween.Kill();
    }
    private void SetAttackSpeed(){
        animator.SetFloat("AttackSpeed", nowAttackSpeed);
        animator.SetFloat("AirAttack", (characterController.isGrounded)?0f:1f);
        animator.SetBool("PressingJump", Input.GetKey(KeyCode.Space));
    }
    private void Update() {
        Move();
        Attack();
        SetAttackSpeed();
    }
    private void FixedUpdate() {
        AttackSpeedUp();
    }
    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.white;
        //캐릭터 현재 속도
        GUILayout.Label("현재 공격속도 : " + nowAttackSpeed, labelStyle);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 충돌된 물체의 릿지드 바디를 가져옴
        Rigidbody body = hit.collider.attachedRigidbody;

        // 만약에 충돌된 물체에 콜라이더가 없거나, isKinematic이 켜저있으면 리턴
        if (body == null || body.isKinematic) return;

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        // pushDir이라는 벡터값에 새로운 백터값 저장. 부딪힌 물체의 x의 방향과 y의 방향을 저장
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        // 부딪힌 물체의 릿지드바디의 velocity에 위에 저장한 백터 값과 힘을 곱해줌
        body.velocity = pushDir * 4f;
    }
    public void Attacking(GameObject obj)
    {
        if(attackedEnemy.Find(x => x == obj) != null)return;
        attackedEnemy.Add(obj);
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.TakeDamage(atkPower);
        enemy.HitEffect();
        cameraMove.ShakeCamera();
    }
    private void AttackSpeedUp(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
            nowAttackSpeed = (animator.GetCurrentAnimatorStateInfo(0).normalizedTime*attackingTime)+startAttackSpeed;
        }
    }
}