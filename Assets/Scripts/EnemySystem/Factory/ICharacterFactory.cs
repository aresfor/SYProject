
namespace Assets.Scripts.EnemySystem
{
    using UnityEngine;
    public abstract class ICharacterFactory
    {
        public abstract Enemy CreateEnemy(EnemyType enemyType,Vector3 spawnPosition,string assetName);
    }
}