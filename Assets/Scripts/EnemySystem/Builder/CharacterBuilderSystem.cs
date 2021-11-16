using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EnemySystem
{
    public class CharacterBuilderSystem:ICharacterBuilderSystem
    {
        private int CharacterID = 0;
        public override void Construct(ICharacterBuilder builder)
        {
            builder.LoadAsset(CharacterID);
            builder.AddAI();
            builder.SetCharacterAttr();
            builder.AddToCharacterList(CharacterID);
            CharacterID++;
        }
    }
}
