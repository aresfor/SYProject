using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Animancer;
using UnityEngine.AI;

[TaskDescription("跑动Action")]
public class MyRun : Action
{
    public SharedVector3 Destination;
    AnimancerState state;
    NavMeshAgent agent;
    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public override void OnStart()
    {
        state = GetComponent<AnimationComponent>().PlayAnimFromStart(HeroFsmStateTypes.Run);
        agent.SetDestination(Destination.Value);
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
                agent.SetDestination(hit.point);
            }
        }
        if (Vector3.Distance(agent.destination,transform.position) < 0.1f && !Destination.Value.Equals(Vector3.zero))
            { return TaskStatus.Success; }
        return TaskStatus.Running;
    }
}