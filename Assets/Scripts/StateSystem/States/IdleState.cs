using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : HeroFsmStateBase
{
    public HeroFsmStateTypes conflictTypes =
        HeroFsmStateTypes.RePluse | HeroFsmStateTypes.Dizziness |
        HeroFsmStateTypes.Striketofly | HeroFsmStateTypes.Sneer |
        HeroFsmStateTypes.Fear;
    public override HeroFsmStateTypes GetConflictTypes()
    {
        return conflictTypes;
    }
    public IdleState()
    {
        this.stateTypes = HeroFsmStateTypes.Idle;
        this.stateName = "Anim_Idle";
        this.priority = 1;
    }
}
