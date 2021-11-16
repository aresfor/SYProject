using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sel.SkillSystem
{
    //��Ҫ��ĳ��ϵͳ���м���Ӱ�����������Ӷ�Ӧ�ķ���ӿ�,��Ҫ��Ӷ�Ӧ�ļ�����IBulletServer��ӦIBulletSkill��IBulletSkill��ʵ��ʵ�ּ��ܵĽӿڣ������滻����
    //��Ϊ������Ҫ���⿪�ţ���ÿ��ģ���������ֻ��Ҫ�ӵ����͵ļ��ܣ���SkillServerȥ��ֵIBulletServer�Ϳ���ֻ����Լ��Ĺ���

    //ͬʱSkillServer���Թرգ�������һ��NullServer��ֵ��SkillSystem��SkillServer�Ϳ���
    //���ģ��ִ�е�ʵ�ʲ�����Ч����������������жϣ��е�ģ����Ҫ�Ļ�������IsOpenȥ�ж�

    //Ϊʲô���ɷ������ʽ��
    //1.�����ø���ģ��ͨ���ӿ�ֻ��ȡ�Լ���Ҫ�ķ��񣬼����ڷ��������ѡ��
    //2.������һЩ������淨���еĹ�����Խ�����ҵļ��ܣ���ʱ��رռ��ܷ���Ϳ�����(��ʵ����ֱ����UI���ܽ�����)
    //���߿�������һ��һ�׶������ʱ��ǿ���ļ���

    //Ŀǰ�����һ���ռ����ۣܲ�����С���ܲ�
    public abstract class SkillServer:IBulletServer
    {
        //IBulletSkill bulletSkill->ISkill slot1,slot2,slot3, slot3Ϊ�ռ����ܲ�
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
                //TODO ����Ǳ�������Ӧ���ڳ�ʼ����ʱ��bIsEffecting����Ϊtrue
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
