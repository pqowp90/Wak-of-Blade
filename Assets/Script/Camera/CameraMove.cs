using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraMove : MonoBehaviour
{
    private Transform realCamera;
    public Transform objTargetTransform;
    public float distance, heightDamping, rotationDamping, posDemping;
    private Vector3 targetPos;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float mouseSpeed;
    private float mouseX;
    private float mouseY;
    private float mouseX2=1f;
    private float mouseY2=1f;
    public bool NoInput;
    public void SlowMouse(bool active){
        if(active){
            mouseX2 = 0.1f;
            mouseY2 = 0.2f;
        }else{
            mouseX2 = 1f;
            mouseY2 = 1f;
        }
    }
    private void Start(){
        realCamera = transform.GetChild(0);
    }
    public void ShakeCamera(){
        realCamera.DOShakeRotation(0.1f, 3f, 50, 90f, true);
    }
    void LateUpdate()
    {
        CameraRotate();
        CameraMoveFunc();
    }
    private void CameraMoveFunc(){
        targetPos = Vector3.Lerp(targetPos, objTargetTransform.position, posDemping * Time.deltaTime);
        Vector3 lookDirect = -(transform.forward * distance);
        RaycastHit raycastHit;
        if(Physics.Raycast(targetPos, lookDirect, out raycastHit, lookDirect.magnitude, layerMask)){
            transform.position = raycastHit.point;
        }else{
            transform.position = targetPos + lookDirect;
        }
    }
    private void CameraRotate(){
        if(NoInput)return;
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed * mouseX2;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed * mouseY2;

        mouseX = (mouseX > 180.0f) ? mouseX - 360.0f : mouseX;
        mouseY = (mouseY > 180.0f) ? mouseY - 360.0f : mouseY;
        mouseY = Mathf.Clamp(mouseY, -90, 90);

        transform.localRotation = Quaternion.Euler(-mouseY, mouseX, 0f);
    }
}
