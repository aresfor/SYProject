using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class ExtentionMethods
{
    public static string GetMappedString(this HeroFsmStateTypes type)
    {
        return Enum.GetName(typeof(HeroFsmStateTypes), type);
    }
}
public class HeroFsmStateTypesEnumComparer : IEqualityComparer<HeroFsmStateTypes>
{
    public bool Equals(HeroFsmStateTypes type1,HeroFsmStateTypes type2)
    {
        return (int)type1 == (int)type2;
    }
    public int GetHashCode(HeroFsmStateTypes type)
    {
        return (int)type;
    }
}
//这个类好像没什么用？
public class TimerHandleComparer:Comparer<TimerExMsg>
{
    public override int Compare(TimerExMsg handle1, TimerExMsg handle2)
    {
        Debug.LogWarning("TimerHandleComparer call!");
        return handle1.ExpiredTime.CompareTo(handle2.ExpiredTime);
    }

    //public int CompareTo(TimerExMsg other)
    //{
    //    this.
    //    return 
    //}

    public bool Equals(TimerExMsg handle1, TimerExMsg handle2)
    {
        Debug.LogWarning("TimerHandleComparer.Equals call!");
        return handle1.Handle == handle2.Handle;
    }
}
