#if UNITY_EDITOR
namespace Assets.Scripts.Windows
{
    using Sirenix.OdinInspector;
    using Sirenix.Utilities;
    using System.Linq;
    using UnityEditor;
    [GlobalConfig("ScriptableObjects/EnemyOverView")]
    public class EnemyOverView:GlobalConfig<EnemyOverView>
    {
        [ReadOnly]
        [ListDrawerSettings(Expanded = true)]
        public EnemySO[] AllEnemys;

        //按钮在最底层
        [Button(ButtonSizes.Medium),PropertyOrder(-1)]
        public void UpdateEnemyOverview()
        {
            // Finds and assigns all scriptable objects of type Character
            this.AllEnemys = AssetDatabase.FindAssets("t:EnemySO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<EnemySO>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
    }
}
#endif
