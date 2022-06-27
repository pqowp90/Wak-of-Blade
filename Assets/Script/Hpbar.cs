using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour, IPoolable
{
    [SerializeField]
    Image fillBar;
    private float fillAmount;
    private float realFillAmount;
    [SerializeField]
    private float hpChangeSpeed;
    public CanvasGroup canvasGroup;
    private Transform playerTransform;
    public Transform enemyTransform;
    private bool ShowThis = false;
    private bool ShowDistance = false;
    [SerializeField]
    private float hideDistance;
    public bool inScreen;
    private void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        playerTransform = FindObjectOfType<PlayerMove>().transform;
    }
    
    public void UpdateHpbar(float _fillAmount){
        ShowThis = true;
        fillAmount = _fillAmount;
    }
    private void Update() {
        realFillAmount = Mathf.Lerp(realFillAmount, fillAmount, Time.deltaTime * hpChangeSpeed);
        fillBar.fillAmount = realFillAmount;
        PlayerDistance();
        canvasGroup.alpha=(ShowThis&&inScreen)?1f:0f;
    }
    private void PlayerDistance(){

        if((Vector3.Distance(playerTransform.position, enemyTransform.position)>=hideDistance)){
            ShowThis = false;
        }
    }
    public void OnPool()
    {
        
    }
}
