using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private void Start(){
        PoolManager.CreatePool<Effect>("Blood", gameObject);
        PoolManager.CreatePool<Effect>("ChargeEffect", gameObject);
        PoolManager.CreatePool<Effect>("UppercutEffect", gameObject);
    }
    public static void CurserOnOff(bool active){
        Cursor.visible = active;
            Cursor.lockState = active?CursorLockMode.None:CursorLockMode.Locked;
    }
    
}
