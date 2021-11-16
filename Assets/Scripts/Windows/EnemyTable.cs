#if UNITY_EDITOR
namespace Assets.Scripts.Windows
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Serialization;
    using Sirenix.Serialization;
    using Sirenix.OdinInspector;

    public class EnemyTable
    {
        [FormerlySerializedAs("AllEnemys")]
        [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
        private readonly List<EnemyWrapper> AllEnemys;

        public EnemyTable(IEnumerable<EnemySO> enemys)
        {
            this.AllEnemys = enemys.Select(x => new EnemyWrapper(x)).ToList();
        }
        public EnemySO this[int index]
        {
            get { return this.AllEnemys[index].Enemy; }
        }
        private class EnemyWrapper
        {
            private EnemySO enemy;
            public EnemySO Enemy => this.enemy;
            public EnemyWrapper(EnemySO enemy)
            {
                this.enemy = enemy;
            }

            [ShowInInspector]
            public int MaxHP
            {
                get { return this.enemy.MaxHP; }
                set { this.enemy.MaxHP = value; EditorUtility.SetDirty(this.enemy); }
            }

            [ShowInInspector]
            public float MoveSpeed
            {
                get { return this.enemy.MoveSpeed; }
                set { this.enemy.MoveSpeed = value; EditorUtility.SetDirty(this.enemy); }
            }

        }


    }
}
#endif
