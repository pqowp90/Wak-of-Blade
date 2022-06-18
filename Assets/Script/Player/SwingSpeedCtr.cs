using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSpeedCtr : MonoBehaviour
{
    [SerializeField ]
    private bool isAttacking = false;
    [SerializeField]
    private PlayerMove playerMove;
    public void StartCtr(){
        isAttacking = true;
        playerMove.AttackStart();
    }
    public void EndCtr(){
        isAttacking = false;
        playerMove.AttackEnd();
    }
}
