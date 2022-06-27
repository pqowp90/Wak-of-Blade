using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour, IPoolable
{
    [SerializeField]
    private float lifeTime;
    public void OnPool()
    {
        StartCoroutine(distroyEffect());
    }
    private IEnumerator distroyEffect(){
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
        transform.SetParent(GameManager.Instance.transform);
    }
}
