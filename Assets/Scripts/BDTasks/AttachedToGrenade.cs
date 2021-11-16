using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

[TaskDescription("被手雷吸引")]
public class AttachedToGrenade : Action
{
    public bool HasInterestedGrenade = false;
    public float InterestedDistance = 8f;
    public Grenade CurrentInterestedGrenade;
    public override void OnAwake()
    {
        EventCenter.AddListener<Grenade>(EventDefine.GrenadeInstantiate, FindNewGrenade);
    }
    void FindNewGrenade(Grenade grenade)
    {
        if(!HasInterestedGrenade)
        {
            if (Vector3.Distance(transform.position, grenade.transform.position) <= InterestedDistance)
            {
                HasInterestedGrenade = true;
                CurrentInterestedGrenade = grenade;
                EventCenter.RemoveListener<Grenade>(EventDefine.GrenadeInstantiate, FindNewGrenade);
                EventCenter.AddListener<Grenade>(EventDefine.GrenadeExplode, GrenadeExplode);
            }
        }else
        {
            return;
        }
    }
    void GrenadeExplode(Grenade grenade)
    {
        if(grenade.Equals(CurrentInterestedGrenade))
        {
            HasInterestedGrenade = false;
            //炸不死的话还要重新添加监听
            //EventCenter.AddListener<Grenade>(EventDefine.GrenadeInstantiate, FindNewGrenade);

            EventCenter.RemoveListener<Grenade>(EventDefine.GrenadeExplode, GrenadeExplode);
        }
    }
    public override TaskStatus OnUpdate()
    {
        if (HasInterestedGrenade)
        {
            //距离远播放奔跑动画，设置导航Destination

            //距离近了就播放攻击动画

            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
