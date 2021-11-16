using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBounds : MonoBehaviour
{
    Bounds bounds;
    //SphereCollider s = new SphereCollider()
    //{
    //    center = new Vector3(1, 0, 0),
    //    radius = 1,
    //};
    // Start is called before the first frame update
    void Start()
    {
        bounds = new Bounds(Vector3.zero, Vector3.one);
        Debug.Log("size:" + bounds.size + " min:" + bounds.min + " max:"+bounds.max);

        //Debug.Log(s.ClosestPoint(Vector3.one));
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(bounds.center, bounds.size);
        
    }


}
