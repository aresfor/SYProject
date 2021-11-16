using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    private Bounds                         m_Bounds;
    private List<SceneObject>              m_SObjects;
    private Dictionary<TreeType, TreeNode> m_TreeMap;
    private DivisionTreeType               m_DivisionTreeType;
    private int                            m_Depth;
    public  int                            m_MaxDepth;

    private TreeNode                       m_ParentNode;
    public  Bounds                         Bounds   => this.m_Bounds;
    public  List<SceneObject>              SObjects => this.m_SObjects;
    public int                             Depth    => this.m_Depth;


    private int m_TreeOffset      = 0;
    private int m_TreeTargetIndex = 8;

    public int Count => this.m_TreeMap.Count;

    public TreeNode(DivisionTreeType type,TreeNode parent,int MaxDepth,int Depth)
    {
        this.m_DivisionTreeType = type;
        this.m_TreeMap = new Dictionary<TreeType, TreeNode>();
        this.m_SObjects = new List<SceneObject>();
        this.m_ParentNode = parent;
        this.m_Depth = Depth;
        this.m_MaxDepth = MaxDepth;
        
        //默认八叉树
        this.m_TreeTargetIndex = 8;
        this.m_TreeOffset = 0;

        if (this.m_DivisionTreeType != DivisionTreeType.Octree)
        {
            this.m_TreeTargetIndex = 12;
            this.m_TreeOffset = 8;
        }
    }

    public void SetBounds(Bounds bounds)
    {
        this.m_Bounds = bounds;
    }

    //TODO 可以让某一分区对象超过一定数目才进行Div，目前是每个对象尽量放一个区
    public void DivTree()
    {
        //深度到了就退出
        if(this.m_Depth >= this.m_MaxDepth) { return; }
        if (this.GetNullInitlize())
        {
            //分割
            if (this.m_DivisionTreeType == DivisionTreeType.Octree)
            {
                //分割八叉树
                this.DivOcTreeRectangle();
            }
        }
    }

    public bool Append(SceneObject sobject)
    {
        this.DivTree();
        Renderer renderer = sobject.Self.GetComponent<Renderer>();
        if (this.m_Bounds.Contains(renderer.bounds.max) && this.m_Bounds.Contains(renderer.bounds.min))
        {
            TreeNode node = this.CheckIntersects(renderer.bounds); //如果可放下，继续向下查找
            if (node != null)
            {
                //this.DivTree();
                node.Append(sobject);
            }
            else
            {
                this.m_SObjects.Add(sobject);
            }
        }//当子节点物体动态变动时候就要向上回溯看节点是否能容纳这个节点
        else
        {
            m_ParentNode?.Append(sobject);
            //Debug.Log(sobject.InstanceID + "改变节点了");
        }
        return true;
    }
    //动态更新物体所在的节点
    public void Update()
    {
        if(m_SObjects == null) { return; }
        List<SceneObject> clearList = new List<SceneObject>();
        foreach (var go in m_SObjects)
        {
            if(!go.Self.GetComponent<Renderer>().bounds.Intersects(this.m_Bounds))
            {
                //树的根节点，MainNode的parent是空
                m_ParentNode?.Append(go);
                clearList.Add(go);
            }
        }
        //为了防止迭代器失效，遍历完变更的物体后再删除他们
        foreach (var go in clearList)
        {
            m_SObjects.Remove(go);
        }
        
        //使用外面的迭代器让这里代码更干净
        //foreach (var treeNode in m_TreeMap.Values)
        //{
        //    treeNode.Update();
        //}
    }

    //供SceneTree来进行一些遍历节点的操作
    public void Iterator(Action<TreeNode> iteratorcallback)
    {
        if (iteratorcallback == null || this.m_TreeMap == null || this.m_TreeMap.Count <= 0) return;
        if (this.m_DivisionTreeType == DivisionTreeType.Octree)
        {
            for (int i = this.m_TreeOffset; i < this.m_TreeTargetIndex; i++)
            {
                TreeType treeType = (TreeType) (1 << i);
                if (this.m_TreeMap.ContainsKey(treeType))
                    iteratorcallback?.Invoke(this.m_TreeMap[treeType]);
            }
        }
    }

    //判断当前节点的子层级节点是否包括
    private TreeNode CheckIntersects(Bounds bounds)
    {
        //遍历所有节点,必须包含 最大和最小，否则用父节点包含它
        for (int i = this.m_TreeOffset; i < this.m_TreeTargetIndex; i++)
        {
            TreeType treeType = (TreeType) (1 << i);
            if(!this.m_TreeMap.ContainsKey(treeType)) continue;
            TreeNode node     = this.m_TreeMap[treeType];
            if (node.Bounds.Contains(bounds.max) && node.Bounds.Contains(bounds.min))
            {
                return node;
            }
        }

        return null;
    }
    //层级增加
    private void DivOcTreeRectangle()
    {
        this.m_TreeMap.Clear();

        Vector3 BoundsSize = this.m_Bounds.size * 0.5f;
        
        for (int i = this.m_TreeOffset; i < this.m_TreeTargetIndex; i++)
        {
            TreeType treeType = (TreeType) (1 << i);
            TreeNode treeNode = new TreeNode(this.m_DivisionTreeType,this,this.m_MaxDepth,this.m_Depth+1);
            Vector3  center   = this.m_Bounds.center * 0.5f + this.GetCenter(treeType) * 0.5f;

            Bounds bounds = new Bounds(center, BoundsSize);
            treeNode.SetBounds(bounds);
            this.m_TreeMap[treeType] = treeNode;
        }
    }

    private bool GetNullInitlize()
    {
        return this.m_TreeMap.Count <= 0;
    }
    public void Clear() => this.Recovery();
    public void Recovery()
    {
        //目前先直接清理，后面可以考虑内存池，因为涉及引用
        this.m_TreeMap.Clear();
    }
    
    private Vector3 GetCenter(TreeType treeType)
    {
        switch (treeType)
        {
            case TreeType.None:
                return Vector3.zero;
            case TreeType.Left_Bottom_Front:
                return this.m_Bounds.min;
            case TreeType.Left_Top_Front:
                return new Vector3(this.m_Bounds.min.x, this.m_Bounds.max.y, this.m_Bounds.min.z);
            case TreeType.Right_Top_Front:
                return new Vector3(this.m_Bounds.max.x, this.m_Bounds.max.y, this.m_Bounds.min.z);
            case TreeType.Right_Bottom_Front:
                return new Vector3(this.m_Bounds.max.x, this.m_Bounds.min.y, this.m_Bounds.min.z);
            case TreeType.Left_Bottom_Background:
                return new Vector3(this.m_Bounds.min.x, this.m_Bounds.min.y, this.m_Bounds.max.z);
            case TreeType.Left_Top_Background:
                return new Vector3(this.m_Bounds.min.x, this.m_Bounds.max.y, this.m_Bounds.max.z);
            case TreeType.Right_Top_Background:
                return this.m_Bounds.max;
            case TreeType.Right_Bottom_Background:
                return new Vector3(this.m_Bounds.max.x, this.m_Bounds.min.y, this.m_Bounds.max.z);
            case TreeType.Left_Bottom:
                break;
            case TreeType.Left_Top:
                break;
            case TreeType.Right_Top:
                break;
            case TreeType.Right_Bottom:
                break;
        }
        return Vector3.zero;
    }

}