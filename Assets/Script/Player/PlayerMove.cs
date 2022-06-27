using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class PlayerMove : MonoBehaviour
{
    private bool lastIsGrounded = false;
    public CharacterController characterController;
    private float moveY;
    private Vector3 MoveDir;
    private Vector3 realMoveDir;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    public Collider waponCollider;
    [SerializeField]
    private List<GameObject> attackedEnemy = new List<GameObject>();
    [SerializeField]
    private CameraMove cameraMove;
    public bool NoInput = false;
    private bool usingSkill = false;
    public Item nowWapon;
    public Vector3 addForce = Vector3.zero;
    [SerializeField]
    private float resistance;
    private Vector3 hitPointNormal;
    [Header("PlayerStat")]
    [SerializeField]
    private float atkPower;
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpPow;
    [SerializeField]
    private float RotateDemp;
    [SerializeField]
    private float MoveDemp;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float nowAttackSpeed;
    [SerializeField]
    private float startAttackSpeed;
    [SerializeField]
    private float slopeSpeed = 8f;
    [SerializeField]
    private float attackingTime = 0;
    private Tween attackTween;
    private bool willSlideOnSlope = true;
    private Vector3 downForce;
    private int hp;
    [SerializeField]
    Image fillBar;
    private float fillAmount;
    private float realFillAmount;
    [SerializeField]
    private float hpChangeSpeed;
    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;
    private bool movePos = false;
    private Vector2 pos;
    public void MoveTransorm(Vector3 _pos){
        pos = _pos;
        movePos = true;
    }
    private bool IsSliding{
        get{
            Debug.DrawRay(transform.position, Vector3.down*2f, Color.blue);
            if(characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f)){
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up)>characterController.slopeLimit;
            }else{
                return false;
            }
        }
    }
    private float DowmDowm{
        get{
            Debug.DrawRay(transform.position, Vector3.down*2f, Color.blue);
            if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 0.2f)){
                return Vector3.Angle(slopeHit.normal, Vector3.down)/9f;
            }else{
                return 0f;
            }
        }
    }
    public void loseHp(int _hp){
        if(hp<=0)return;
        hp -= _hp;
        if(hp<=0){
            MoveTransorm(new Vector3(19.961f, -0.453f, -38.576f));
            hp = maxHp;
        }
        textMeshProUGUI.text = ""+hp+"/"+maxHp;
    }

    private void PlayerHpBar(){
        fillAmount = hp/maxHp;
        realFillAmount = Mathf.Lerp(realFillAmount, fillAmount, Time.deltaTime * hpChangeSpeed);
        fillBar.fillAmount = realFillAmount;
    }
    public void addForceGo(Vector3 velocity){
        moveY = velocity.y;
        realMoveDir.y = velocity.y;
        addForce.x = velocity.x;
        addForce.z = velocity.z;
    }
    public void SetVelocity(Vector3 velocity){
        addForce.x = velocity.x;
        addForce.z = velocity.z;
    }
    
    private void Start(){
        hp = maxHp;
        attackedEnemy.Clear();
        MoveDir = Vector3.zero;
        moveY = 0f;
        nowAttackSpeed = startAttackSpeed;
        cameraMove = Camera.main.transform.GetComponentInParent<CameraMove>();
    }
    private void Move(){
        moveY = realMoveDir.y;

        Transform CameraTransform = Camera.main.transform;
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        if(((isNotAttacking()||!characterController.isGrounded)&&!usingSkill)){
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");
            MoveDir = horizontal * right + vertical * forward;
        }else{
            MoveDir = Vector3.zero;
        }

        MoveDir.y = 0f;
        MoveDir.Normalize();
        
        
        
        MoveDir *= speed;
        realMoveDir = Vector3.Lerp(realMoveDir, MoveDir, MoveDemp*Time.deltaTime);
        realMoveDir.y = moveY;
        animator.SetFloat("VelocityY", moveY);
        animator.SetBool("IsGround", characterController.isGrounded);
        if(characterController.isGrounded){
            if(!lastIsGrounded&&realMoveDir.y<-0.1f)realMoveDir.y = -0.1f;
            lastIsGrounded = true;
            if(Input.GetButton("Jump")&&!usingSkill&&!IsSliding){
                realMoveDir.y = jumpPow;
                animator.SetTrigger("Jump");
            }
        }
        else{
            lastIsGrounded = false;
            realMoveDir.y -= gravity * Time.deltaTime;
        }
        addForce.y = 0f;


        if(MoveDir != Vector3.zero){
            animator.SetBool("Runing", true);
            if(isNotAttacking()){
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(MoveDir), RotateDemp * Time.deltaTime);
            }
        }
        else{
            animator.SetBool("Runing", false);
        }
        
        downForce = Vector3.zero;
        if(willSlideOnSlope && IsSliding && characterController.isGrounded){
            realMoveDir += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }else if(realMoveDir.y < 0){
            downForce = -DowmDowm * Vector3.up;
        }
        if(movePos){
            characterController.enabled = false;
            movePos = false;
            transform.position = pos;
            characterController.enabled = true;
            return;
        }
        characterController.Move((realMoveDir + addForce + downForce) * Time.deltaTime);
    }
    private bool isNotAttacking(){
        return !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")&&!animator.GetBool("Attacking");
    }
    private void Attack(){
        if(Input.GetMouseButtonDown(0)&&!NoInput&&nowWapon != null&&!usingSkill){
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
    }
    public void AttackEnd(){
        waponCollider.enabled = false;
        nowAttackSpeed = startAttackSpeed;
    }
    private void SetAttackSpeed(){
        animator.SetFloat("AttackSpeed", nowAttackSpeed);
        animator.SetFloat("AirAttack", (characterController.isGrounded)?0f:1f);
        animator.SetBool("PressingJump", Input.GetKey(KeyCode.Space)&&!usingSkill);
    }
    private void Update() {
        Move();
        SetAttackSpeed();
        Attack();
        PlayerHpBar();
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("AttackWait")){
            animator.SetTrigger("GoAttack");
            AttackStart();
        }
    }
    private void FixedUpdate() {
        if(addForce.x!=0){
            addForce.x += resistance * ((addForce.x>0)?-1f:1f);
            if(Mathf.Abs(addForce.x)<3)addForce.x=0;
        }
        if(addForce.z!=0){
            addForce.z += resistance * ((addForce.z>0)?-1f:1f);
            if(Mathf.Abs(addForce.z)<3)addForce.z=0;
        }
        addForce.y = moveY;
        
        AttackSpeedUp();
    }
    public void SetUsingSkill(bool _usingSkill){
        usingSkill = _usingSkill;
    }
    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.white;
        //캐릭터 현재 속도
        //GUILayout.Label("Y보정치 : " + downForce.y, labelStyle);
        GUILayout.Label("HP : " + hp, labelStyle);
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
        enemy.TakeDamage(atkPower+nowWapon.damage);
        enemy.HitEffect();
        cameraMove.ShakeCamera(0.05f, new Vector3(0f, 0.2f, 0f), 40, 90);
    }
    private void AttackSpeedUp(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
            nowAttackSpeed = (animator.GetCurrentAnimatorStateInfo(0).normalizedTime*attackingTime)+startAttackSpeed;
        }
    }
}