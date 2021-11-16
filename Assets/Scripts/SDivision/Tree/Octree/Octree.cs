using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SceneTree只是逻辑分区对象，Octree才是实际的树
public struct Octree
{
    private SceneTree        m_SceneTree;
    private TreeNode         m_MainNode;
    private DivisionTreeType m_DivisionTreeType;
    //private int              m_MaxDepth;
    
    public TreeNode MainNode => this.m_MainNode;
    
    public void InitOctree(SceneTree tree, Bounds treebounds, IEnumerable<SceneObject> boundses,int maxDepth)
    {
        this.m_SceneTree = tree;
        if (this.m_MainNode == null)
        {
            this.m_MainNode = new TreeNode(tree.DivTreeType,null,maxDepth,1);
            //this.m_MaxDepth = maxDepth;
            this.m_MainNode.SetBounds(treebounds);
            this.m_MainNode.DivTree();
        }

        if (boundses != null)
        {
            IEnumerator<SceneObject> iterator = boundses.GetEnumerator();
            while (iterator.MoveNext())
            {
                SceneObject sobject = iterator.Current;
                if (this.m_MainNode.Append(sobject))
                {
                
                }
            }
            iterator.Dispose();
        }
    }
    //public void Update()
    //{
    //    if(m_MainNode != null)
    //    {
    //        UpdateSceneObjects(m_MainNode);
    //    }
    //    //m_MainNode?.Update();
    //}
    
}