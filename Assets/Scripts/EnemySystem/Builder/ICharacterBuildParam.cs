using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EnemySystem
{
    public abstract class ICharacterBuildParam
    {
        public ICharacter Character;
        public Vector3 SpawnPosition;
        public int AttrID;
        public string AssetName;
    }
}
