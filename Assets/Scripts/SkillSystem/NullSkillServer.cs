using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sel.SkillSystem
{
    public class NullSkillServer : SkillServer
    {
        public override bool IsOpen()
        {
            return false;
        }

    }
}
