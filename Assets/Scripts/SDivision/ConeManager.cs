using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeManager : MonoBehaviour
{
    [SerializeField]               private Camera           m_Camera;
    [SerializeField]               private List<Cone>       m_Cones;
    //相机视锥体分级
    [SerializeField, Range(1, 10)] private int              m_ConeLevel = 1;

    [SerializeField] private MonoSDivisionManager m_MonoSDivisionManager;
    [SerializeField] private float m_Aspect;
    [SerializeField] private bool  m_IsOrthographic;
    [SerializeField] private Transform m_TestTrans;

    //lazyTime防止重复的去分割八叉树
    public float LazyTime = 2f;

    //保存正在处于LazyTime的树，下次离开的时候重置Timer的时间
    private Dictionary<SceneTree, TimerExMsg> LazyRemovalTree;

    //组成相机视锥体的组件
    private Plane[] m_Plane;

    private void Awake()
    {
        this.m_Aspect = (float) Screen.width / (float) Screen.height;
        this.m_Camera = this.GetComponent<Camera>();
        this.ConeGrading();
        SDivisionManager.Instance.SceneTrees.AddRange(this.m_MonoSDivisionManager.SceneTrees);
        //SDivisionManager.Instance.SetDivTreeType(m_MonoSDivisionManager.DivTreeType);
        //TODO 为每个sceneTree创建根节点，这点可以放到Update中
        SDivisionManager.Instance.Refresh();
        
    }
    //测试用，只显示当前的树
    private void Init()
    {
        foreach (var stree in SDivisionManager.Instance.SceneTrees)
        {
            this.ClearTree(stree);
        }
    }
    private void Start()
    {
        LazyRemovalTree = new Dictionary<SceneTree, TimerExMsg>();
        this.Init();
    }
    private void Update()
    {
        this.InitlizeCone();
    }

    //视锥体分级。
    private void ConeGrading()
    {
        float avgFar = (this.m_Camera.farClipPlane) / this.m_ConeLevel;
        Cone  cone   = new Cone(this.m_Camera.nearClipPlane, avgFar, this.m_Camera.fieldOfView);
        cone.RenderCamera = this.m_Camera;
        cone.InityCone(this.m_Camera.aspect);
        this.m_Cones.Add(cone);
        for (int i = 1; i < this.m_ConeLevel; i++)
        {
            Cone newCone = new Cone(cone.Far, cone.Far + avgFar, this.m_Camera.fieldOfView);
            newCone.RenderCamera = this.m_Camera;
            newCone.InityCone(this.m_Camera.aspect);
            this.m_Cones.Add(newCone);
            cone = newCone;
        }
    }

    //初始化视锥体
    private void InitlizeCone()
    {
        if (this.m_Camera == null) 
        {
            Debug.LogWarning("Render Camera Is null");
            return;
        }

        for (int i = 0; i < this.m_Cones.Count; i++)
        {
            this.m_Cones[i].InityCone(this.m_Camera.aspect);
        }

        this.m_Plane = GeometryUtility.CalculateFrustumPlanes(this.m_Camera);
        
        if (SDivisionManager.Instance.SceneTrees != null &&
            SDivisionManager.Instance.SceneTrees.Count > 0)
        {
            for (int i = 0; i < SDivisionManager.Instance.SceneTrees.Count; i++)
            {
                SceneTree stree = SDivisionManager.Instance.SceneTrees[i];
                this.CheckTreeBounds(stree);
            }
        }

        if (SDivisionManager.Instance.SceneTreesWhiteList != null &&
            SDivisionManager.Instance.SceneTreesWhiteList.Count > 0)
        {
            for (int i = 0; i < SDivisionManager.Instance.SceneTreesWhiteList.Count; i++)
            {
                SceneTree stree = SDivisionManager.Instance.SceneTreesWhiteList[i];
                for (int j = 0; j < stree.SceneObjects.Count; j++)
                {
                    SceneObject sobject = stree.SceneObjects[j];
                    bool state = GeometryUtility.TestPlanesAABB(this.m_Plane, sobject.Bounds);
                    //bool hasObstacleBetween = false;
                    
                    if (sobject.Renderer)
                        sobject.Renderer.enabled = state;
                }
            }
        }
    }
    //与相机视锥体相交的树加入WhiteList，也就是Update要显示分级的树
    private void CheckTreeBounds(SceneTree stree)
    {
        //TODO 这里需要改，改完可以不用初始化代码
        if (stree.SceneTreeMainNode != null)
        {
            bool state = GeometryUtility.TestPlanesAABB(this.m_Plane, stree.SceneTreeMainNode.Bounds);
            //视锥体从离开到与树边界碰撞
            if (state != stree.PreState && state == true)
            {
                if(LazyRemovalTree.ContainsKey(stree))
                {
                    TimerManager.Instance.Reset(LazyRemovalTree[stree]);
                    TimerManager.Instance.Pause(LazyRemovalTree[stree]);
                }
                else
                {
                    SDivisionManager.Instance.AddSceneTreesWhiteList(stree);
                    SDivisionManager.Instance.RemoveSceneTreesGreyList(stree);
                    stree.Initlize(stree.DivTreeType);
                }
                
            }
            //视锥体从与树碰撞到离开树
            else if(state != stree.PreState && state == false)
            {
                if(!LazyRemovalTree.ContainsKey(stree))
                {
                    TimerExMsg timerExMsg = TimerManager.Instance.SetTimer(new Action<SceneTree>(ClearTree), LazyTime, true, false,1,TimerState.Pending,stree);
                    LazyRemovalTree.Add(stree, timerExMsg);
                }
                else
                {
                    TimerManager.Instance.Resume(LazyRemovalTree[stree]);
                }
                
            }
            stree.PreState = state;
        }
    }
    //清理树节点
    private void ClearTree(SceneTree stree)
    {
        SDivisionManager.Instance.AddSceneTreesGreyList(stree);
        SDivisionManager.Instance.RemoveSceneTreesWhiteList(stree);
        LazyRemovalTree.Remove(stree);
        Debug.Log("清理" + stree.name);
        stree.Clear();
    }


    private void OnDrawGizmos()
    {
        if (this.m_Camera == null) return;
        this.m_Cones.ForEach((item) => { item.DebugDrawCone(this.transform.position, this.transform.rotation); });
    }
}