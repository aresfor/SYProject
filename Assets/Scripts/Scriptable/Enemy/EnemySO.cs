using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/CreateEnemySO", order = 1)]
public class EnemySO : SerializedScriptableObject
{
    public int CurrentHP = 0;
    public int MaxHP = 0;
    public float MoveSpeed = 1.0f;
    public string AttrName = "";
    
    //Enemy的特有属性
}
