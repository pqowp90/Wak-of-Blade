using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private void Awake(){
        PoolManager.CreatePool<Effect>("Blood", gameObject);
        PoolManager.CreatePool<Effect>("ChargeEffect", gameObject);
        PoolManager.CreatePool<Effect>("ChargeStart", gameObject);
        PoolManager.CreatePool<Effect>("HitImpact", gameObject);
        PoolManager.CreatePool<Effect>("AngGiMo", gameObject);
        PoolManager.CreatePool<Effect>("Ddeack", gameObject);
        PoolManager.CreatePool<Effect>("Jump", gameObject);
        PoolManager.CreatePool<Effect>("UppercutEffect", gameObject);
        PoolManager.CreatePool<Hpbar>("HpBar", gameObject);
        PoolManager.CreatePool<TalkBox>("TalkBox", gameObject);
        CurserOnOff(false);
    }
    public static void CurserOnOff(bool active){
        Cursor.visible = active;
            Cursor.lockState = active?CursorLockMode.None:CursorLockMode.Locked;
    }
    public static bool InScreen(Transform ObjTransform){
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        var point = ObjTransform.position;
        foreach (var plane in planes)
        {
            if(plane.GetDistanceToPoint(point)<0){
                return false;
            }
        }
        return true;
    }
    
}
