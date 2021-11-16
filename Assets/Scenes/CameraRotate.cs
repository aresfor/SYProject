using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float speed;

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            speed = -speed;
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(speed == 0)
            {
                speed = 1;
            }else
            {
                speed = 0;
            }
        }
        this.transform.Rotate(Vector3.up * Time.deltaTime * this.speed);
    }
}
