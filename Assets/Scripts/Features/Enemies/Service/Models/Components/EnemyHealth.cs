using UniRx;

namespace Features.Enemies
{
    public sealed class EnemyHealth : IEnemyComponent
    {
        private readonly ReactiveProperty<int> m_Health;

        public EnemyHealth(int health)
        {
            m_Health = new ReactiveProperty<int>(health);
        }

        public IReadOnlyReactiveProperty<int> Health => m_Health;

        public void SetHpDecrease(int damage)
        {
            m_Health.Value -= damage;
        }
    }
}