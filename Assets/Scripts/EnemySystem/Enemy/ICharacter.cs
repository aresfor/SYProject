using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.Scripts.EnemySystem
{
    public abstract class ICharacter
    {
        public GameObject gameObject;
        public NavMeshAgent agent;

        public ICharacterAttr CharacterAttr;
        public virtual void SetCharacterAttr(ICharacterAttr characterAttr)
        {
            this.CharacterAttr = characterAttr;
            this.CharacterAttr.InitAttr();

            agent.speed = this.CharacterAttr.MoveSpeed;
        }
    }
}
