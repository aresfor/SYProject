using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sel.SkillSystem
{
    public class IceBulletSkill : IBulletSkill
    {
        //TODO 冰冻子弹特有的属性
        public GameObject BulletEffect;

        public override void InitSkill()
        {
            //TODO 从配置中读取初始化技能数值
            if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                BulletSkillSO bulletSkill= AssetDatabase.LoadAssetAtPath<BulletSkillSO>("Assets/ScriptableObjects/Skill/BulletSkill/" + "IceBulletSkill" + ".asset");
                this.BulletEffect = bulletSkill.BulletEffect;
                this.extraDamage = bulletSkill.ExtraDamage;
                this.FireType = bulletSkill.FireType;
                this.CoolDown = bulletSkill.CoolDown;
                this.Duration = bulletSkill.Duration;
            }
            else
            {
                //TODO Addressable
            }
        }
        protected override void SetTrailRender(Bullet bullet)
        {
            Debug.LogWarning("执行冰冻子弹技能，改变子弹轨迹渲染，但是还没有实现");
        }
        protected override void SetExtraDamage(Bullet bullet)
        {
            Debug.LogWarning("执行冰冻子弹技能，子弹额外伤害，但是还没有实现");
        }
        protected override void SetBulletEffect(Bullet bullet)
        {
            bullet.BulletEffect = BulletEffect;
        }
    }
}
