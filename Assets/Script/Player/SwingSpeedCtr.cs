using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSpeedCtr : MonoBehaviour
{
    [SerializeField]
    private PlayerMove playerMove;
    public void StartCtr(){
        playerMove.AttackStart();
    }
    public void EndCtr(){
        playerMove.AttackEnd();
    }
}
