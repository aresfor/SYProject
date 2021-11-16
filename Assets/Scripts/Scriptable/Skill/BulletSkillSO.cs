using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
[CreateAssetMenu(fileName = "BulletSkill", menuName = "ScriptableObjects/BulletSkill",order = 4)]
public class BulletSkillSO : SkillSO
{
    [AssetsOnly]
    [VerticalGroup(GENERAL_SETTINGS_VERTICAL_GROUP)]
    [Title("子弹技能效果")]
    public GameObject BulletEffect;

    [BoxGroup(STATS_BOX_GROUP)]
    [Title("额外伤害")]
    public int ExtraDamage;

    [BoxGroup(STATS_BOX_GROUP)]
    [Title("子弹轨迹")]
    public TrailRenderer Trail;

   

}
#endif
