using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
[TaskDescription("身上着火就后退")]
public class AfraidFire : Action
{
    public SharedBool IsFired;
    public override TaskStatus OnUpdate()
    {
        if(IsFired.Value)
        {
            //播放着火后退动画，

            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}
