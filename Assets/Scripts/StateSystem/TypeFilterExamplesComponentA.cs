// TypeFilterExamplesComponent.cs
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class TypeFilterExamplesComponentA : OdinEditorWindow
{
    [MenuItem("Window/TypeFilter")]
    private static void openWindow()
    {
        GetWindow<TypeFilterExamplesComponentA>()
            .position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600); 
    }
    [TypeFilter("GetFilteredTypeListA")]
    public BaseClassA A, B;

    [TypeFilter("GetFilteredTypeListA")]
    public BaseClassA[] Array = new BaseClassA[3];

    public IEnumerable<Type> GetFilteredTypeListA()
    {
        var q = typeof(BaseClassA).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition)                             // Excludes C1<>
            .Where(x => typeof(BaseClassA).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass

        // Adds various C1<T> type variants.
        q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(GameObject)));
        q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(AnimationCurve)));
        q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(List<float>)));

        return q;
    }

    public abstract class BaseClassA
    {
        public int BaseField;
    }

    public class A1A : BaseClassA { public int _A1; }
    public class A2A : A1A { public int _A2; }
    public class A3A : A2A { public int _A3; }
    public class B1A : BaseClassA { public int _B1; }
    public class B2A : B1A { public int _B2; }
    public class B3A : B2A { public int _B3; }
    public class C1<T> : BaseClassA { public T C; }
}
#endif