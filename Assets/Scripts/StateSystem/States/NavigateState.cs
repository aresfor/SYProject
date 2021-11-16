using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigateState :HeroFsmStateBase
{
    public HeroFsmStateTypes conflictStateTypes = 
        HeroFsmStateTypes.RePluse | HeroFsmStateTypes.Dizziness |
        HeroFsmStateTypes.Striketofly | HeroFsmStateTypes.Sneer |
        HeroFsmStateTypes.Fear;

    NavMeshAgent agent;

    public NavigateState(NavMeshAgent agent)
    {
        this.stateTypes = HeroFsmStateTypes.Run;
        this.stateName = "Anim_Run";
        this.priority = 1;
        this.agent = agent;
    }
    public override HeroFsmStateTypes GetConflictTypes()
    {
        return conflictStateTypes;
    }
    public override void OnExit(StackFsmState stackFsm)
    {
        agent.isStopped = true;
    }
}
