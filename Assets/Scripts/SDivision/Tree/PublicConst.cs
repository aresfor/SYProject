using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PublicConst
{
}

public enum DivisionTreeType
{
    Octree,
    Quadtree,
}

[Flags]
public enum TreeType : Int32
{
    None = 0,
    Left_Bottom_Front  = 1 << 0,
    Left_Top_Front     = 1 << 1,
    Right_Top_Front    = 1 << 2,
    Right_Bottom_Front = 1 << 3,

    Left_Bottom_Background  = 1 << 4,
    Left_Top_Background     = 1 << 5,
    Right_Top_Background    = 1 << 6,
    Right_Bottom_Background = 1 << 7,

    Left_Bottom  = 1 << 8,
    Left_Top     = 1 << 9,
    Right_Top    = 1 << 10,
    Right_Bottom = 1 << 11,
}