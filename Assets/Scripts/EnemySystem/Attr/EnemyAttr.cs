using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EnemySystem
{
    public class EnemyAttr:ICharacterAttr
    {
        //添加Enemy的特有属性

        public EnemyAttr()
        {

        }
        public EnemyAttr(int currentHP,int maxHP,float moveSpeed,string attrName)
        {
            this.MaxHP = maxHP;
            this.MoveSpeed = moveSpeed;
            this.CurrentHP = currentHP;
            this.AttrName = attrName;
        }
    }
}
