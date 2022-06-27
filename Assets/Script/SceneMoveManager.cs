using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoSingleton<SceneMoveManager>
{
    
    public void SceneMove(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}
