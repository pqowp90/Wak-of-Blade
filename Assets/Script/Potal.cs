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
    private void Start() {
        player = FindObjectOfType<PlayerMove>();
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("포탈 이동");
        player.MoveTransorm(pos);
        Respawn.Instance.RespawnEveryone();
        //SceneMoveManager.Instance.SceneMove(sceneName);
    }
}
