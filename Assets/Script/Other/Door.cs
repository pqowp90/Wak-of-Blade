using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Quaternion openRotation;
    [SerializeField]
    private Quaternion closeRotation;
    private void OnTriggerEnter(Collider other) {
        transform.GetChild(0).rotation = openRotation;
    }
    private void OnTriggerExit(Collider other) {   
        transform.GetChild(0).rotation = closeRotation;
    }
}
