using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO 实际存储在分叉树节点中的数据，如果不考虑脏标识可以把Bounds,Position去掉到时候直接从GameObject获取
public struct SceneObject
{
    public int      InstanceID;
    public Vector3  Position;
    public Bounds   Bounds;
    public Renderer Renderer;
    public GameObject Self;
}