using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGoldManager : MonoSingleton<PlayerGoldManager>
{
    private void Start() {
        //저장된 골드 불러오기
        textMeshProUGUI.text = gold.ToString();
    }
    [SerializeField]
    private long gold = 0;
    public void AddGold(int _gold){
        gold += _gold;
        SetGoldUI();
    }
    public void UseGold(int _gold){
        gold -= _gold;
        SetGoldUI();
    }
    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;
    private void SetGoldUI(){
        textMeshProUGUI.text = gold.ToString();
    }
    public long GetGold(){
        return gold;
    }
    
}
