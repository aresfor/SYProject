using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EnemySystem
{
    using UnityEngine;
    public class CharacterFactory:ICharacterFactory
    {
        private CharacterBuilderSystem BuilderSys = new CharacterBuilderSystem();

        public override Enemy CreateEnemy(EnemyType enemyType,Vector3 spawnPosition,string assetName)
        {
            EnemyBuildParam enemyBuildParam = new EnemyBuildParam();

            switch(enemyType)
            {
                case EnemyType.PoliceZombie:
                    enemyBuildParam.Character = new PoliceZombie();
                    break;
                case EnemyType.NormalZombie:
                    enemyBuildParam.Character = new NormalZombie();
                    break;
                default:
                    Debug.LogWarning("无法创建" + Enum.GetName(typeof(EnemyType), enemyType));
                    break;
            }

            if(enemyBuildParam.Character == null) { return null; }

            //设置共享参数
            enemyBuildParam.SpawnPosition = spawnPosition;
            enemyBuildParam.AssetName = assetName;
            enemyBuildParam.EnemyType = enemyType;

            //设置建造者参数
            EnemyBuilder enemyBuilder = new EnemyBuilder();
            enemyBuilder.SetParam(enemyBuildParam);

            //开始生产
            BuilderSys.Construct(enemyBuilder);
            return enemyBuildParam.Character as Enemy;
        }
    }
}
