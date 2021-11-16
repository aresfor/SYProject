using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
[TaskDescription("Idle条件判断")]
public class MyIdle : Conditional
{
    public SharedVector3 Destination;
    public override void OnStart()
    {
        GetComponent<AnimationComponent>().PlayIdleFromStart();
    }
    public override TaskStatus OnUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Destination = hit.point;
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Running;
    }
}
