using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkBox : MonoBehaviour, IPoolable
{
    [SerializeField]
    TextMeshProUGUI boxText;
    [SerializeField]
    private ContentSizeFitter contentSizeFitter;
    public Transform npcTransfrom;
    private CanvasGroup canvasGroup;
    private Transform playerTransform;
    public bool inScreen = false;
    public bool ShowThis = false;
    [SerializeField]
    private float hideDistance;
    [SerializeField]
    private Sprite TextBoxSprite;
    [SerializeField]
    private Sprite FeelingSignSprite;
    private Image image;
    private void Start() {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        playerTransform = FindObjectOfType<PlayerMove>().transform;
    }
    public void UpdateText(string _text){
        if(_text=="!!!"){
            image.sprite = FeelingSignSprite;
            boxText.text = "";
        }else{
            image.sprite = TextBoxSprite;
            boxText.text = _text;
        }
        ShowThis = true;
        
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) contentSizeFitter.transform);
    }
    
    private void Update() {
        PlayerDistance();
        canvasGroup.alpha=(ShowThis&&inScreen)?1f:0f;
    }
    private void PlayerDistance(){

        if((Vector3.Distance(playerTransform.position, npcTransfrom.position)>=hideDistance)){
            ShowThis = false;
        }
    }

    public void OnPool()
    {
        
    }
}
