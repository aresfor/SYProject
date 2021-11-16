using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System;
using UnityEditor;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
//[NodeTint(1.0f, 0.8f, 0.8f)]
public class SkillNode : Node
{
    public SkillSO skill;
    [ReadOnly]
    [Input(ShowBackingValue.Unconnected)]
    public string preSkillName;
    [ReadOnly]
    public string skillName;
    [ReadOnly]
    public int skillLevel;
    [ReadOnly]
    public SkillEvolve skillEvolve;
    [Output]
    public string Out;
    //Use this for initialization
    protected override void Init()
    {
        base.Init();
        skillName = skill.Name;
        skillLevel = skill.Level;
        skillEvolve = skill.Evolve;
    }

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
        if(port.fieldName == "Out")
            return GetInputValue<string>("skillName", skillName);
        return null;
	}
    [ContextMenu("Save Dependencies")]
    public void SaveDependencies()
    {
        skill.preSkillName.Clear();
        foreach(var i in GetInputPort("preSkillName").GetInputValues<string>())
        {
            if(skill.preSkillName.Contains(i))
            {
                Debug.LogError("有技能的ID相同，请检查");
            }
            skill.preSkillName.Add(i);
        }
        EditorUtility.SetDirty(skill);
        AssetDatabase.SaveAssets();
    }
}
#endif