using UnityEditor;
using System.Reflection;
using UnityEngine;
public class ProjectWindowUtilEx
{
    public static void CreateScriptUtil(string path, string templete)
    {
        MethodInfo method = typeof(ProjectWindowUtil).GetMethod("CreateScriptAsset",
            BindingFlags.Static | BindingFlags.NonPublic);
        if (method != null)
            method.Invoke(null, new object[] { templete, path });
        else
            Debug.Log("method none");
    }
}