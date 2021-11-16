using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sel.SkillSystem
{
    public class FireBulletSkill:IBulletSkill
    {
        //TODO 火焰子弹特有的属性


        public override void InitSkill()
        {
            //TODO 从配置中读取初始化技能数值
        }
        protected override void SetTrailRender(Bullet bullet)
        {
            Debug.LogWarning("执行火焰子弹技能，改变子弹轨迹渲染，但是还没有实现");
        }
        protected override void SetExtraDamage(Bullet bullet)
        {
            Debug.LogWarning("执行火焰子弹技能，子弹额外伤害，但是还没有实现");
        }
        protected override void SetBulletEffect(Bullet bullet)
        {
            Debug.LogWarning("执行火焰子弹技能，子弹效果，但是还没有实现");
        }
    }
}
