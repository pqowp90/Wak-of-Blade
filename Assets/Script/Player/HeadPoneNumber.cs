using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HeadPoneNumber : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI1;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI2;
    private float getYearDeley = 10f;
    //private textmesh
    private void Start(){
        StartCoroutine(GetYearCorutine());
    }
    public string GetYear()
    {
        return DateTime.Now.ToString(("yyyy"));
    }
    private IEnumerator GetYearCorutine(){
        while(true){
            int older = int.Parse(GetYear()) - 1987 + 1;
            textMeshProUGUI1.text = ""+older;
            textMeshProUGUI2.text = ""+older;
            yield return new WaitForSeconds(getYearDeley);
        }
    }
}
