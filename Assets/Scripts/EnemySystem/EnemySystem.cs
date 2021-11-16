using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.EnemySystem
{
    public class EnemySystem
    {
        [DictionaryDrawerSettings(KeyLabel = "实例ID",ValueLabel ="敌人",IsReadOnly = false)]
        public Dictionary<int, Enemy> EnemyDic = new Dictionary<int, Enemy>();
        public void AddEnemy(int key, Enemy enemy)
        {
            if (!EnemyDic.ContainsKey(key))
                EnemyDic.Add(key, enemy);
            else
                Debug.LogWarning("敌人拥有相同的InstanceID无法加入敌人列表");
        }
        public void GetAttack(int key,int damage)
        {
            if(EnemyDic.ContainsKey(key))
                EnemyDic[key].GetAttack(damage);
        }
    }
}
