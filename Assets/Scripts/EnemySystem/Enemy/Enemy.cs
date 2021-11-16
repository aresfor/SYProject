
namespace Assets.Scripts.EnemySystem
{
    public class Enemy : ICharacter
    {
        public bool isDead = false;
        public virtual void GetAttack(int damage)
        {

        }
    }
}
