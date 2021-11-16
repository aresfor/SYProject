using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sel.SkillSystem
{
    public interface IBulletServer:IBaseServer
    {
        public abstract IBulletSkill GetBulletSkill();
    }

}
