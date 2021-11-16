using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EnemySystem
{
    public class EnemyBuildParam:ICharacterBuildParam
    {
        //添加Enemy特有的Param
        public EnemyType EnemyType;
        //追踪玩家目标的实现？
        public EnemyBuildParam()
        {
        }
    }
}
