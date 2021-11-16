using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class Grenade:MonoBehaviour
{
    [SerializeField] private float ExpiredTime;
    [SerializeField] private float ExplodeRadius;
    [SerializeField] private float ExplodePower;
    [SerializeField] private GameObject ExplodeEffect;
    
    public Grenade(float expiredTime)
    {
        this.ExpiredTime = expiredTime;
        TimerManager.Instance.SetTimer(new Action(Explode), ExpiredTime);
    }
    public void Explode()
    {
        //广播这个爆炸
        EventCenter.Broadcast<Grenade>(EventDefine.GrenadeExplode, this);
        
        //播放爆炸效果

        //对周围敌人造成伤害，AOE效果，可结合八叉树
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, ExplodeRadius);
    }
#endif
}
