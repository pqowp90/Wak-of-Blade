using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform objTargetTransform;
    public float height, distance, heightDamping, rotationDamping, posDemping;
    private Vector3 targetPos;

    void LateUpdate()
    {
        CameraMoveFunc();
    }
    private void CameraMoveFunc(){
        float nowHeight = transform.position.y;
        nowHeight = Mathf.Lerp(nowHeight, objTargetTransform.position.y + height, heightDamping * Time.deltaTime);
        targetPos = objTargetTransform.position;
        targetPos -= transform.forward * distance;
        transform.position = Vector3.Lerp(transform.position, targetPos, posDemping * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, nowHeight, transform.position.z);
    }
}
