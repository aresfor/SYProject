using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Sel.SkillSystem
{
    public abstract class ISkill
    {
        public SkillFireType FireType;
        public bool bIsEffecting;
        public bool bIsCooling;
        public float Duration;
        public float CoolDown;
        public abstract void InitSkill();
        public virtual void SetNotCooling()
        {
            Debug.Log("冷却完毕");
            bIsCooling = false;
        }
        public virtual void SetNotEffecting()
        {
            Debug.Log("持续时间结束");
            bIsEffecting = false;
        }
    }
}
