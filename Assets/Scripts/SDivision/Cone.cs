using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//相机视锥体类
[Serializable]
public struct Cone
{
    private static Matrix4x4 ClipMatrix;

    [SerializeField] private float m_FOV;
    [SerializeField] private float m_Near;
    [SerializeField] private float m_Far;
    [SerializeField] private float m_Aspect;
    [SerializeField] private float m_NearClipPlaneHeight;
    [SerializeField] private float m_FarClipPlaneHeight;
    [SerializeField] private float m_NearClipPlaneWidth;
    [SerializeField] private float m_FarClipPlaneWidth;
    [SerializeField] private float m_FarNearAspect;

    [SerializeField] public Camera    m_Camera;
    private                 Plane[]   m_Planes;

    public float Near
    {
        get { return this.m_Near; }
        set { this.m_Near = value; }
    }

    public float Far
    {
        get { return this.m_Far; }
        set { this.m_Far = value; }
    }

    public float FOV
    {
        get { return this.m_FOV; }
        set { this.m_FOV = value; }
    }

    public Camera RenderCamera
    {
        get { return this.m_Camera; }
        set { this.m_Camera = value; }
    }

    public Cone(float near, float far, float fov)
    {
        this.m_Near = near;
        this.m_Far = far;
        this.m_FOV = fov;

        this.m_NearClipPlaneHeight = 0;
        this.m_FarClipPlaneHeight = 0;
        this.m_NearClipPlaneWidth = 0;
        this.m_FarClipPlaneWidth = 0;
        this.m_FarNearAspect = 0;
        this.m_Aspect = 0;
        this.m_Camera = null;
        this.m_Planes = null;
    }

    public void InityCone(float aspect)
    {
        this.m_NearClipPlaneHeight = 2 * this.m_Near * (float) Math.Tan(this.m_FOV / 2 * Mathf.Deg2Rad);
        this.m_NearClipPlaneWidth = aspect * this.m_NearClipPlaneHeight;

        this.m_FarClipPlaneHeight =
            2 * this.Far * (float) Math.Tan(this.m_FOV / 2 * Mathf.Deg2Rad);
        this.m_FarClipPlaneWidth = aspect * this.m_FarClipPlaneHeight;

        this.m_FarNearAspect = (this.m_NearClipPlaneWidth / this.m_NearClipPlaneHeight) /
                               (this.m_FarClipPlaneWidth / this.m_FarClipPlaneHeight);
        this.m_Aspect = aspect;
    }

    public void DebugDrawCone(Vector3 position, Quaternion rotate)
    {
        //Near
        Vector3 nearLeftBottom = new Vector3(-(float) this.m_NearClipPlaneWidth / 2,
            -(float) this.m_NearClipPlaneHeight / 2,
            this.m_Near);
        Vector3 nearLeftTop = new Vector3(-(float) this.m_NearClipPlaneWidth / 2,
            (float) this.m_NearClipPlaneHeight / 2,
            this.m_Near);

        Vector3 nearRightBottom = new Vector3((float) this.m_NearClipPlaneWidth / 2,
            -(float) this.m_NearClipPlaneHeight / 2,
            this.m_Near);
        Vector3 nearRightTop = new Vector3((float) this.m_NearClipPlaneWidth / 2,
            (float) this.m_NearClipPlaneHeight / 2,
            this.m_Near);

        //Far
        Vector3 farLeftBottom = new Vector3(-(float) this.m_FarClipPlaneWidth / 2,
            -(float) this.m_FarClipPlaneHeight / 2,
            this.m_Far);
        Vector3 farLeftTop = new Vector3(-(float) this.m_FarClipPlaneWidth / 2, (float) this.m_FarClipPlaneHeight / 2,
            this.m_Far);

        Vector3 farRightBottom = new Vector3((float) this.m_FarClipPlaneWidth / 2,
            -(float) this.m_FarClipPlaneHeight / 2,
            this.m_Far);
        Vector3 farRightTop = new Vector3((float) this.m_FarClipPlaneWidth / 2, (float) this.m_FarClipPlaneHeight / 2,
            this.m_Far);


        Vector3 leftbottom = position + rotate * nearLeftBottom;
        Vector3 lefttop    = position + rotate * nearLeftTop;

        Vector3 rightbottom = position + rotate * nearRightBottom;
        Vector3 righttop    = position + rotate * nearRightTop;


        Vector3 fleftbottom = position + rotate * farLeftBottom;
        Vector3 flefttop    = position + rotate * farLeftTop;

        Vector3 frightbottom = position + rotate * farRightBottom;
        Vector3 frighttop    = position + rotate * farRightTop;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(leftbottom,  rightbottom);
        Gizmos.DrawLine(lefttop,     righttop);
        Gizmos.DrawLine(leftbottom,  lefttop);
        Gizmos.DrawLine(rightbottom, righttop);

        Gizmos.DrawLine(fleftbottom,  frightbottom);
        Gizmos.DrawLine(flefttop,     frighttop);
        Gizmos.DrawLine(fleftbottom,  flefttop);
        Gizmos.DrawLine(frightbottom, frighttop);


        Gizmos.DrawLine(fleftbottom,  leftbottom);
        Gizmos.DrawLine(flefttop,     lefttop);
        Gizmos.DrawLine(frighttop,    righttop);
        Gizmos.DrawLine(frightbottom, rightbottom);
    }
}