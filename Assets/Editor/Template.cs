using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Template
{
    [MenuItem("Assets/Create/cs/temp.cs")]
    static void CreateCS()
    {
        ProjectWindowUtilEx.CreateScriptUtil(@"81-C# Script-NewBehaviourScript.cs.txt", "temp.cs");
    }
}
