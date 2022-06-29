using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraMove : MonoBehaviour
{
    private Transform realCamera;
    private Transform fakeCamera;
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
    [SerializeField]
    private Transform emptyObject;
    public void SlowMouse(bool active){
        if(active){
            mouseX2 = 0.05f;
            mouseY2 = 0.1f;
        }else{
            mouseX2 = 1f;
            mouseY2 = 1f;
        }
    }
    private void Start(){
        fakeCamera = transform.GetChild(0);
        realCamera = fakeCamera.GetChild(0);
    }
    public void ShakeCamera(float duration, float strength = 1, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true){
        realCamera.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut).OnComplete(()=>{emptyObject.position = Vector3.zero;});
        
    }
    public void ShakeCamera(float duration, Vector3 strength, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true){
        realCamera.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut).OnComplete(()=>{emptyObject.position = Vector3.zero;});
        
    }
    public void ShakeCameraRotation(float duration, float strength = 90, int vibrato = 10, float randomness = 90, bool fadeOut = true){
        realCamera.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut);
    }
    void LateUpdate()
    {   
        fakeCamera.localPosition = Vector3.Lerp(fakeCamera.localPosition, -realCamera.localPosition, 0.03f);
        fakeCamera.localRotation = Quaternion.Lerp(fakeCamera.localRotation, Quaternion.Inverse(realCamera.localRotation), 0.05f);
        CameraRotate();
        CameraMoveFunc();
    }
    private void CameraMoveFunc(){
        targetPos = Vector3.Lerp(targetPos, objTargetTransform.position, posDemping * Time.deltaTime);
        Vector3 lookDirect = -(transform.forward * distance);
        RaycastHit raycastHit;
        if(Physics.Raycast(targetPos, lookDirect, out raycastHit, lookDirect.magnitude, layerMask)){
            transform.position = Vector3.Lerp(objTargetTransform.position, raycastHit.point, 0.97f);
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
