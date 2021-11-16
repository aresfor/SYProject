using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
[CreateAssetMenu(fileName = "LongGun", menuName = "ScriptableObjects/CreateLongGun", order = 2)]
public class LongGun : EquipableItem
{
    [BoxGroup(STATS_BOX_GROUP)]
    public int ATK;

    

}
#endif
