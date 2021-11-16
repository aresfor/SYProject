using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EnemySystem
{
    public abstract class ICharacterBuilder
    {
        public abstract void SetParam(ICharacterBuildParam param);
        public abstract void LoadAsset(int characterID);
        public abstract void AddAI();
        public abstract void SetCharacterAttr();
        public abstract void AddToCharacterList(int characterID);
    }
}
