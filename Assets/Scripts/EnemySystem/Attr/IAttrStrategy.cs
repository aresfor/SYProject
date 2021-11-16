using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EnemySystem
{
    public abstract class IAttrStrategy
    {
        public abstract void InitAttr(ICharacterAttr attr);
    }
}
