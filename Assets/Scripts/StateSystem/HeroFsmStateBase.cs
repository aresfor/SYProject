using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFsmStateBase
{
    public HeroFsmStateTypes stateTypes;

    public string stateName;

    public int priority;

    public void SetState(string stateName,HeroFsmStateTypes stateTypes,int priority)
    {
        this.stateName = stateName;
        this.stateTypes = stateTypes;
        this.priority = priority;
        
    }
    public virtual HeroFsmStateTypes GetConflictTypes()
    {
        return HeroFsmStateTypes.None;
    }

    public bool CheckConflictStatePriorityHigher(int priority)
    {
        if (priority > this.priority)
            return true;
        return false;
    }
    public bool CanEnter(StackFsmState stackFsm)
    {
        if (stackFsm.GetTopState() == null) { return true; }
        if (!stackFsm.CheckConflictState(this.GetConflictTypes()))
        {
            return true;
        }
        return false;
    }
    
    public virtual void OnEnter(StackFsmState stackFsm) { }
    public virtual void OnExit(StackFsmState stackFsm) { }
    public virtual void OnRemove(StackFsmState stackFsm) { }
}
