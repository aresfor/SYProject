using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float RotationSpeed = 5;
    public Camera myCamera;
    public Vector3 preMousePos;
    public GameObject obj;
    public bool bDebug = false;
    private void Awake()
    {
        myCamera = GetComponent<Camera>();
    }
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if (preMousePos == Vector3.zero)
                preMousePos = Input.mousePosition;
            Vector3 mousePosition = Input.mousePosition;
            Vector3 upDir = mousePosition.y/Camera.main.scaledPixelHeight * myCamera.transform.up;
            Vector3 rightDir = mousePosition.x/ Camera.main.pixelWidth * myCamera.transform.right;
            Vector3 mouseWorldDir = upDir + rightDir;
            Vector3 preUpDir = preMousePos.y / Camera.main.scaledPixelHeight * myCamera.transform.up;
            Vector3 preRightDir = preMousePos.x / Camera.main.pixelWidth * myCamera.transform.right;
            Vector3 preMouseWorldDir = preUpDir + preRightDir;
            
            Vector3 ObjToMouseDir = -new Vector3(mouseWorldDir.x - preMouseWorldDir.x, 
                mouseWorldDir.y - preMouseWorldDir.y,0);
            Vector3 cameraWorldForward = - myCamera.transform.forward;
            //Ðý×ªÖá
            Vector3 rotationAxis = Vector3.Cross(ObjToMouseDir, cameraWorldForward);

            Vector3 distanceDir = (Input.mousePosition - preMousePos).normalized;
            float rotateDistance = Vector3.Magnitude(distanceDir * Time.deltaTime);
            obj.transform.RotateAround(obj.transform.position,
                rotationAxis, rotateDistance * RotationSpeed);

            preMousePos = Input.mousePosition;
        }

    }
#if UNITY_EDITOR
    //private void OnDrawGizmos()
    //{
    //    if(!bDebug) { return; }
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawLine(Vector3.zero, rotationAxis);
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(Vector3.zero, ObjToMouseDir);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(Vector3.zero, cameraWorldForward);
    //}
#endif
}
