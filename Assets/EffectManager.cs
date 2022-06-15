using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PoolManager.CreatePool<Effect>("Blood", gameObject);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
