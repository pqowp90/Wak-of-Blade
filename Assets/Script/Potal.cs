using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private Vector3 pos;
    private PlayerMove player;
    [SerializeField]
    private bool isHeal;
    private void Start() {
        player = FindObjectOfType<PlayerMove>();
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("포탈 이동");
        player.MoveTransorm(pos, isHeal);
        Respawn.Instance.RespawnEveryone();
        //SceneMoveManager.Instance.SceneMove(sceneName);
    }
}
