using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;
//SceneTree只是逻辑上放在场景中的自定义边界的立方体，由SceneTree的边界开始创建分叉树
[ExecuteInEditMode]
public class SceneTree : MonoBehaviour
{
    [SerializeField] private string            m_TreeName;
    [SerializeField] private Vector3           m_SceneTreeSize;
    [SerializeField] private Bounds            m_SceneTreeNodeBounds;
    [SerializeField] private Bounds            m_SceneTreeBounds;
    [SerializeField] private Vector3           m_Max;
    [SerializeField] private Vector3           m_Min;
    [SerializeField] private Vector3           m_MaxPosition;
    [SerializeField] private Vector3           m_MinPosition;
    [SerializeField] private DivisionTreeType  m_DivisionTreeType;
    [SerializeField] private List<SceneObject> m_SceneObjectBounds;
    [SerializeField] private Transform         m_SceneTreeTrans;
    [OnValueChanged("SetNewDepth")]
    [SerializeField]private int               m_MaxDepth = 5;
    //保存之前与摄像机的平截头体碰撞情况
    [SerializeField] private bool              m_PreState;
    public                   Bounds            SceneTreeNodeBounds => this.m_SceneTreeNodeBounds;
    public                   TreeNode          SceneTreeMainNode   => this.m_Octree.MainNode;
    public                   DivisionTreeType  DivTreeType         => this.m_DivisionTreeType;
    public                   List<SceneObject> SceneObjects        => this.m_SceneObjectBounds;
    private void SetNewDepth()
    {
        this.m_Octree.MainNode.m_MaxDepth = this.m_MaxDepth;
    }
    public bool PreState
    {
        get=> this.m_PreState;
        set
        {
            if(value != m_PreState)
            {
                m_PreState = value;
            }
        }
    }
    /// <summary>
    /// 动态添加物体到八叉树
    /// </summary>
    /// <param name="obj">构造一个SceneObj传递</param>
    public void AddSceneObject(SceneObject obj)
    {
        this.m_Octree.MainNode?.Append(obj);
    }
    public List<GameObject> Sphere(Vector3 position,float radius) => SphereIntersect(m_Octree.MainNode, position, radius);
    //圆形AOE
    public List<GameObject> SphereIntersect(TreeNode node,Vector3 position, float radius)
    {
        List<GameObject> intersectObjects = new List<GameObject>();
        //本节点碰撞检测
        if(node.Count>0)
        {
            if (node.SObjects != null && node.SObjects.Count > 0)
            {
                foreach (var so in node.SObjects)
                {
                    if (Vector3.Distance(so.Self.transform.position, position) <= radius)
                        intersectObjects.Add(so.Self);
                }
            }
        }
        //子节点碰撞检测
        node.Iterator((item) =>
        {
            if (item.SObjects != null && item.SObjects.Count > 0)
            {
                foreach (var so in item.SObjects)
                {
                    if(Vector3.Distance(so.Self.transform.position,position)<=radius)
                        intersectObjects.Add(so.Self);
                }
            }
            
            //遍历子节点
            if (item.Count > 0)
            {
                var closetPoint = item.Bounds.ClosestPoint(position);
                //相交
                if (item.Bounds.Contains(closetPoint) || Vector3.Distance(closetPoint, position) <= radius)
                {
                    intersectObjects.AddRange(SphereIntersect(item, position, radius));
                }
            }
        });
        return intersectObjects;
    }
    public void Update()
    {
        if(m_Octree.MainNode!=null)
        {
            UpdateSceneObjects(m_Octree.MainNode);
        }
    }
    private void UpdateSceneObjects(TreeNode node)
    {
        if (node != null)
        {
            node.Update();
            node.Iterator((item) =>
            {
                if (item.Count > 0)
                    UpdateSceneObjects(item);
                item.Update();
            }
            );
        }
    }
    private Octree m_Octree;
    private bool   m_Init;
    public  Color  m_DebugOctreeColor;

    private void Awake()
    {
#if UNITY_EDITOR
        this.m_TreeName = $"{this.transform.name}_{this.GetInstanceID()}";
#endif
    }

    public void Clear()
    {
        if (!this.m_Init) return;
        this.m_Init = false;
        this.ClearNodes(this.m_Octree.MainNode);
    }

    private void ClearNodes(TreeNode clear)
    {
        if (clear != null)
        {
            clear.Iterator((item) =>
            {
                if (item.Count > 0)
                    ClearNodes(item);
                item.Recovery();
            });
            clear.Recovery();
        }
    }

    //进行初始化操作
    public void Initlize(DivisionTreeType type)
    {
        if (this.m_Init) return;

        if (this.m_SceneObjectBounds == null)
            this.m_SceneObjectBounds = new List<SceneObject>();
        this.m_DivisionTreeType = type;
        this.m_PreState = false;
#if UNITY_EDITOR
        this.InitlizeTreeBounds();
#endif
        this.CreateOctree();
        this.m_Init = true;
    }

    public void CreateOctree()
    {
        if (this.m_Init) return;
        if (this.m_Octree.MainNode == null)
        {
            this.m_Octree = new Octree();
        }
        //逻辑SceneTree没有初始化就要创建树
        this.m_Octree.InitOctree(this,
            new Bounds(this.m_SceneTreeTrans.position,
                new Vector3(this.m_SceneTreeSize.x, this.m_SceneTreeSize.y, this.m_SceneTreeSize.z)),
            this.m_SceneObjectBounds,this.m_MaxDepth);
    }
    
