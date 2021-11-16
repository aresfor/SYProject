#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 
// This is the base-class for all items. It contains a lot of layout using various layout group attributes. 
// We've also defines a few relevant groups in constant variables, which derived classes can utilize.
// 
// Also note that each item deriving from this class, needs to specify which Item types are
// supported via the SupporteItemTypes property. This is then referenced in ValueDropdown attribute  
// on the Type field, so that when users only can specify supported item-types.  
// 

public abstract class SkillSO : ScriptableObject
{
    protected const string LEFT_VERTICAL_GROUP = "Split/Left";
    protected const string STATS_BOX_GROUP = "Split/Left/Stats";
    protected const string GENERAL_SETTINGS_VERTICAL_GROUP = "Split/Left/General Settings/Split/Right";

    [HideLabel, PreviewField(55)]
    [VerticalGroup(LEFT_VERTICAL_GROUP)]
    [HorizontalGroup(LEFT_VERTICAL_GROUP + "/General Settings/Split", 55, LabelWidth = 67)]
    public Texture Icon;

    [BoxGroup(LEFT_VERTICAL_GROUP + "/General Settings")]
    [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
    [Title("技能名称")]
    public string Name;

    [BoxGroup(LEFT_VERTICAL_GROUP + "/General Settings")]
    [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
    [Title("技能ID")]
    public int ID;

    [VerticalGroup("Split/Right")]
    [Title("开放编辑")]
    public bool OpenEdit = false;

    [BoxGroup("Split/Right/Description")]
    [HideLabel, TextArea(4, 14)]
    [EnableIf("OpenEdit")]
    [Title("技能描述")]
    public string Description;

    [HorizontalGroup("Split", 0.5f, MarginLeft = 5, LabelWidth = 130)]
    [BoxGroup("Split/Right/Notes")]
    [HideLabel, TextArea(4, 9)]
    [EnableIf("OpenEdit")]
    public string Notes;

    [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
    //[ValueDropdown("SupportedItemTypes")]
    //[ValidateInput("IsSupportedType")]
    [Title("技能类型")]
    public SkillType Type;

    [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
    [Title("技能类型")]
    public SkillFireType FireType;

    [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
    //[ValueDropdown("SupportedItemTypes")]
    //[ValidateInput("IsSupportedType")]
    [EnumToggleButtons]
    [Title("技能属性")]
    public SkillAttribute Attrubute;

    [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
    [Title("冷却时间")]
    public float CoolDown;

    [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
    [Title("持续时间")]
    [MaxValue("CoolDown")]
    public float Duration;

    [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
    [Title("前置技能")]
    [InfoBox("需要先学习这些技能才能学习这个技能")]
    [DictionaryDrawerSettings(KeyLabel = "技能ID", ValueLabel = "技能名字", IsReadOnly = true)]
    [ReadOnly]
    public List<string> preSkillName;
    //[VerticalGroup("Split/Right")]
    //public StatList Requirements;

    //一般特定技能没有，所有这里转移
    //[AssetsOnly]
    //[VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
    //[Title("实际技能对象")]
    //public GameObject Prefab;

    [BoxGroup(STATS_BOX_GROUP)]
    [Title("技能等级")]
    public int Level = 1;

    [BoxGroup(STATS_BOX_GROUP)]
    [Title("技能阶级")]
    [InfoBox("技能等级达到一定等级就会让技能产生质变")]
    //[ValueDropdown("SupportedItemTypes")]
    //[ValidateInput("IsSupportedType")]
    public SkillEvolve Evolve;

    //public abstract skill[] SupportedItemTypes { get; }

    //private bool IsSupportedType(ItemType type)
    //{
    //    return this.SupportedItemTypes.Contains(type);
    //}
}

#endif
