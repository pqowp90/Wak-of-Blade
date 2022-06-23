using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public enum Key_UI_State{
        none,
        pickUp,
        talk,
        full,
    }
public class ShowKeyUI : MonoSingleton<ShowKeyUI>
{
    [SerializeField]
    private Image keyImage;
    [SerializeField]
    private float fadeDemp;
    private float alpha=0f;
    private CanvasGroup canvasGroup;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI2;
    private Key_UI_State key_UI_State;
    [SerializeField]
    private ContentSizeFitter contentSizeFitter;
    
    private string WhatText(Key_UI_State _key_UI_State){
        switch (_key_UI_State)
        {
            case Key_UI_State.pickUp:
            SetColor(Color.white);
            return "줍기";
            case Key_UI_State.full:
            SetColor(Color.red);
            return " 꽉 찼습니다";
            case Key_UI_State.talk:
            SetColor(Color.white);
            return "대화";
        }
        return " ";
    }
    private void SetColor(Color color){
        keyImage.color = color;
        textMeshProUGUI.color = color;
        textMeshProUGUI2.color = color;
    }
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void ShowUI(Key_UI_State _key_UI_State){
        bool changedText = (key_UI_State!=_key_UI_State);
        key_UI_State = _key_UI_State;
        textMeshProUGUI.text = WhatText(key_UI_State);
        if(changedText)
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) contentSizeFitter.transform);
        if(key_UI_State!=Key_UI_State.none)
            alpha = 1f;
    }
    public void HideUI(){
        alpha = 0f;
    }
    private void Update() {
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, alpha, fadeDemp * Time.deltaTime);
        if(alpha>0){
            alpha*=0.5f;
        }
    }
}
