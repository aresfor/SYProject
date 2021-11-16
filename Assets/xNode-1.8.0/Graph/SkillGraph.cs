using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class SkillGraph : NodeGraph
{
    [ContextMenu("保存前置技能关系")]
    public void SaveDependencies()
    {
        foreach (var node in nodes)
        {
            if(node is SkillNode)
            {
                SkillNode n = node as SkillNode;
                n.SaveDependencies();
            }
        }
    }
}