using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EnemySystem
{
    public abstract class ICharacterAttr
    {
        public int CurrentHP = 0;
        public int MaxHP = 0;
        public float MoveSpeed = 1.0f;
        public string AttrName = "";

        protected IAttrStrategy AttrStrategy = null;

        //public int GetHP() => CurrentHP;
        //public int GetMaxHP() => MaxHP;
        //public float GetMoveSpeed() => MoveSpeed;
        //public string GetAttrName() => AttrName;
        //public IAttrStrategy GetAttrStrategy() => AttrStrategy;
        public void SetAttrStrategy(IAttrStrategy attrStrategy)
        {
            this.AttrStrategy = attrStrategy;
        }
        public virtual void InitAttr()
        {
            this.AttrStrategy?.InitAttr(this);
        }
        public void FullHP()
        {
            this.CurrentHP = this.MaxHP;
        }
        //在伤害计算上的一些函数写在这里
        
    }
}