    private void InitlizeTreeBounds()
    {
        Renderer[] renderers = this.transform.GetComponentsInChildren<Renderer>();
        if (renderers.Length <= 0) return;

        this.m_SceneObjectBounds.Clear();
        this.m_MinPosition = renderers[0].transform.position;
        this.m_MaxPosition = renderers[0].transform.position;
        for (int i = 0; i < renderers.Length; i++)
        {
            this.m_MinPosition = Vector3.Min(renderers[i].transform.position, this.m_MinPosition);
            this.m_MaxPosition = Vector3.Max(renderers[i].transform.position, this.m_MaxPosition);
        }

        Vector3 center = this.m_MinPosition + (this.m_MaxPosition - this.m_MinPosition) / 2;
        this.m_SceneTreeNodeBounds = new Bounds(center, Vector3.zero);
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer    renderer = renderers[i];
            SceneObject sobject  = new SceneObject();
            sobject.InstanceID = i;
            sobject.Position = renderer.transform.position;
            sobject.Bounds = renderer.bounds;
            sobject.Renderer = renderer;
            sobject.Self = renderer.gameObject;

            this.m_MinPosition = Vector3.Min(renderer.transform.position, this.m_MinPosition);
            this.m_SceneObjectBounds.Add(sobject);
            this.m_SceneTreeNodeBounds.Encapsulate(renderer.bounds);
        }

        this.m_Max = this.m_SceneTreeNodeBounds.max;
        this.m_Min = this.m_SceneTreeNodeBounds.min;
    }

    private void OnDrawGizmos()
    {
        if (this.m_Octree.MainNode != null)
        {
            Bounds bounds = new Bounds(this.m_SceneTreeTrans.position, this.m_Octree.MainNode.Bounds.size);
            this.m_Octree.MainNode.SetBounds(bounds);
        }

        Vector3 frontLeftBottom  = new Vector3(this.m_Min.x, this.m_Min.y, this.m_Max.z);
        Vector3 frontRightBottom = new Vector3(this.m_Max.x, this.m_Min.y, this.m_Max.z);
        Vector3 frontLeftTop     = new Vector3(this.m_Min.x, this.m_Max.y, this.m_Max.z);
        Vector3 frontRightTop    = new Vector3(this.m_Max.x, this.m_Max.y, this.m_Max.z);

        Vector3 bkLeftBottom  = new Vector3(this.m_Min.x, this.m_Min.y, this.m_Min.z);
        Vector3 bkRightBottom = new Vector3(this.m_Max.x, this.m_Min.y, this.m_Min.z);
        Vector3 bkLeftTop     = new Vector3(this.m_Min.x, this.m_Max.y, this.m_Min.z);
        Vector3 bkRightTop    = new Vector3(this.m_Max.x, this.m_Max.y, this.m_Min.z);

        Gizmos.DrawLine(frontLeftBottom, frontRightBottom);
        Gizmos.DrawLine(frontLeftBottom, frontLeftTop);
        Gizmos.DrawLine(frontRightTop,   frontLeftTop);
        Gizmos.DrawLine(frontRightTop,   frontRightBottom);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(bkLeftBottom, bkRightBottom);
        Gizmos.DrawLine(bkLeftBottom, bkLeftTop);
        Gizmos.DrawLine(bkRightTop,   bkLeftTop);
        Gizmos.DrawLine(bkRightTop,   bkRightBottom);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(bkLeftBottom,  frontLeftBottom);
        Gizmos.DrawLine(bkLeftTop,     frontLeftTop);
        Gizmos.DrawLine(bkRightTop,    frontRightTop);
        Gizmos.DrawLine(bkRightBottom, frontRightBottom);

        this.DebugNode(this.m_Octree.MainNode);
    }

    private void DebugNode(TreeNode node)
    {
        if (node != null)
        {
            Bounds  bounds   = node.Bounds;
            Vector3 lbottomf = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
            Vector3 ltopf    = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            Vector3 rbottomf = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            Vector3 rtopf    = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);

            Vector3 lbottomb = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            Vector3 ltopb    = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            Vector3 rbottomb = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            Vector3 rtopb    = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);

            Gizmos.color = this.m_DebugOctreeColor;
            Gizmos.DrawLine(lbottomf, rbottomf);
            Gizmos.DrawLine(lbottomf, ltopf);
            Gizmos.DrawLine(rtopf,    ltopf);
            Gizmos.DrawLine(rtopf,    rbottomf);

            Gizmos.DrawLine(lbottomb, rbottomb);
            Gizmos.DrawLine(lbottomb, ltopb);
            Gizmos.DrawLine(rtopb,    ltopb);
            Gizmos.DrawLine(rtopb,    rbottomb);

            Gizmos.DrawLine(lbottomf, lbottomb);
            Gizmos.DrawLine(ltopf,    ltopb);
            Gizmos.DrawLine(rtopf,    rtopb);
            Gizmos.DrawLine(rbottomf, rbottomb);

            if (node.Count > 0)
            {
                node.Iterator((item) => { this.DebugNode(item); });
            }
        }
    }
}