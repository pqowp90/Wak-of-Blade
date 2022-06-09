using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float attackDistance;
    [SerializeField]
    private float hp = 10;
    public float speed = 10;
    CharacterController characterController;
    private Renderer enemyRenderer;
    GameObject target;
    public enum State{
        Move,
        Attack,
    }
    [SerializeField]
    State monsterState = State.Move;

    private void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        characterController = GetComponent<CharacterController>();
        target = FindObjectOfType<PlayerMove>().gameObject;
    }

    private void Update()
    {

        ChangeState();
        Vector3 nextPos = target.transform.position - transform.position;
        nextPos.Normalize();
        nextPos *= speed * Time.deltaTime;

        Vector3 newForward = characterController.velocity;
        newForward.y = 0;

        if (newForward.magnitude > 0)
            transform.forward = Vector3.Lerp(transform.forward, newForward, 5 * Time.deltaTime);

        if (!characterController.isGrounded)
        {
            nextPos.y -= 9.8f * Time.deltaTime;
        }
        if(monsterState == State.Move)
            characterController.Move(nextPos);
        if(monsterState == State.Attack)
            enemyRenderer.material.color = new Color(1f, 0f, 0f);
        else
            enemyRenderer.material.color = new Color(1f, 1f, 1f);
    }
    private void ChangeState(){
        if(Vector3.Distance(transform.position, target.transform.position)>attackDistance){
            monsterState = State.Move;
        }else{
            monsterState = State.Attack;
        }
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;

        Debug.Log(hp);

        if (hp <= 0)
        {
            Debug.Log("사망");
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wapon"))
        {
            Debug.Log("아야");
            StartCoroutine(ChangeColor());
        }
    }
    private IEnumerator ChangeColor(){
        transform.GetComponent<Renderer>().material.DOColor(Color.green, 0.5f);
        yield return new WaitForSeconds(0.5f);
        transform.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
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

}