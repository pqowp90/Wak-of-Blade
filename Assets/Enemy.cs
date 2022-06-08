using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float attackDistance;
    float hp = 10;
    public float speed = 10;
    CharacterController characterController;

    GameObject target;
    public enum State{
        Move,
        Attack,
    }
    [SerializeField]
    State monsterState = State.Move;

    private void Start()
    {
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
            transform.Rotate(transform.eulerAngles + (new Vector3(1f, 0f, 0f))); 
    }
    private void ChangeState(){
        if(Vector3.Distance(transform.position, target.transform.position)>attackDistance && monsterState != State.Move){
            transform.rotation = Quaternion.identity;
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Wapon"))
        {
            Debug.Log("아야");
            transform.GetComponent<Renderer>().material.DOColor(Color.red, 3);
        }
    }

}