using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
[TaskDescription("玩家跑怪物就会兴奋")]
public class CheckPlayerRunAway : Conditional
{
    public Vector3 PrePlayerPosition;
    public SharedVector3 PlayerPosition;

    public float PursueDistance;
    public float RefreshPrePositionInternel = 2f;

    public override void OnAwake()
    {
        System.Action temp = () => this.RefreshPrePosition();
        TimerManager.Instance.SetTimer(temp, 0f, true, true, RefreshPrePositionInternel);
    }
    public override void OnStart()
    {
        RefreshPrePosition();
    }
    void RefreshPrePosition()
    {
        PrePlayerPosition = PlayerPosition.Value;
    }

    public override TaskStatus OnUpdate()
    {
        if(Vector3.Distance(PrePlayerPosition,PlayerPosition.Value) >=  PursueDistance)
        {
            return TaskStatus.Success;
            //接下来会进入追击Action
        }

        return TaskStatus.Failure;
    }
}
