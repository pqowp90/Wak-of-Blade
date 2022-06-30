using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public void ReStart(){
        SceneManager.LoadScene("Home");
    }
    public void Quit(){
        Application.Quit();
    }
}
