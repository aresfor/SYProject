using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class SDivisionManager
{
    private ConeManager      mRenderConeManager;
    private float            m_Aspect;
    private DivisionTreeType m_DivisionTreeType;
    private List<SceneTree>  m_SceneTrees;
    private List<SceneTree>  m_SceneTreesWhiteList;
    private List<SceneTree>  m_SceneTreesGreyList;
    
    public  List<SceneTree>  SceneTrees => this.m_SceneTrees;
    public List<SceneTree> SceneTreesWhiteList => this.m_SceneTreesWhiteList;
    public List<SceneTree> SceneTreesGreyList  => this.m_SceneTreesGreyList;

    public static readonly SDivisionManager Instance = new SDivisionManager();

    public void SetDivTreeType(DivisionTreeType type)
    {
        this.m_DivisionTreeType = type;
    }
    //添加物体到树
    public void AppendSceneObject(SceneObject obj)
    {
        foreach (var tree in m_SceneTreesWhiteList)
        {
            if(tree.SceneTreeMainNode!=null)
            {
                if(tree.SceneTreeMainNode.Bounds.Contains(obj.Bounds.min) && tree.SceneTreeMainNode.Bounds.Contains(obj.Bounds.max))
                {
                    tree.SceneTreeMainNode.Append(obj);
                }
            }
        }
    }
    SDivisionManager()
    {
        //默认八叉树，通过SetDivTreeType改变，目前只实现八叉树
        this.m_DivisionTreeType = DivisionTreeType.Octree;
        this.m_SceneTrees = new List<SceneTree>();
        this.m_SceneTreesWhiteList = new List<SceneTree>();
        this.m_SceneTreesGreyList = new List<SceneTree>();
    }

    public void AddSceneTreesWhiteList(SceneTree stree)
    {
        if (!this.m_SceneTreesWhiteList.Contains(stree))
            this.m_SceneTreesWhiteList.Add(stree);
    }
    //GreyList中的树并不进行空间划分
    public void AddSceneTreesGreyList(SceneTree stree)
    {
        if (!this.m_SceneTreesGreyList.Contains(stree))
        {
            this.m_SceneTreesGreyList.Add(stree);
            for (int i = 0; i < stree.SceneObjects.Count; i++)
            {
                SceneObject sobject = stree.SceneObjects[i];
                if (sobject.Renderer)
                    sobject.Renderer.enabled = false;
            }
        }
    }

    public void RemoveSceneTreesWhiteList(SceneTree stree)
    {
        if (this.m_SceneTreesWhiteList.Contains(stree))
            this.m_SceneTreesWhiteList.Remove(stree);
    }

    public void RemoveSceneTreesGreyList(SceneTree stree)
    {
        if (this.m_SceneTreesGreyList.Contains(stree))
            this.m_SceneTreesGreyList.Remove(stree);
    }

    //TODO 考虑删除
    public void Refresh()
    {
        if (this.m_SceneTrees != null && this.m_SceneTrees.Count > 0)
        {
            for (int i = 0; i < this.m_SceneTrees.Count; i++)
            {
                SceneTree st = this.m_SceneTrees[i];
                st.Initlize(this.m_DivisionTreeType);
            }
        }
    }

    
    public void CreateTree()
    {
        if (this.m_SceneTrees != null && this.m_SceneTrees.Count > 0)
        {
            for (int i = 0; i < this.m_SceneTrees.Count; i++)
            {
                SceneTree st = this.m_SceneTrees[i];
                st.CreateOctree();
                st.Clear();
            }
        }
    }

    public void Clear()
    {
        if (this.m_SceneTrees != null && this.m_SceneTrees.Count > 0)
        {
            for (int i = 0; i < this.m_SceneTrees.Count; i++)
            {
                SceneTree st = this.m_SceneTrees[i];
                st.Clear();
            }
        }
    }
}