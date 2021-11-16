using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.Scripts.EnemySystem
{
    //TODO 读取asset资产可以交给资产工厂
    public class EnemyBuilder:ICharacterBuilder
    {
        private EnemyBuildParam Param = null;
        public override void SetParam(ICharacterBuildParam param)
        {
            this.Param = param as EnemyBuildParam;
        }
        public override void LoadAsset(int characterID)
        {
            //TODO Addressable加载未完成
            //从资产中加载模型
            GameObject prefab = null;
            if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/" + this.Param.AssetName + ".prefab");
            }else
            {
                //从Addressable中加载
            }
            if(prefab == null)
            {
                Debug.LogError("加载模型失败");
                return;
            }
            GameObject instance = GameObject.Instantiate(prefab, Param.SpawnPosition, Quaternion.identity);

            //设置物体名字为模型名字+characterID
            instance.name = this.Param.AssetName + "_" +characterID;

            //设置Enemy在GameManager列表中的ID供其他模块使用
            instance.AddComponent<Unit>();
            instance.GetComponent<Unit>().InstanceID = characterID;

            //设置Param中的实际对象的模型为加载的模型
            this.Param.Character.gameObject = instance;
        }
        public override void AddAI()
        {
            //TODO 行为树？
            //添加导航AI，唤醒
            this.Param.Character.gameObject.AddComponent<NavMeshAgent>();
            this.Param.Character.agent 
                = this.Param.Character.gameObject.GetComponent<NavMeshAgent>();
            
        }
        public override void SetCharacterAttr()
        {
            //TODO 属性在编辑器配置好，获取属性后赋值给Param.Character，Param.Character.SetCharacterAttr
            //TODO Addressable加载未完成
            EnemySO so = null;
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                so = AssetDatabase.LoadAssetAtPath<EnemySO>("Assets/ScriptableObjects/Enemy/" + this.Param.AssetName + ".asset");
            }
            else
            {
                //从Addressable中加载
            }
            if(so == null)
            {
                Debug.LogError("加载属性asset失败");
                return;
            }
            EnemyAttr enemyAttr = new EnemyAttr(so.CurrentHP, so.MaxHP, so.MoveSpeed, so.AttrName);
            EnemyStrategy enemyStrategy = new EnemyStrategy();
            enemyAttr.SetAttrStrategy(enemyStrategy);
            this.Param.Character.SetCharacterAttr(enemyAttr);
        }
        public override void AddToCharacterList(int characterID)
        {
            GameManager.Instance.AddEnemy(characterID, this.Param.Character as Enemy);
        }
    }
}
