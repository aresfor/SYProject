using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sel.SkillSystem
{
    public abstract class IBulletSkill:ISkill
    {
        public TrailRenderer trail;
        public int extraDamage;
        public GameObject bulletEffect;
        
        public virtual void RenderBullet(Bullet bullet)
        {
            SetTrailRender(bullet);
            SetExtraDamage(bullet);
            SetBulletEffect(bullet);
        }
        protected abstract void SetTrailRender(Bullet bullet);
        protected abstract void SetExtraDamage(Bullet bullet);
        protected abstract void SetBulletEffect(Bullet bullet);

        //....其他效果
    }

}
