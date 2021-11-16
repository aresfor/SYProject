using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sel.SkillSystem
{
    public class NormalSkillServer : SkillServer
    {
        public NormalSkillServer(IBulletSkill bulletSkill)
        {
            this.Slot1 = bulletSkill;
        }
    }
}
