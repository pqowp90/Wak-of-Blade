using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordLight : MonoBehaviour
{
    private Material mymat;
    private float emissionIntensity = 0f;
    private void Start(){
        mymat = GetComponent<Renderer>().material;
        DOTween.To(()=> 0, x=> emissionIntensity = x, 0.02f, 5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
    private void Update(){
        mymat.SetColor("_EmissionColor", new Color(191f, 6f, 0f, 1f)*emissionIntensity);
    }
}
