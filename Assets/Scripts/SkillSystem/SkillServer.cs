using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sel.SkillSystem
{
    //需要对某个系统进行技能影响就声明并添加对应的服务接口,还要添加对应的技能如IBulletServer对应IBulletSkill，IBulletSkill是实际实现技能的接口，可以替换技能
    //因为服务需要对外开放，而每个模块比如武器只需要子弹类型的技能，用SkillServer去赋值IBulletServer就可以只获得自己的功能

    //同时SkillServer可以关闭，新生成一个NullServer赋值给SkillSystem的SkillServer就可以
    //别的模块执行但实际不产生效果，不用做特殊的判断，有的模块需要的话可以用IsOpen去判断

    //为什么做成服务的形式？
    //1.可以让各个模块通过接口只获取自己想要的服务，减少在方法上面的选择
    //2.可以做一些特殊的玩法，有的怪物可以禁用玩家的技能，这时候关闭技能服务就可以了(其实可以直接让UI不能交互？)
    //或者可以让玩家获得一套额外的暂时的强力的技能

    //目前设计是一个终极技能槽，两个小技能槽
    public abstract class SkillServer:IBulletServer
    {
        //IBulletSkill bulletSkill->ISkill slot1,slot2,slot3, slot3为终极技能槽
        protected ISkill Slot1;
        protected ISkill Slot2;
        protected ISkill Slot3;

        protected bool isOpen;
        public virtual bool IsOpen()
        {
            return isOpen;
        }
        public void UseSlot1(int slotNum)
        {
            if(Slot1.bIsCooling) { return; }
            SetSlot(Slot1);
        }
        public void UseSlot2(int slotNum)
        {
            if (Slot2.bIsCooling) { return; }
            SetSlot(Slot2);
        }
        public void UseSlot3(int slotNum)
        {
            if (Slot3.bIsCooling) { return; }
            SetSlot(Slot3);
        }
        private void SetSlot(ISkill slot)
        {
            if (slot.FireType == SkillFireType.Passive)
            {
                //TODO 如果是被动技能应该在初始化的时候将bIsEffecting设置为true
                return;
            }
            else if (slot.FireType == SkillFireType.ManuelActive)
            {
                slot.bIsEffecting = !slot.bIsEffecting;
            }
            else if (slot.FireType == SkillFireType.Active)
            {
                slot.bIsEffecting = true;
                TimerManager.Instance.SetTimer(new Action(slot.SetNotCooling), slot.CoolDown);
                TimerManager.Instance.SetTimer(new Action(slot.SetNotEffecting), slot.Duration);
            }
        }
        

        public virtual IBulletSkill GetBulletSkill()
        {
            if(!Slot1.bIsEffecting) { return null; }
            return Slot1 as IBulletSkill;
        }

        
    }
}
