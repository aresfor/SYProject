using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackFsmState
{
    public LinkedList<HeroFsmStateBase> FsmStateBases
        = new LinkedList<HeroFsmStateBase>();
    public Dictionary<HeroFsmStateTypes,List<HeroFsmStateBase>> statesHave
        = new Dictionary<HeroFsmStateTypes, List<HeroFsmStateBase>>();
    public HeroFsmStateBase GetState(string stateName)
    {
        foreach(var state in FsmStateBases)
        {
            if(state.stateName.Equals(stateName))
            {
                return state;
            }
        }
        return null;
    }
    private bool CheckIsFirstState(HeroFsmStateBase state)
    {
        return state == this.GetTopState().Value;
    }
    public bool ChangeState(HeroFsmStateBase state)
    {
        HeroFsmStateBase tempState = this.GetState(state.stateName);
        if(tempState != null)
        {
            this.InsertState(tempState,isContainSelf:true);
            return CheckIsFirstState(tempState);
        }
        this.InsertState(state);
        return CheckIsFirstState(state);
    }
    public bool ChangeState<T>(HeroFsmStateTypes types,string stateName,int priority)
        where T:HeroFsmStateBase,new()
    {
        HeroFsmStateBase state = this.GetState(stateName);

        if(state != null)
        {
            this.InsertState(state, true);
            return CheckIsFirstState(state);
        }
        state = new HeroFsmStateBase();
        state.SetState(stateName, types, priority);
        this.InsertState(state);
        return CheckIsFirstState(state);
    }
    public void InsertState(HeroFsmStateBase stateToInsert, bool isContainSelf = false,bool forceInsert = false)
    {
        //if(!forceInsert)
        //{
        //    if (!this.canInsert(_state)) { return; }
        //}
        if (!stateToInsert.CanEnter(this)) { return; }
        LinkedListNode<HeroFsmStateBase> currentStateNode = this.GetTopState();
        while (currentStateNode != null)
        {
            if (stateToInsert.priority >= currentStateNode.Value.priority)
            {
                break;
            }
            currentStateNode = currentStateNode.Next;
        }

        HeroFsmStateBase tempTopState = this.GetTopState().Value;
        //检查一个buff叠加问题？
        if(isContainSelf)
        {
            if (stateToInsert.stateName == currentStateNode.Value.stateName)
            {
                //如果自己在这个位置代表优先级没有变化，不做插入
                return;
            }else
            {
                FsmStateBases.Remove(stateToInsert);
                FsmStateBases.AddBefore(currentStateNode, stateToInsert);
            }
        }else
        {
            if(currentStateNode !=null)
            {
                this.FsmStateBases.AddBefore(currentStateNode, stateToInsert);
            }else
            {
                this.FsmStateBases.AddLast(stateToInsert);
            }

            if(this.statesHave.TryGetValue(stateToInsert.stateTypes,out var stateList))
            {
                stateList.Add(stateToInsert);
            }else
            {
                this.statesHave.Add(stateToInsert.
                    stateTypes, new List<HeroFsmStateBase>() { stateToInsert });
            }

            if(this.GetTopState().Value == stateToInsert)
            {
                tempTopState?.OnExit(this);
                stateToInsert.OnEnter(this);
            }
        }
    }
    public bool HasAbsoluteEqualsState(HeroFsmStateTypes targetStateTypes)
    {
        foreach (var state in this.statesHave)
        {
            if (targetStateTypes == state.Key && state.Value.Count > 0)
            {
                return true;
            }
        }

        return false;
    }
    public bool CheckConflictState(HeroFsmStateTypes types)
    {
        foreach(var state in statesHave)
        {
            if ((types & state.Key) == state.Key && state.Value.Count > 0)
            {
                return true;
            }
        }
        return false;
    }
    public void RemoveState(string stateName)
    {
        HeroFsmStateBase state = this.GetState(stateName);
        if(state == null) { return; }

        bool isRemoveStateIsCurrentState = state == FsmStateBases.Last.Value;
        this.statesHave[state.stateTypes].Remove(state);
        this.FsmStateBases.Remove(state);
        state.OnRemove(this);
        if(isRemoveStateIsCurrentState)
        {
            this.GetTopState()?.Value.OnEnter(this);
        }
    }
    public LinkedListNode<HeroFsmStateBase> GetTopState()
    {
        return FsmStateBases.First;
    }
    public LinkedListNode<HeroFsmStateBase> GetCurrentState() => GetTopState();
}
