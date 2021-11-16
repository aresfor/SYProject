using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
[CreateAssetMenu(fileName = "HealBottle",menuName = "ScriptableObjects/HealBottle",order = 3)]
public class HealBottle : ConsumableItem
{
    [BoxGroup(STATS_BOX_GROUP)]
    public int HealNum;
}
#endif
