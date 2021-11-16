using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EnemySystem
{
    using UnityEngine;
    public class EnemyStrategy:IAttrStrategy
    {
        public override void InitAttr(ICharacterAttr attr)
        {
            //通过策略去修改属性
            Debug.LogWarning("EnemyStrategy没有实现，不过执行InitAttr了");
        }
    }
}
