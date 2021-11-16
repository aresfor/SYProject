#if UNITY_EDITOR
namespace Assets.Scripts.Windows
{
    using Sirenix.OdinInspector.Editor;
    using System.Linq;
    using UnityEngine;
    using Sirenix.Utilities.Editor;
    using Sirenix.Serialization;
    using UnityEditor;
    using Sirenix.Utilities;
    using Assets.Scripts.EnemySystem;

    public class EnemyEditorWindow:OdinMenuEditorWindow
    {
        [MenuItem("Window/EnemyEditorWindow")]
        private static void OpenWindow()
        {
            GetWindow<EnemyEditorWindow>().position
                = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }
        //[SerializeField]
        //private NormalZombie normalZombie = new NormalZombie();
        //[SerializeField]
        //private PoliceZombie policeZombie = new PoliceZombie();
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                //{ "Zombie/NormalZombie",normalZombie },
                //{ "Zombie/PoliceZombie",policeZombie },
            };
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;
            tree.SortMenuItemsByName();

            EnemyOverView.Instance.UpdateEnemyOverview();

            tree.Add("Zombie", new EnemyTable(EnemyOverView.Instance.AllEnemys));

            tree.AddAllAssetsAtPath("Zombie", "Assets/ScriptableObjects/Enemy", typeof(EnemySO), true);

            tree.AddAllAssetsAtPath("Item", "Assets/ScriptableObjects/Item", typeof(Item), true);

            tree.AddAllAssetsAtPath("Skill", "Assets/ScriptableObjects/Skill", typeof(SkillSO), true);

            // Add drag handles to items, so they can be easily dragged into the inventory if characters etc...
            tree.EnumerateTree().Where(x => x.Value as Item).ForEach(AddDragHandles);

            // Add icons to characters and items.
            tree.EnumerateTree().AddIcons<Item>(x => x.Icon);
            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
                {
                    Debug.LogWarning("Create Item not implement");
                    //ScriptableObjectCreator.ShowDialog<Item>("Assets/Plugins/Sirenix/Demos/Sample - RPG Editor/Items", obj =>
                    //{
                    //    obj.Name = obj.name;
                    //    base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    //});
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Enemy")))
                {
                    Debug.LogWarning("Create Enemy not implement");

                    //ScriptableObjectCreator.ShowDialog<Character>("Assets/Plugins/Sirenix/Demos/Sample - RPG Editor/Character", obj =>
                    //{
                    //    obj.Name = obj.name;
                    //    base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    //});
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }

}
#endif
