namespace Features.Enemies
{
    public sealed class EnemiesFactory : IEnemiesFactory
    {
        public EnemyModel CreatEnemy()
        {
            EnemyModel result = new EnemyModel();
            result.AddComponent(new EnemyPosition());
            result.AddComponent(new EnemyHealth(EnemiesConstants.EnemiesParams.Hp));

            return result;
        }
    }
}