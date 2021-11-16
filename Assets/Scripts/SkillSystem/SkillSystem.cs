using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sel.SkillSystem
{
    public class SkillSystem
    {
        private static SkillServer skillServer;
        private static SkillServer savedServer;
        public SkillSystem()
        {
            var iceSkill = new IceBulletSkill();
            iceSkill.InitSkill();
            skillServer = new NormalSkillServer(iceSkill);
        }

        //XXX 测试技能系统，输入不应该在这
        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                skillServer.UseSlot1(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                skillServer.UseSlot2(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                skillServer.UseSlot3(3);
            }
        }
        public static SkillServer GetSkillServer()
        {
            return skillServer;
        }
        public static void ShutDownSkillServer()
        {
            savedServer = skillServer;
            skillServer = new NullSkillServer();
        }
        //public static IBulletSkill GetBulletSkill()
        //{
        //    return skillServer?.GetBulletSkill();
        //}
    }

}
